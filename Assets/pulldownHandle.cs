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


	private float handlePullScalar; // how far down we have pulled the handle as a 0.00f - 1.00f

	public static bool pullingDownMenuHandle = false; // if we are pulling the handle or not

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
	}

	void OnMouseDown(){
		// toggle pulling down handle mode
		pullingDownMenuHandle = true;

		// account for origin offset of handle
		wrldTouch = camCam.ScreenToWorldPoint (new Vector3 (0f, Input.mousePosition.y, wrldTouchDepth));
		originOffset = transform.position.y - wrldTouch.y;
	}

	void OnMouseUp(){
		// toggle pulling down handle mode
		pullingDownMenuHandle = false;
	}
}
