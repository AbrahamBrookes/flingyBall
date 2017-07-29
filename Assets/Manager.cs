using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
	
	// public gameobject variables
	public Camera cam;

	// internal gameobject references

	// public variables

	// private variables
	private string logStr = "eyy";


	// Use this for initialization
	void Start () {
		/*if( cam == null ) {
			Debug.Log("camera not found");
		} else {
			Debug.Log("camera found");
		}*/

	}
	
	// Update is called once per frame
	void Update () {

		// check for click, log mouse position
		if (Input.GetMouseButton (0)) {
			logStr = Input.mousePosition.ToString ();
		}

		
	}

	void OnGUI() {
		GUI.Box (new Rect (100, 100, 100, 20), logStr);
	}

}
