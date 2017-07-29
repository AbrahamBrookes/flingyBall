using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]


public class flingyBall : MonoBehaviour 
{

	public Camera cam;
	public GameObject pivotPoint;
	public float forceMultiplier = 500;

	private GameObject springBase;
	private SpringJoint springy;
	private Rigidbody rb;
	private Rigidbody pivrb; // pivot

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 springVec;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		pivrb = pivotPoint.GetComponent<Rigidbody>();

	}
	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			// clear all force acting on the gameobject
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

		}
		if (Input.GetMouseButtonUp (0)) {
			// launch the sphere
			//Debug.Log(springVec);
			rb.AddForce(Vector3.Scale(springVec,new Vector3(forceMultiplier,forceMultiplier,forceMultiplier)));
			
		}
		if (Input.GetMouseButton (0)) {

			// grab the ball
			// find screen co-ords of touch
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
			// convert screen co-ords to a position in the world (the z of which is controlled ^)
			Vector3 curPosition = cam.ScreenToWorldPoint(curScreenPoint);
			// get the vector between in-world touch location and pivot point which we place in editor
			Vector3 curPivot =  pivrb.position - curPosition;
			// clamp the magnitude so our ball can only go so far back
			Vector3 ballClamp = pivrb.position - Vector3.ClampMagnitude(curPivot, 10.0f);
			// record the vector between our clamped ball and the pivot point
			// we'll use this on release to determine the shot
			springVec = Vector3.ClampMagnitude (curPivot, 10.0f);
			rb.position = ballClamp;
		}
	}


}