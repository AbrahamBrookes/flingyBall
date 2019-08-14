using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using flingyball;


public class btn_how : MonoBehaviour, IPointerClickHandler {

	public GameMode game;


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}



	public void OnPointerClick( PointerEventData data){
		Debug.Log( "How to play" );
		// this object was clicked - do something
		game.prePlayTutorial();
	} 
}
