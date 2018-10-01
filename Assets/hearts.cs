using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hearts : MonoBehaviour {

	private int heartsLeft;
	private List<Transform> heartCircles;

	private flingyBall flingyBall;

	// Use this for initialization
	void Start () {

		flingyBall = GameObject.Find ("Manager").GetComponent<flingyBall> ();
		
		heartCircles = new List<Transform> ();
		foreach (Transform child in transform) {
			heartCircles.Add (child);
			Debug.Log ("adding");
		}


		heartsLeft = heartCircles.Count;
		Debug.Log (heartsLeft);

	}

	void Update(){
	}

	public void loseLife(){
		Debug.Log (heartsLeft);
		heartsLeft--;

		if( heartsLeft < 0 )
			flingyBall.loseGame ();
		
		heartCircles [heartsLeft].GetComponent<Heart>().lose ();
	}

	public void gainLife(){
		Debug.Log (heartsLeft);
		heartCircles [heartsLeft].GetComponent<Heart>().win ();
		heartsLeft++;
	}
}
