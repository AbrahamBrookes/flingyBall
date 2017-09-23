using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pdMenuMid : MonoBehaviour {

	private bool tutActive;

	public GameObject touchCircle;
	public GameObject leftArrow;
	public GameObject rightArrow;

	private bool poppingOutTutCircle = false; // to use for animating the tutCircle popping out
	private bool poppingInTutCircle = false;
	private float poppingOutTutCircleLerp = 0f;

	private Vector3 touchCircleStartPos; // the touchCircles hidden resting pos when not popped out
	private Vector3 touchCircleOutPos; // where the touchCircle extends to when shown
	private Vector3 leftArrowStartPos;
	private Vector3 rightArrowStartPos;
	private Vector3 leftArrowOutPos;
	private Vector3 rightArrowOutPos;


	// Use this for initialization
	void Start () {
		touchCircleStartPos = touchCircle.transform.position;
		touchCircleOutPos = touchCircleStartPos;
		touchCircleOutPos.y = touchCircleOutPos.y - 2f;
		leftArrowStartPos = leftArrow.transform.position;
		leftArrowOutPos = leftArrowStartPos;
		leftArrowOutPos.y = leftArrowOutPos.y - 2f;
		rightArrowStartPos = rightArrow.transform.position;
		rightArrowOutPos = rightArrowStartPos;
		rightArrowOutPos.y = rightArrowOutPos.y - 2f;
	}
	
	// Update is called once per frame
	void Update () {

		if (poppingOutTutCircle) { // animate tutCircle

			Vector3 touchCirclePos =  Vector3.Lerp (touchCircleStartPos, touchCircleOutPos, poppingOutTutCircleLerp);
			touchCircle.transform.position = touchCirclePos;
			touchCirclePos =  Vector3.Lerp (leftArrowStartPos, leftArrowOutPos, poppingOutTutCircleLerp);
			leftArrow.transform.position = touchCirclePos;
			touchCirclePos =  Vector3.Lerp (rightArrowStartPos, rightArrowOutPos, poppingOutTutCircleLerp);
			rightArrow.transform.position = touchCirclePos;

			// increment and complete the lerp
			poppingOutTutCircleLerp += 0.04f;
			if (poppingOutTutCircleLerp > 1.1) {
				poppingOutTutCircleLerp = 0f;
				poppingOutTutCircle = false;
			}
		}

		if (poppingInTutCircle) { // animate tutCircle

			Vector3 touchCirclePos =  Vector3.Lerp (touchCircleOutPos, touchCircleStartPos, poppingOutTutCircleLerp);
			touchCircle.transform.position = touchCirclePos;
			touchCirclePos =  Vector3.Lerp (leftArrowOutPos, leftArrowStartPos, poppingOutTutCircleLerp);
			leftArrow.transform.position = touchCirclePos;
			touchCirclePos =  Vector3.Lerp (rightArrowOutPos, rightArrowStartPos, poppingOutTutCircleLerp);
			rightArrow.transform.position = touchCirclePos;

			// increment and complete the lerp
			poppingOutTutCircleLerp += 0.04f;
			if (poppingOutTutCircleLerp > 1.1) {
				poppingOutTutCircleLerp = 0f;
				poppingInTutCircle = false;
			}
		}
		
	}

	void OnMouseDown () {
		pulldownHandle.autoRetractingMenu = true;

		tutActive = tutActive ? false : true;

		if (tutActive) { // tutorial activated, prepare for tutorial
			// show touchy thing
			poppingOutTutCircle = true;
		} else {
			// hide touchy thing
			poppingInTutCircle = true;
		}
	}
}
