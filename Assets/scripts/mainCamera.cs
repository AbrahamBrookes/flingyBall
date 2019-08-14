using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class mainCamera : MonoBehaviour {

	private GameMode game;


	// Use this for initialization
	void Start () {
		game= GameObject.Find ("Manager").GetComponent<GameMode> ();
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
