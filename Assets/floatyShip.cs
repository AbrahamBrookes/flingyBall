using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 /*
  * When the player kills the floatyship it turns into a destructive earth-bound bomb
  * killing all other enemies it hits, and racking up multiplier scores on the way
  * 
  */


public class floatyShip : MonoBehaviour {

	public GameObject enemyGoal;
	
	private MeshCollider phy;
	private Rigidbody rb;
	private ConstantForce constForce;
	private Object newNum;
	private Light tickyLight;

	private float deathWaitDur = 10; // seconds after we crash that we destroy this object
	private float deathTimer;

	private int numKills; // how many enemies this has killed on its way down
	private int scoreMultiplier = 1; // multiply coinz received on death
	private int otherShipScoreMultiplier;
	private int accumulativeScore = 0; // keep track of how much carnage we've caused as we die
	private int scoreThisRound = 0; // the points to award less our current accumulative score (so the player gets coinz as the carnage happens)

	private bool goneDown = false; // to track when we've been dealt the fatal blow
	private Vector3 hitHere;

	public GameObject shadowHack; // the shadow of this enemy, to cast onto the terrain
	private RaycastHit shadowHit;

	private bool tickingDown = false; // if we are ticking down, to talk to the update function
	private bool tickLerpDirection = true;
	private float tickLerpMe = 0.0f;
	private float tickLerpSpeed = 0.06f;
	private float tickLerpMagnitude = 10f;
	private int tickCounter;

	// layermasks
	private int justEnemiesMask;
	private int justTheTowermask;
	private int combinedMask;

	// Use this for initialization
	void Start () {
		phy = gameObject.GetComponent<MeshCollider> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		constForce = gameObject.GetComponent<ConstantForce> ();
		shadowHack = Instantiate (shadowHack, transform.position, Quaternion.identity);
		tickyLight = transform.Find ("tickyLight").GetComponent<Light>();


		justEnemiesMask = 1 << 13;
		justTheTowermask = 1 << 14;
		combinedMask = justEnemiesMask | justTheTowermask;

	}
	
