using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulldownHandle : MonoBehaviour {

	public GameObject menuItself;
	public float menuPullRatio = 1.25f;
	public float menuOffset = 0.75f;
	public float handleUpperLimit = 0.575f;
	public float handleLowerLimit = 0.4f;

	private GameObject spawnTest;
	private Vector3 wrldTouch;
	public Camera camCam;
	private Vector3 handleStartPos;
	private float originOffset;

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

		handleStartPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		
		if (pullingDownMenuHandle) {
			/*moveHandleThisFrame.z = Input.mousePosition.y/1000;
			if (moveHandleThisFrame.z < handleUpperLimit && moveHandleThisFrame.z > handleLowerLimit) { // clamp the pulldown range of the handle
				transform.localPosition = moveHandleThisFrame;
				moveMenuThisFrame.z = menuOffset  + ((Input.mousePosition.y / 1000) * menuPullRatio);
				menuItself.transform.localPosition = moveMenuThisFrame;
			}*/

			wrldTouch = camCam.ScreenToWorldPoint (new Vector3 (0f, Input.mousePosition.y, 11.55f));
			wrldTouch.x = handleStartPos.x;
			wrldTouch.y += originOffset;
			if (wrldTouch.y < 24) {
				wrldTouch.y = 24;
				wrldTouch.z = -141.1199f;
			}
			if (wrldTouch.y > handleStartPos.y) {
				wrldTouch.y = handleStartPos.y;
				wrldTouch.z = handleStartPos.z;
			}
			transform.position = wrldTouch;

		}
	}

	void OnMouseDown(){
		pullingDownMenuHandle = true;
		touchStartPosY = Input.mousePosition.y;
		moveHandleThisFrame = transform.localPosition;
		moveMenuThisFrame = menuItself.transform.localPosition;

		// account for origin offset of handle
		wrldTouch = camCam.ScreenToWorldPoint (new Vector3 (0f, Input.mousePosition.y, 11.55f));
		originOffset = transform.position.y - wrldTouch.y;
	}

	void OnMouseUp(){
		pullingDownMenuHandle = false;
	}
}
