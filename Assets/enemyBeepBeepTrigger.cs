using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBeepBeepTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<floatyShip> ()) { // ship in trigger

			other.GetComponent<floatyShip> ().tickDown();

		}
			

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
