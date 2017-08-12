using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballyBall : MonoBehaviour {

	private float ballTime;
	private int killCount = 0;

	// Use this for initialization
	void Start () {
		// record time to check later
		ballTime = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - ballTime > 10.0f) {
			Destroy (gameObject);
		}

	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.GetComponent<floatyShip> ()) { // hit a ship

			floatyShip otherShip = collision.gameObject.GetComponent<floatyShip> ();

			if (otherShip.getGoneDown () == true) { // ship is already dead or dying

			} else if (otherShip.getGoneDown () == false) { // otherShip is healthy, not for long!
				killCount++;
				int shipMultiplier = otherShip.getScoreMultiplier();

				if (shipMultiplier > killCount) {
					otherShip.setScoreMultiplier (shipMultiplier + 1);

				} else {
					otherShip.setScoreMultiplier (killCount);
				}

				otherShip.crashAndBurn (collision);

			}

		}
	}

}
