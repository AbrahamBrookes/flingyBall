using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class endOfRoundScreen : MonoBehaviour {

	private GameMode game;


	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Manager").GetComponent<GameMode> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void startEndOfRoundScreen(){
		game.restartRound ();
	}




	public void finishEndOfRoundScreen(){
		game.PlayGame ();
	}
}
