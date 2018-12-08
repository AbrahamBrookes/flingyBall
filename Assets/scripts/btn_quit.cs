using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class btn_quit : MonoBehaviour, IPointerClickHandler {

	private flingyBall game;


	// Use this for initialization
	void Start () {
		game= GameObject.Find ("Manager").GetComponent<flingyBall> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnPointerClick( PointerEventData data){
		Debug.Log( "quitting" );
		// this object was clicked - do something
		Application.Quit();
	}   
}
