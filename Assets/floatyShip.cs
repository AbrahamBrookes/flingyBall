using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 /*
  * When the player kills the floatyship it turns into a destructive earth-bound bomb
  * killing all other enemies it hits, and racking up multiplier scores on the way
  * 
  */


public class floatyShip : MonoBehaviour {

	public GameObject missionGoal;
	public GameObject manager;

	private MeshCollider phy;
	private Rigidbody rb;
	private ConstantForce constForce;
	private Object newNum;

	private float deathWaitDur = 10; // seconds after we crash that we destroy this object
	private float deathTimer;

	private int numKills; // how many enemies this has killed on its way down
	private int scoreMultiplier = 1; // multiply coinz received on death
	private int accumulativeScore = 0; // keep track of how much carnage we've caused as we die
	private int scoreThisRound = 0; // the points to award less our current accumulative score (so the player gets coinz as the carnage happens)

	private bool goneDown = false; // to track when we've been dealt the fatal blow

	// Use this for initialization
	void Start () {
		phy = gameObject.GetComponent<MeshCollider> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		constForce = gameObject.GetComponent<ConstantForce> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!goneDown) {
			constForce.relativeForce = new Vector3 (0.0f, Mathf.Cos (Time.time), 0.1f);
			gameObject.transform.LookAt (missionGoal.transform.position);
		} else {
			if (Time.time > deathTimer) {
				//despawn me
				Destroy (gameObject);
			}
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
					scoreMultiplier = otherShip.getScoreMultiplier ();
					scoreMultiplier++;
					otherShip.setScoreMultiplier (scoreMultiplier);
					crashAndBurn (collision);
				}

			} else { // otherShip is healthy

			}
		
		}

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

	public void crashAndBurn(Collision collision){
		// crash
		rb.useGravity = true;
		rb.angularDrag = 1.0f;
		rb.drag = 1.0f;
		rb.mass = 150.0f;
		goneDown = true;
		constForce.relativeForce = Vector3.zero;
		// tag as killsEnemies for double-up points
		gameObject.tag = "killsEnemies";
		// burn

		// tally
		numKills++;

		// award coinz
		printNumbers (collision, scoreMultiplier);
		flingyBall.coinz += (numKills * scoreMultiplier) - accumulativeScore;

		// die
		deathTimer = Time.time + deathWaitDur;


	}

	public void printNumbers(Collision collision, int prtScore){
		if (prtScore < 2) return;

		newNum = Instantiate(Resources.Load("gui_x"), collision.contacts[0].point, Quaternion.Euler(new Vector3(0.0f,180.0f,0.0f)));

		Destroy (newNum, 2.0f);
		int loopCtr = prtScore.ToString().Length;
		while(prtScore > 0) {
			int curNum = prtScore % 10;
			// figure out where to place the number
			Vector3 numPosition = new Vector3 (collision.contacts[0].point.x + (loopCtr * 4), collision.contacts[0].point.y, collision.contacts[0].point.z);
			// spawn it in
			newNum =  Instantiate(Resources.Load( "gui_" + curNum.ToString() ), numPosition, Quaternion.Euler(new Vector3(0.0f,180.0f,0.0f)));
			// tell it to die after a time
			Destroy (newNum, 2.0f);
			// prepare for next loop iteration
			prtScore /= 10;
			loopCtr--;
		}


	}
}
