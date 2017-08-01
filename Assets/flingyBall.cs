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
	private int wpnStatus = 0; // 0 = idle, 1 = reloading, 2 = pulling back mode, 3 = aiming mode
	private Vector3 moveBolt = Vector3.zero;
	private float chargeMeter = 0.0f;
	private float touchStart = 0.0f;
	private float chargeHeight = Screen.height * 0.65f;
	private float aimHeight = Screen.height * 0.35f;
	private float distanceDragged = 0.0f;

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

			/*
			 * 		we're going to use the top two thirds of the screen
			 * 		as the shot charging area - the higher up the screen
			 * 		you touch and drag from, the higher the force applied
			 * 		to the bolt.
			 * 
			 * 		once the touch hits the bottom third of the screen then 
			 * 		aim mode takes over, allowing the player to aim up
			 * 		as they drag down further.
			 * 
			 */

			// top 'charge' area
			if (Input.mousePosition.y > aimHeight) { // in charge area
				touchStart = Input.mousePosition.y;
			}

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






			if (Input.mousePosition.y > aimHeight && wpnStatus != 3) { // in charge area
				// once we enter aiming mode we want to be able to swipe back into the charge area while still pointing the weapon
				wpnStatus = 2;
				distanceDragged = touchStart - Input.mousePosition.y;
				chargeMeter = distanceDragged / chargeHeight; // the amount we have pulled the bolt back as a percentage of the full charge allowable
				wpnPosition = new Vector3 (wpnPosition.x - (springVec.normalized.x * 5 * chargeMeter), wpnPosition.y - (springVec.normalized.y * 5 * chargeMeter), wpnPosition.z - (springVec.normalized.z * 5 * chargeMeter));

			}







			if (Input.mousePosition.y <= aimHeight) { // in aim mode area
				wpnStatus = 3;
				wpnPosition = new Vector3 (wpnPosition.x - (springVec.normalized.x * 5 * chargeMeter), wpnPosition.y - (springVec.normalized.y * 5 * chargeMeter), wpnPosition.z - (springVec.normalized.z * 5 * chargeMeter));
			}



			// point the weapon model at the pivot point
			rb.transform.LookAt (wpnPivotRb.position);
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