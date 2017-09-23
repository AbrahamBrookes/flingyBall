using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutTouchCircle : MonoBehaviour {

	private bool isDragging = false;
	private float releaseLerp = 1f; // used to lerp the touchCircle back to starting position when released
	private bool lerpingBack = false; // are we currently lerping?

	private float pullDownAmt; // the amount we have pulled down the screen, for adjusting the blend shapes of the pullDownCircle
	private float pullDownOffset = 0f; // offset from where we touched the pulldownCircle so's to avoid jumping
	private SkinnedMeshRenderer pullySkinned;
	private SkinnedMeshRenderer leftArrowSkinned;
	private SkinnedMeshRenderer rightArrowSkinned;

	public Camera cam;
	public float touchCircleScreenDepth;
	public float pulldownLateralArrowsMultiplier = 13.5f;
	private bool lateralArrowsOut = false;
	private bool springingLateralArrows = false; // while true we will animate our lateral arrows out
	private float springLateralArrowsLerp = 0f; // the lerp value for the above
	private float lateralArrowsReleasePos; // where the lateral arrows were when mouseup event fired (Y pos)

	public GameObject rightArrow;
	public GameObject leftArrow;
	private Vector3 rightArrowStartPos;
	private Vector3 leftArrowStartPos;

	// Use this for initialization
	void Start () {
		rightArrowStartPos = rightArrow.transform.position;
		leftArrowStartPos = leftArrow.transform.position;
		pullySkinned = gameObject.GetComponent<SkinnedMeshRenderer> ();
		leftArrowSkinned = leftArrow.GetComponent<SkinnedMeshRenderer> ();
		rightArrowSkinned = rightArrow.GetComponent<SkinnedMeshRenderer> ();
	}


	void Update () {
		
		if (isDragging) {
			// figure out how much we have pulled down
			pullDownAmt = (cam.pixelHeight - (Input.mousePosition.y + pullDownOffset)) / cam.pixelHeight * 100;
			// clamp that value
			pullDownAmt = pullDownAmt > 100 ? 100 : pullDownAmt;
			pullDownAmt = pullDownAmt < 0 ? 0 : pullDownAmt;

			pullySkinned.SetBlendShapeWeight (0, pullDownAmt);

			float placeLateralArrowsY = leftArrowStartPos.y - ((pullDownAmt / 100) * pulldownLateralArrowsMultiplier);
			leftArrow.transform.position = new Vector3(leftArrow.transform.position.x, placeLateralArrowsY-2f, leftArrowStartPos.z);
			rightArrow.transform.position = new Vector3(rightArrow.transform.position.x, placeLateralArrowsY-2f, rightArrowStartPos.z);

			if (pullDownAmt > 60 && lateralArrowsOut == false) {
				// trigger lateral arrows to spring out
				springingLateralArrows = true;
			}

			if (springingLateralArrows) {
				
				// animate our lateral arrows to pop out from the side of the pulldown arrow
				float sringLateralArrowsAmt = Mathf.Lerp (0.69f, -0.7f, springLateralArrowsLerp);
				leftArrow.transform.position = new Vector3(sringLateralArrowsAmt, leftArrow.transform.position.y, leftArrow.transform.position.z);
				rightArrow.transform.position = new Vector3(sringLateralArrowsAmt*-1, rightArrow.transform.position.y, leftArrow.transform.position.z);

				springLateralArrowsLerp += 0.1f;

				// reset our lerp once exceeded max
				if (springLateralArrowsLerp >= 1) {
					springingLateralArrows = false;
					lateralArrowsOut = true;
					springLateralArrowsLerp = 0f;
				}
			}

			if (lateralArrowsOut) {
				// allow the player to move the lateral arrows
				float lateralArrowScalar = (Input.mousePosition.x - (cam.pixelWidth / 2)) / (cam.pixelWidth / 2);
				rightArrowSkinned.SetBlendShapeWeight (0, lateralArrowScalar*80);
				leftArrowSkinned.SetBlendShapeWeight (0, lateralArrowScalar*-80);
			}

		} else {
			
			if (lerpingBack) {
				// lerp the transform back to start position
				pullySkinned.SetBlendShapeWeight (0, Mathf.Lerp (0f, pullDownAmt, releaseLerp));

				// stash lateral arrows
				float placeLateralArrowsY = Mathf.Lerp (leftArrowStartPos.y-2f, lateralArrowsReleasePos, releaseLerp);
				leftArrow.transform.position = new Vector3(0.69f, placeLateralArrowsY, leftArrowStartPos.z);
				rightArrow.transform.position = new Vector3(-0.69f, placeLateralArrowsY, rightArrowStartPos.z);
				rightArrowSkinned.SetBlendShapeWeight (0, 0);
				leftArrowSkinned.SetBlendShapeWeight (0, 0);
				lateralArrowsOut = false;

				releaseLerp -= 0.1f;

				if (releaseLerp <= -0.5f) {
					lerpingBack = false;
					releaseLerp = 1f;
				}
			}

		}
		
	}

	void OnMouseDown (){
		// drag that prick
		isDragging = true;
		pullDownOffset = cam.pixelHeight - Input.mousePosition.y;
	}

	void OnMouseUp (){
		isDragging = false;
		lerpingBack = true;
		lateralArrowsReleasePos = leftArrow.transform.position.y;
	}
}
