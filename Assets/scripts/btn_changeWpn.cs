using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class btn_changeWpn : MonoBehaviour, IPointerClickHandler {

	private flingyBall game;
	public GameObject projectile;


	// Use this for initialization
	void Start () {
		game= GameObject.Find ("Manager").GetComponent<flingyBall> ();
	}

	// Update is called once per frame
	void Update () {

	}



	public void OnPointerClick( PointerEventData data){
		Debug.Log( "Selecting new Wpn" );
		// this object was clicked - do something
		game.theBall = projectile;

	} 
}