	// Update is called once per frame
	void Update () {
		//CodeProfiler.Begin ("FloatyShipUpdate");

		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 0, QueryTriggerInteraction.UseGlobal);
		shadowHack.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);

		if (!goneDown) {


		} else {

			if (Time.time > deathTimer) {
				//despawn me
				Destroy (shadowHack);
				Destroy (gameObject);
			}
		}

		if (!goneDown && tickingDown) {


			tickyLight.intensity = tickLerpMe * tickLerpMagnitude;

			if (tickLerpDirection) { // lerping up
				tickLerpMe+=tickLerpSpeed;
				if (tickLerpMe > 1.0f) {
					tickLerpDirection = false;
				}
			} else { // lerping down
				tickLerpMe-=tickLerpSpeed;
				if (tickLerpMe < 0.0f) {
					tickLerpDirection = true;
					tickCounter++;
				}
			}
			if (tickCounter == 7)
				asplode ();
		}

		//CodeProfiler.End ("FloatyShipUpdate");
		
	}



	void FixedUpdate() {

		if (!goneDown && transform.position.z < -10f) {
			Vector3 relativePos = enemyGoal.transform.position - transform.position;
			Quaternion rotMe = Quaternion.LookRotation (relativePos);
			Quaternion startRot = transform.rotation;
			transform.rotation = Quaternion.Lerp (startRot, rotMe, 0.1f);
		}

	}




	void OnCollisionEnter(Collision collision)
	{
		
		if (collision.gameObject.GetComponent<floatyShip> ()) { // hit another ship
			
			floatyShip otherShip = collision.gameObject.GetComponent<floatyShip> ();
			if (otherShip.getGoneDown ()) {
				// otherShip is in kamikaze mode!
				if (!goneDown) { // we are healthy, not for long!
					// accept the otherShip's multiplier
					otherShipScoreMultiplier = otherShip.getScoreMultiplier ();
					scoreMultiplier = scoreMultiplier >= otherShipScoreMultiplier ? scoreMultiplier : otherShipScoreMultiplier; // keep whichever is largest
					scoreMultiplier++;
					otherShip.setScoreMultiplier (scoreMultiplier);
					crashAndBurn ();
				}

			} else { // otherShip is healthy

			}
		
		}
		/*
		if (collision.gameObject.GetComponent<ballyBall> ()) { // hit by projectile

			ballyBall projectile = collision.gameObject.GetComponent<ballyBall> ();

			if (!goneDown) { // we are healthy, not for long!
				// consider the ballyBall's numKills as a multiplier
				scoreMultiplier += projectile.killCount;
				// die
				crashAndBurn ();
			}

		}*/

	}

	public void setScoreMultiplier(int setIt){
		scoreMultiplier = setIt;
	}

	public int getScoreMultiplier(){
		return scoreMultiplier;
	}

	public bool getGoneDown(){
		return goneDown;
	}

	public void crashAndBurn(){
		Destroy(tickyLight);
		// crash
		rb.useGravity = true;
		rb.angularDrag = 0.5f;
		rb.drag = 0.5f;
		rb.mass = 10.0f;
		goneDown = true;
		constForce.relativeForce = Vector3.zero;
		constForce.force = Vector3.zero;
		// tag as killsEnemies for double-up points
		gameObject.tag = "killsEnemies";
		// change collision layer
		gameObject.layer = LayerMask.NameToLayer("projectile");
		// cancel ticking
		tickingDown = false;
		// burn

		// tally
		numKills++;

		// award coinz
		printNumbers (scoreMultiplier);
		flingyBall.coinz += (numKills * scoreMultiplier) - accumulativeScore;
		flingyBall.enemiesKilledThisWave++;

		// die
		deathTimer = Time.time + deathWaitDur;


	}

	public void tickDown(){
		tickingDown = true;
		tickyLight.enabled = true;
	}

	public void asplode(){
		tickingDown = false;

		GameObject splody =  Instantiate(Resources.Load("asplosion"), transform.position, Quaternion.identity) as GameObject;

		crashAndBurn ();

		Collider[] killColliders = Physics.OverlapSphere (transform.position, 16.0f, combinedMask);

		foreach (Collider curCollider in killColliders){
			
			if (curCollider.gameObject.GetComponent<floatyShip> ()) {
				scoreMultiplier++;
				floatyShip curShip = curCollider.gameObject.GetComponent<floatyShip> ();
				curShip.setScoreMultiplier (scoreMultiplier);
				curShip.crashAndBurn ();

			} else {
				
				if (curCollider.gameObject.name == "theTower") {
					playerLoseALife ();
				}
			}

		}


		Collider[] pushColliders = Physics.OverlapSphere (transform.position, 64.0f, justEnemiesMask);

		foreach (Collider curCollider in pushColliders){
			Rigidbody curRB = curCollider.GetComponent<Rigidbody> ();
			curRB.AddExplosionForce (20000.0f, transform.position, 320.0f);
		}


		Destroy (gameObject);
		Destroy (shadowHack);
		Destroy (tickyLight);


	}

	public void printNumbers(int prtScore){
		//if (prtScore < 2) return;
		Vector3 prtPos = transform.position + (Vector3.up * 6);
		newNum = Instantiate(Resources.Load("gui_x"), prtPos, Quaternion.Euler(new Vector3(0.0f,180.0f,0.0f)));

		Destroy (newNum, 2.0f);
		int loopCtr = prtScore.ToString().Length;
		while(prtScore > 0) {
			int curNum = prtScore % 10;
			// figure out where to place the number
			Vector3 numPosition = new Vector3 (prtPos.x + (loopCtr * 4), prtPos.y, prtPos.z);
			// spawn it in
			newNum =  Instantiate(Resources.Load( "gui_" + curNum.ToString() ), numPosition, Quaternion.Euler(new Vector3(0.0f,180.0f,0.0f)));
			// tell it to die after a time
			Destroy (newNum, 2.0f);
			// prepare for next loop iteration
			prtScore /= 10;
			loopCtr--;
		}


	}

	private void playerLoseALife(){
		Debug.Log ("player took a hit!");
	}


}
