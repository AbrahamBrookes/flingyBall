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
		
		if (goneDown == true) { // crashing and burning, take out other enemies on the way
			if (collision.gameObject.tag == "enemy") {
				scoreMultiplier *= 2;
				numKills++;

				scoreThisRound = (numKills * scoreMultiplier) - accumulativeScore;

				accumulativeScore = numKills * scoreMultiplier;

				flingyBall.coinz += scoreThisRound;
			}

		} else if (collision.gameObject.tag == "killsEnemies") {
			crashAndBurn ();
		}

	}

	public bool crashAndBurn(){
		// crash
		rb.useGravity = true;
		rb.angularDrag = 1.0f;
		rb.drag = 2.0f;
		rb.mass = 150.0f;
		goneDown = true;
		constForce.relativeForce = Vector3.zero;
		// tag as killsEnemies for double-up points
		gameObject.tag = "killsEnemies";
		// burn

		// tally
		numKills++;

		// award coinz
		flingyBall.coinz += (numKills * scoreMultiplier) - accumulativeScore;

		// die
		deathTimer = Time.time + deathWaitDur;


		return true;
	}
}
