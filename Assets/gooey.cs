using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gooey : MonoBehaviour {

	private bool test = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI (){
		if (!test) {
			Debug.Log("gooey");
			test = true;
		}
	}
}
