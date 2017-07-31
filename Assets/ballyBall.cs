using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballyBall : MonoBehaviour {

	private float ballTime;

	// Use this for initialization
	void Start () {
		// record time to check later
		ballTime = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - ballTime > 16.0f) {
			Destroy (gameObject);
		}

	}

}
