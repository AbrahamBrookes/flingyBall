using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tappyTut : MonoBehaviour {

	// this script is attached to the question mark icon
	// once that icon is clicked we launch tut mode

	private bool tutorialMode = false;
	public GameObject tutTapMesh;

	private Renderer tapRndr;

	// Use this for initialization
	void Start () {
		tapRndr = tutTapMesh.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {


		// handle tutorial
		if (tutorialMode == true) {
			tapRndr.enabled = true;
		} else {
			tapRndr.enabled = false;
		}

	}

	void OnMouseDown () {
		tutorialMode = !tutorialMode;
	}
}
