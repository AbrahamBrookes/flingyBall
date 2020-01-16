using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using flingyball;


public class btn_quit : MonoBehaviour, IPointerClickHandler {

	public GameMode game;




	public void OnPointerClick( PointerEventData data){
		Debug.Log( "quitting" );
		// this object was clicked - do something
		Application.Quit();
	}   
}
