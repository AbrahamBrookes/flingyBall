using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

	private Vector3 startPos;
	private bool losing;
	private bool winning;

	// Use this for initialization
	void Start () {
		losing = false;
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (losing) {
			transform.Translate (Vector3.up.normalized * Time.deltaTime);
			if (Vector3.Distance (transform.position, startPos) > 3)
				losing = false;
		}

		if (winning) {
			transform.Translate (Vector3.down * Time.deltaTime);
			if (Vector3.Distance (transform.position, startPos) <= 0.5)
				winning = false;
		}
	}

	public void lose(){
		losing = true;
		Debug.Log ("lose a life!");
	}

	public void win(){
		winning = true;
		Debug.Log ("gain a life!");
	}
}
