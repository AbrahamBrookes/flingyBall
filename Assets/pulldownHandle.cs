using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulldownHandle : MonoBehaviour {

	public GameObject menuItself;
	public float menuPullRatio = 1.25f;
	public float menuOffset = 0.75f;
	public float handleUpperLimit = 0.575f;
	public float handleLowerLimit = 0.4f;

	public static bool pullingDownMenuHandle = false;

	private float touchStartPosY;
	private Vector3 moveHandleThisFrame;
	private Vector3 moveMenuThisFrame;
	private float menuStartPosZ;
	// Use this for initialization
	void Start () {
		moveHandleThisFrame = transform.localPosition;
		moveMenuThisFrame = menuItself.transform.localPosition;
		menuStartPosZ = menuItself.transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		if (pullingDownMenuHandle) {
			moveHandleThisFrame.z = Input.mousePosition.y/1000;
			if (moveHandleThisFrame.z < handleUpperLimit && moveHandleThisFrame.z > handleLowerLimit) { // clamp the pulldown range of the handle
				transform.localPosition = moveHandleThisFrame;
				moveMenuThisFrame.z = menuOffset  + ((Input.mousePosition.y / 1000) * menuPullRatio);
				menuItself.transform.localPosition = moveMenuThisFrame;
			}
		}
	}

	void OnMouseDown(){
		pullingDownMenuHandle = true;
		touchStartPosY = Input.mousePosition.y;
		moveHandleThisFrame = transform.localPosition;
		moveMenuThisFrame = menuItself.transform.localPosition;
	}

	void OnMouseUp(){
		pullingDownMenuHandle = false;
	}
}
