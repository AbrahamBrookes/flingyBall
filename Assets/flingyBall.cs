using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]


public class flingyBall : MonoBehaviour 
{

	public Camera cam;
	public GameObject pivotPoint;
	public GameObject theBall;
	public GameObject theEnemy;
	public float forceMultiplier = 500;
	public float zlingDepth = 10;
	public float spawnInterval = 3;

	private GameObject springBase;
	private SpringJoint springy;
	private Rigidbody rb;
	private Rigidbody pivrb; // pivot
	private GameObject curBall;
	private GameObject[] projectiles;

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 springVec;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 curPivot;
	private Vector3 ballClamp;
	private int projectileCount = 0;
	private float spawnTimer = 0;

	void Start()
	{
		pivrb = pivotPoint.GetComponent<Rigidbody>();
		spawnTimer = Time.time + spawnInterval;

	}
	void Update()
	{

		// handle user input 

		if (Input.GetMouseButtonDown (0)) {
			// spawn a new projectile
			// find screen co-ords of touch
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
			// convert screen co-ords to a position in the world (the z of which is zlingDepth)
			curPosition = cam.ScreenToWorldPoint(curScreenPoint);
			// clamp the magnitude so our ball can only go so far back
			ballClamp = pivrb.position - Vector3.ClampMagnitude(curPivot, 10.0f);
			curBall = Instantiate (theBall, ballClamp, new Quaternion(0,0,0,0));
			rb = curBall.GetComponent<Rigidbody>();
		}
		if (Input.GetMouseButtonUp (0)) {
			// launch the sphere
			//Debug.Log(springVec);
			rb.AddForce(Vector3.Scale(springVec,new Vector3(forceMultiplier,forceMultiplier,forceMultiplier)));

			
		}
		if (Input.GetMouseButton (0)) {

			// grab the ball
			// find screen co-ords of touch
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
			// convert screen co-ords to a position in the world (the z of which is zlingDepth)
			curPosition = cam.ScreenToWorldPoint(curScreenPoint);
			// get the vector between in-world touch location and pivot point which we place in editor
			curPivot =  pivrb.position - curPosition;
			// clamp the magnitude so our ball can only go so far back
			ballClamp = pivrb.position - Vector3.ClampMagnitude(curPivot, 10.0f);
			// record the vector between our clamped ball and the pivot point
			// we'll use this on release to determine the shot
			springVec = Vector3.ClampMagnitude (curPivot, 10.0f);
			rb.position = ballClamp;
		}

		// end handle user input


		// spawn enemies
		if (spawnTimer < Time.time) {
			GameObject newShip = Instantiate (theEnemy, new Vector3 (Random.Range(-30.0f, 30.0f), Random.Range(30.0f, 100.0f), Random.Range(100.0f, 150.0f)), transform.rotation);
			spawnTimer = Time.time + spawnInterval;
			newShip.transform.LookAt (pivrb.position);
		}

	}


}