using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pdMenuMid : MonoBehaviour {

	private bool tutActive;

	public GameObject touchCircle;
	public GameObject leftArrow;
	public GameObject rightArrow;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		/*if (tutActive) { // handle tutorial update

		}*/
		
	}

	void OnMouseDown () {
		tutActive = tutActive ? false : true;

		if (tutActive) { // tutorial activated, prepare for tutorial
			// show touchy thing
			touchCircle.GetComponent<Renderer> ().enabled = true;
			leftArrow.GetComponent<Renderer> ().enabled = true;
			rightArrow.GetComponent<Renderer> ().enabled = true;
		} else {
			// hide touchy thing
			touchCircle.GetComponent<Renderer> ().enabled = false;
			leftArrow.GetComponent<Renderer> ().enabled = false;
			rightArrow.GetComponent<Renderer> ().enabled = false;
		}
	}
}
