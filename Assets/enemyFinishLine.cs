using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFinishLine : MonoBehaviour {

	private string logStr;
	private int enemyCounter;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "enemy") {
			collision.gameObject.GetComponent<floatyShip>().crashAndBurn();
			enemyCounter++;
		}
	}

	void OnGUI() {
		GUIStyle myStyle = new GUIStyle ();
		myStyle.fontSize = 42;
		GUI.Box (new Rect (100, 100, 200, 80), enemyCounter.ToString(), myStyle);
	}
}
