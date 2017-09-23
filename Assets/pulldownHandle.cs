using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulldownHandle : MonoBehaviour {

	public GameObject menuItself;

	private Vector3 wrldTouch; // our screen youch location translated into world position (screenToWorldPosition)
	public float wrldTouchDepth = 11.2f; // the depth to set our world touch location (for fiddling)
	public Camera camCam; // preferably the inworldUIcamera
	private Vector3 handleStartPos; // where our handle is when scene is loaded
	private Vector3 menuStartPos; // where our menu is when scene is loaded
	private float originOffset; // the offset of our touch location wrt to origin of model
	public float maxHandlePullPos = 24; // the furthest location that the handle can be pulled to (a global position, not an offset)

	private float mouseDownTimestamp; // a timestamp of when mousedown was triggered,for calculating click event *** ONLY ASSIGN Time.TimeSinceLevelLoad ***
	private Vector2 mouseDownLocation;

	private float handlePullScalar; // how far down we have pulled the handle as a 0.00f - 1.00f

	public static bool pullingDownMenuHandle = false; // if we are pulling the handle or not

	public static bool autoRetractingMenu = false;
	private bool autoRetractingMenuFirstFrame = true; // used to set start positions of animateion so we can trigger this from other scripts
	private float autoRetractMenuLerp = 0f;
	private Vector3 handleRetractStartPos; // where the handle was when we began retracting it
	private Vector3 menuRetractStartPos; // where the menu was when we began retracting it

	// Use this for initialization
	void Start () {
		handleStartPos = transform.position;
		menuStartPos = menuItself.transform.position;
	}

	// Update is called once per frame
	void Update () {
		
		if (pullingDownMenuHandle) {

			// calculate where we are in the pull
			handlePullScalar = (handleStartPos.y - transform.position.y) / (handleStartPos.y - maxHandlePullPos);

			// tell the menu itself to be somewhere in relation to the handle pull
			// max pos (1.0 scalar) = 31.3
			// min pos (0.0 scalar) = 43.3
			// range = 31.3 - 43.3 = -12.0
			// these are just values I have pulled out of the inspector
			float menuNewPos = menuStartPos.y + (handlePullScalar * -12f);
			menuItself.transform.position = new Vector3(menuStartPos.x, menuNewPos, menuStartPos.z);

			// get world position for touch
			wrldTouch = camCam.ScreenToWorldPoint (new Vector3 (0f, Input.mousePosition.y, wrldTouchDepth));

			// fudge the shit because I'm lazy
			wrldTouch.x = handleStartPos.x;

			// account for the offset of the location of the users tap on the object from the objects origin
			wrldTouch.y += originOffset; 

			// clamp the handles movement
			if (wrldTouch.y < maxHandlePullPos) {
				wrldTouch.y = maxHandlePullPos;
				wrldTouch.z = -141.1199f;
			}
			if (wrldTouch.y > handleStartPos.y) {
				wrldTouch.y = handleStartPos.y;
				wrldTouch.z = handleStartPos.z;
			}

			// update handle position
			transform.position = wrldTouch;

		}






		if (autoRetractingMenu) { // animate retracting the menu
			if (autoRetractingMenuFirstFrame) {
				handleRetractStartPos = transform.position;
				menuRetractStartPos = menuItself.transform.position;
				autoRetractingMenuFirstFrame = false;
			}
			// animate the menu closed
			Vector3 handlePos = Vector3.Lerp(handleRetractStartPos, handleStartPos, autoRetractMenuLerp);
			transform.position = handlePos;

			Vector3 menuPos = Vector3.Lerp (menuRetractStartPos, menuStartPos, autoRetractMenuLerp);
			menuItself.transform.position = menuPos;

			autoRetractMenuLerp += 0.1f;
			if(autoRetractMenuLerp > 1.1f){
				autoRetractMenuLerp = 0f;
				autoRetractingMenu = false;
				autoRetractingMenuFirstFrame = true;
			}
		}






	}

	void OnMouseDown(){

		// record the timestamp for calculating if this is a click or not
		mouseDownTimestamp = Time.timeSinceLevelLoad;
		// record the position of the touch to check for dragging when deciding if this was a click
		mouseDownLocation = Input.mousePosition;

		// toggle pulling down handle mode
		pullingDownMenuHandle = true;

		// account for origin offset of handle
		wrldTouch = camCam.ScreenToWorldPoint (new Vector3 (0f, Input.mousePosition.y, wrldTouchDepth));
		originOffset = transform.position.y - wrldTouch.y;
	}

	void OnMouseUp(){
		// fire all the stuff we want to happen regardless of click or drag here
		// toggle pulling down handle mode
		pullingDownMenuHandle = false;


		// if we are within the click timer then fire click event
		if (Time.timeSinceLevelLoad - mouseDownTimestamp <= flingyBall.clickTime) {
			// also check that we haven't dragged much
			if (Vector2.Distance (mouseDownLocation, Input.mousePosition) < 20f) {
				onClick ();
				return;
			}
		}

		// anything after this point will only be fired if this was not a click event

	}

	void onClick(){
		if (handlePullScalar > 0.1f) {
			// retract the handle
			autoRetractingMenu = true;
		}
	}
}
