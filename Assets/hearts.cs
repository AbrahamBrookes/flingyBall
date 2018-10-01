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
		}


		heartsLeft = heartCircles.Count;

	}

	void Update(){
	}

	public void loseLife(){
		heartsLeft--;

		if( heartsLeft < 0 )
			flingyBall.loseGame ();
		
		heartCircles [heartsLeft].GetComponent<Heart>().lose ();
	}

	public void gainLife(){

		if (heartsLeft >= heartCircles.Count) {
			heartsLeft = heartCircles.Count;
			return;
		}

		heartCircles [heartsLeft].GetComponent<Heart> ().win ();
		heartsLeft++;

	}
}
