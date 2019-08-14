using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using flingyball;


public class btn_togglePause : MonoBehaviour, IPointerClickHandler {

	private GameMode game;


	// Use this for initialization
	void Start () {
		game= GameObject.Find ("Manager").GetComponent<GameMode> ();
	}

	// Update is called once per frame
	void Update () {

	}



	public void OnPointerClick( PointerEventData data){
		Debug.Log( "Toggle pause!" );
		// this object was clicked - do something
		game.togglePauseGame();

	} 
}
