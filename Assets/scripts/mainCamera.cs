using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour {

	private flingyBall game;


	// Use this for initialization
	void Start () {
		game= GameObject.Find ("Manager").GetComponent<flingyBall> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void startGame(){
		game.PlayGame ();
	}

	public void startTutorial(){
		game.PlayTutorial ();
	}
}
