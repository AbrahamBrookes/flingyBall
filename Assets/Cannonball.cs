using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour {

	private float spawnTime;

	// Use this for initialization
	void Start () {
		spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// die after 5 seconds
		if (Time.time - spawnTime > 5.0f) {
			Destroy (this.gameObject);
		}
	}
}
