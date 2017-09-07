using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballyBall : MonoBehaviour {

	private float ballTime;
	public int killCount = 0;
	public GameObject projectileShadow;
	private RaycastHit shadowHit;

	// Use this for initialization
	void Start () {
		// record time to check later
		ballTime = Time.time;
		projectileShadow = Instantiate (projectileShadow, transform.position, Quaternion.identity);
		
	}


	
	// Update is called once per frame
	void Update () {

		//CodeProfiler.Begin ("BallyBallUpdate");
		if (Time.time - ballTime > 10.0f) {
			
			Destroy (projectileShadow);
			Destroy (gameObject);
		}

		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 15, QueryTriggerInteraction.UseGlobal);
		projectileShadow.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);
		projectileShadow.transform.eulerAngles = new Vector3 (shadowHit.normal.x, transform.eulerAngles.y, transform.eulerAngles.z);


		//CodeProfiler.End ("BallyBallUpdate");

	}




	void OnCollisionEnter(Collision collision){
		
		if (collision.gameObject.GetComponent<floatyShip> ()) {
			
			floatyShip ship = collision.gameObject.GetComponent<floatyShip> ();

			if (ship.getGoneDown () == false) {
				killCount++;
				ship.setScoreMultiplier (killCount);
				ship.crashAndBurn ();
			}

		}
	}



}
