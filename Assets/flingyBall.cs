using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]


public class flingyBall : MonoBehaviour 
{

	public Camera cam;
	public GameObject controlsPivot;
	public GameObject wpnPivot;
	public GameObject theBall;
	public GameObject theEnemy;
	public float forceMultiplier = 1000.0f;
	public float zlingDepth = 10.0f;
	public float spawnInterval = 3.0f;
	public float chargeTimeMultiplier = 2.0f; 

	private GameObject springBase;
	private SpringJoint springy;
	private Rigidbody rb;
	private Rigidbody ctrlsPivotRb; // controls pivot   -- we're decoupling the touch location pivot point
	private Rigidbody wpnPivotRb; // controls pivot		-- from the in-game weapon pivot point
	private GameObject curBall;
	private GameObject[] projectiles;

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 springVec;
	private Vector3 curScreenPoint;
	private Vector3 curPosition;
	private Vector3 curPivot;
	private Vector3 wpnPosition;
	private int projectileCount = 0;
	private float spawnTimer = 0.0f;
	private int wpnStatus = 0; // 0 = idle, 1 = reloading, 2 = pulling back, 3 = at max and straining, 4 = overstrained/out of action
	private Vector3 moveBolt = Vector3.zero;
	private float chargeMeter = 0.0f;

	void Start()
	{
		ctrlsPivotRb = controlsPivot.GetComponent<Rigidbody>();
		wpnPivotRb = wpnPivot.GetComponent<Rigidbody> ();
		spawnTimer = Time.time + spawnInterval;

	}
	void Update()
	{

		// handle user input 

		if (Input.GetMouseButtonDown (0)) {

			wpnStatus = 2;

			// spawn a new projectile
			// find screen co-ords of touch
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
			// convert screen co-ords to a position in the world (the z of which is zlingDepth)
			curPosition = cam.ScreenToWorldPoint(curScreenPoint);
			// clamp the magnitude so our ball can only go so far back
			wpnPosition = wpnPivotRb.position - Vector3.ClampMagnitude(ctrlsPivotRb.position - curPosition, 10.0f); 
			curBall = Instantiate(theBall, wpnPosition, Quaternion.Euler(curPivot));
			rb = curBall.GetComponent<Rigidbody>();
			rb.isKinematic = true;
			// point the weapon model at the pivot point
			curBall.transform.LookAt(ctrlsPivotRb.position);
		}

		if (Input.GetMouseButtonUp (0)) {
			if (wpnStatus == 2 || wpnStatus == 3) {

				wpnStatus = 0;

				// launch the projectile
				rb.isKinematic = false;
				rb.AddForce (Vector3.Scale (springVec, new Vector3 (forceMultiplier * chargeMeter, forceMultiplier * chargeMeter, forceMultiplier * chargeMeter)));
				rb.gameObject.tag = "killsEnemies";
				chargeMeter = 0.0f;
			}
			
		}
		if (Input.GetMouseButton (0)) { // holding down left click / touch

			// track the weapon in 3d
			// find screen co-ords of touch
			curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
			// convert screen co-ords to a position in the world (the z of which is zlingDepth, relative to the camera)
			curPosition = cam.ScreenToWorldPoint (curScreenPoint);
			// get the vector between in-world touch location and pivot point which we place in editor
			curPivot = ctrlsPivotRb.position - curPosition;
			// record the vector between our clamped ball and the pivot point
			// we'll use this on release to determine the shot
			springVec = Vector3.ClampMagnitude (curPivot, 10.0f);
			// clamp the magnitude so our projectile can only be drawn so far back
			wpnPosition = wpnPivotRb.position - springVec;


			if (wpnStatus == 2) { // pulling back the bolt

				// update the charge meter
				if (chargeMeter < 1.0f) {
					
					chargeMeter += chargeTimeMultiplier * Time.deltaTime;
					Debug.Log (chargeMeter);
					// move the bolt backwards
					wpnPosition = new Vector3(wpnPosition.x - (springVec.normalized.x*5 * chargeMeter), wpnPosition.y - (springVec.normalized.y*5 * chargeMeter), wpnPosition.z - (springVec.normalized.z*5 * chargeMeter));

				} else {
					wpnStatus = 3;
				}
			}

			if (wpnStatus == 3) { // at max strain
				wpnPosition = new Vector3(wpnPosition.x - (springVec.normalized.x*5), wpnPosition.y - (springVec.normalized.y*5), wpnPosition.z - (springVec.normalized.z*5));
			}


			// point the weapon model at the pivot point
			rb.transform.LookAt( wpnPivotRb.position );
			// place the projectile
			rb.position = wpnPosition;
		}

		// end handle user input


		// spawn enemies
		if (spawnTimer < Time.time) {
			GameObject newShip = Instantiate (theEnemy, new Vector3 (Random.Range(-30.0f, 30.0f), Random.Range(30.0f, 100.0f), Random.Range(100.0f, 150.0f)), transform.rotation);
			spawnTimer = Time.time + spawnInterval;
			newShip.transform.LookAt (ctrlsPivotRb.position);
		}

	}


}