using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using flingyball;


public class TutorialAnimations : MonoBehaviour {

	private GameMode game;
	public Text tutorialText;

	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Manager").GetComponent<GameMode> ();
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	public void step1(){
		gameObject.GetComponent<Animation> ().Play ("tutorial-step1");
	}

	public void step2() {
		tutorialText.text = "Once at the bottom of the screen you can aim by dragging anywhere";
		gameObject.GetComponent<Animation> ().Play ("tutorial-step2");
	}

	public void step3() {
		gameObject.GetComponent<Animation> ().Play ("tutorial-step3");
		tutorialText.text = "Release the shot to fire!";
	}

	public void step4() {
		gameObject.GetComponent<Animation> ().Play ("tutorial-step4");
		tutorialText.text = "Set a more powerful shot by dragging from further up the screen";
	}
}
