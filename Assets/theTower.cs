using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theTower : MonoBehaviour {

	private flingyBall manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Manager").GetComponent<flingyBall> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionEnter( Collision bang ){
		foreach (ContactPoint contact in bang.contacts) {
			Collider other = contact.otherCollider;
			if (other.CompareTag ("enemyProjectile")) {
				Debug.Log ("Hit!");
			}
		}
	}


}
