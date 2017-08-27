using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballyBall : MonoBehaviour {

	private float ballTime;
	private int killCount = 0;
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
		if (Time.time - ballTime > 10.0f) {
			Destroy (projectileShadow);
			Destroy (gameObject);
		}

		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 0, QueryTriggerInteraction.UseGlobal);
		projectileShadow.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);
		projectileShadow.transform.eulerAngles = new Vector3 (shadowHit.normal.x, transform.eulerAngles.y, transform.eulerAngles.z);

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
