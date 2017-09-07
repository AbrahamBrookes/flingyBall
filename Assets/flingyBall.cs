using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]


public class flingyBall : MonoBehaviour 
{

	public static int coinz = 0;
	public static int numEnemies = 0; // how many enemies are on screen, tracked manually so to avoid polling the scene to count
	public static int enemiesKilledThisWave = 0;

	public Camera cam;
	public GameObject controlsPivot;
	public GameObject wpnPivot;
	public GameObject theBall;
	public GameObject theEnemy;
	public GameObject weaponModel;
	public GameObject weaponModelBase;
	public GameObject cog_01;
	public GameObject cog_02;
	public GameObject cog_03;
	public GameObject cog_04;
	public GameObject bigCog;

	public float forceMultiplier = 1000.0f;
	public float zlingDepth = 10.0f;
	public float spawnInterval = 3.0f;
	public float chargeMeterMultiplier = 3.0f;
	private int maxEnemiesThisWave = 4;

	private GameObject springBase;
	private SpringJoint springy;
	private Rigidbody projectileRb;
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
	private Vector3 wpnPosition; // position of our weapon model, to be adjusted throughout the frame and then set at the end
	private Vector3 prjPosition; // position of our projectile model, as above
	private float spawnTimer = 0.0f;
	private int wpnStatus = 0; // 0 = idle, 1 = reloading, 2 = pulling back mode, 3 = aiming mode
	private float chargeMeter = 0.0f;
	private float touchStart = 0.0f;
	private float chargeHeight = Screen.height * 0.65f;
	private float aimHeight = Screen.height * 0.35f;
	private float distanceDragged = 0.0f;
	private SkinnedMeshRenderer wpnSkinMeshRenderer;
	private Vector3 lastWpnPos;
	private Vector3 lastWpnDir;
	private bool firstFrame; // to help with hiding things until they're ready


	// animating the tower cogs
	private bool flingBackCogs = false;
	private bool bigCogRollin = false;
	public float cogBounceTheta = 0; // we'll modulate the the theta to control the speed of the rotation
	public float cogBounceThetaDegredation;
	public float cogBounceAmplitude = 4;
	public float cogBounceAmplitudeDegredation = -0.5f;
	private float cogBounceAmplitudeInternal;
	private float cogBounceAmplitudeDegredationInternal;
	private float cogBounceThetaInternal;
	private float cogBounceThetaDegredationInternal;
	private float rotateAmount;
	public float bigCogTheta;
	private float bigCogThetaInternal;
	public float bigCogThetaDegradation;
	private float bigCogThetaDegradationInternal;
	public float bigCogOffset;


	// wave flipout vars
	public GameObject waveFlipout; // WF_
	public int waveNumber = 1;
	private Transform WF_ones;
	private Transform WF_tens;
	private Transform WF_hunj;
	private Transform WF_thou;
	private float waveMultiplier = 1;











	void Start()
	{
		ctrlsPivotRb = controlsPivot.GetComponent<Rigidbody>();
		wpnPivotRb = wpnPivot.GetComponent<Rigidbody> ();
		spawnTimer = Time.time + spawnInterval;
		wpnSkinMeshRenderer = weaponModel.GetComponent<SkinnedMeshRenderer> ();
		lastWpnPos = weaponModel.transform.position;

		cogBounceThetaInternal = cogBounceTheta;
		cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
		cogBounceAmplitudeInternal = cogBounceAmplitude;
		cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;
		bigCogThetaInternal = bigCogTheta;
		bigCogThetaDegradationInternal = bigCogThetaDegradation;

		Transform signBase = waveFlipout.transform.Find ("Armature").Find ("root").Find ("signbase");
		WF_ones =  signBase.Find("ones");
		WF_tens =  signBase.Find("tens");
		WF_hunj =  signBase.Find("hunjs");
		WF_thou =  signBase.Find("thous");
	}









	void SetWaveNumber(int setTo){

		// clear enemy tracking vars
		numEnemies = 0;
		enemiesKilledThisWave = 0;
		maxEnemiesThisWave = (int) Mathf.Ceil((setTo * waveMultiplier) + 8);
		waveMultiplier += 0.25f;

		// we'll break out each number by using divisors
		int ourInt = setTo % 10;
		WF_ones.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 6),90.0f );

		if (setTo > 9) {
			ourInt = setTo / 10 % 10;
			WF_tens.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
		} else WF_tens.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);

		if (setTo > 99) {
			ourInt = setTo / 100 % 10;
			WF_hunj.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
		} else WF_hunj.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);

		if (setTo > 999) {
			ourInt = setTo / 1000 % 10;
			WF_thou.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
		} else WF_thou.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);

		waveFlipout.GetComponent<Animator>().Play("reveal");
	}










	void Update()
	{
		//CodeProfiler.Begin ("GameLoopUpdate");
		//SetWaveNumber (waveNumber);

		// handle user input 

		if (Input.GetMouseButtonDown (0)) {      //		player started touching the screen this frame
			firstFrame = true;
			wpnStatus = 2;

			// cancel cog animations
			flingBackCogs = false;
			cogBounceThetaInternal = cogBounceTheta;
			cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
			cogBounceAmplitudeInternal = cogBounceAmplitude;
			cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;

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
			wpnPosition = weaponModel.transform.position; 
			// spawn the projectile
			curBall = Instantiate(theBall, wpnPosition, Quaternion.Euler(curPivot));
			// hide the projectile
			curBall.GetComponent<Renderer>().enabled = false;
			firstFrame = true;
			// freeze the projectiles rigidbody
			projectileRb = curBall.GetComponent<Rigidbody>();
			projectileRb.isKinematic = true;
		}













		if (Input.GetMouseButtonUp (0)) {     //      LOOSE!

			// animate cogs
			flingBackCogs = true;
			bigCogRollin = true;

			// save the aiming data so we can place our arrow again
			lastWpnPos = new Vector3(wpnPosition.x, wpnPosition.y, wpnPosition.z);

			if (wpnStatus == 2 || wpnStatus == 3) {

				wpnStatus = 0;

				// launch the projectile
				projectileRb.isKinematic = false;
				projectileRb.AddForce (Vector3.Scale (springVec, new Vector3 (forceMultiplier * chargeMeter, forceMultiplier * chargeMeter, forceMultiplier * chargeMeter)));
				projectileRb.gameObject.tag = "killsEnemies";
				chargeMeter = 0.0f;

				// animnate wpn lol
				wpnSkinMeshRenderer.SetBlendShapeWeight (0, 0);

			}
			
		}





















		if (Input.GetMouseButton (0)) { // 		AIM!

			if (firstFrame == true) {
				curBall.GetComponent<Renderer> ().enabled = false;
			} else {
				curBall.GetComponent<Renderer> ().enabled = true;
			}

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
			wpnPosition = wpnPivotRb.position;// - springVec;






















			if (Input.mousePosition.y > aimHeight && wpnStatus != 3) { // DRAW!!
				
				// once we enter aiming mode we want to be able to swipe back into the charge area without returning to charge mode

				wpnStatus = 2;
				distanceDragged = touchStart - Input.mousePosition.y;
				chargeMeter = distanceDragged / chargeHeight; // the amount we have pulled the bolt back as a percentage of the full charge allowable
				//wpnPosition = new Vector3 (wpnPosition.x - (springVec.normalized.x * 0.001f), wpnPosition.y - (springVec.normalized.y * 0.001f), wpnPosition.z - (springVec.normalized.z * 0.001f));
				// animate the weapon mesh
				wpnSkinMeshRenderer.SetBlendShapeWeight (0, chargeMeter*100);
				// place the projectile
				projectileRb.position = lastWpnPos;
				projectileRb.transform.LookAt (wpnPivotRb.position);
				// move the projectile back in accordance with the chargeMeter
				prjPosition = wpnPosition + ((projectileRb.transform.forward.normalized * -1) * (chargeMeter * chargeMeterMultiplier));
				projectileRb.position = prjPosition;

				// animate cogs

				cog_01.transform.localEulerAngles = new Vector3(chargeMeter*200.0f, 90.0f, 90.0f);
				cog_02.transform.localEulerAngles = new Vector3(chargeMeter*-170.0f, 90.0f, 90.0f);
			}







			if (Input.mousePosition.y <= aimHeight ) { // in aim mode area
					// this will only run on the first frame in aim area, as we won't have changed the wpnStatus yet


				wpnStatus = 3;
			}

			if (wpnStatus == 3) {
				wpnPosition = new Vector3 (wpnPosition.x - (springVec.normalized.x * 0.0001f), wpnPosition.y - (springVec.normalized.y * 0.0001f), wpnPosition.z - (springVec.normalized.z * 0.0001f));


				// point the weapon model at the pivot point
				weaponModel.transform.position = wpnPosition;
				weaponModel.transform.LookAt (wpnPivotRb.position);
				// move the base as well
				weaponModelBase.transform.rotation = Quaternion.Euler(-90.0f, weaponModel.transform.rotation.eulerAngles.y, 0.0f);
				// place the projectile
				projectileRb.transform.position = wpnPosition;
				projectileRb.transform.LookAt (wpnPivotRb.position);

				prjPosition = wpnPosition + ((projectileRb.transform.forward.normalized * -1) * (chargeMeter * chargeMeterMultiplier));
				projectileRb.transform.position = prjPosition;

				// animate the cogs according to movement of ballista
				cog_03.transform.localEulerAngles = new Vector3( 180.0f, springVec.x*25, 0.0f);
				cog_04.transform.localEulerAngles = new Vector3( 0.0f, springVec.y*25, 0.0f);
			}





			firstFrame = false;

		}


		// end handle user input












		// spawn enemies
		if (spawnTimer < Time.time) {
			// limit the amount of enemies on screen
			if (numEnemies < maxEnemiesThisWave) {
				GameObject newShip = Instantiate (theEnemy, new Vector3 (Random.Range (-60.0f, 60.0f), Random.Range (30.0f, 100.0f), Random.Range (140.0f, 180.0f)), transform.rotation);
				spawnTimer = Time.time + spawnInterval;
				newShip.transform.LookAt (ctrlsPivotRb.position);
				numEnemies++;
			}
			if(enemiesKilledThisWave == maxEnemiesThisWave){
				waveNumber++;
				SetWaveNumber (waveNumber);

			}
		}
















		// handle animating the cogs decoupled from user input
		// animate the cogs flinging back
		if(flingBackCogs == true){
			// we'll make these cogs sprang back around the end of their rotation point, like there's a bit of elasticity in the mechanism
			// we'll do this by lerping a decaying sine wave

			cogBounceThetaInternal += cogBounceThetaDegredationInternal;
			cogBounceAmplitudeInternal += cogBounceAmplitudeDegredationInternal;

			if(cogBounceAmplitudeInternal >  0)
				rotateAmount = cogBounceAmplitudeInternal * Mathf.Sin (cogBounceThetaInternal);
			
			if (cogBounceAmplitudeInternal <= 0) {
				// cancel cog animations
				flingBackCogs = false;
				cogBounceThetaInternal = cogBounceTheta;
				cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
				cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;
				cogBounceAmplitudeInternal = cogBounceAmplitude;
			}

			cog_01.transform.eulerAngles = new Vector3(rotateAmount,90f,90f);
			cog_02.transform.eulerAngles = new Vector3(rotateAmount * -0.7f,90f,90f);

		}

		if (bigCogRollin == true) {
			bigCogThetaInternal += bigCogThetaDegradationInternal;
			float t = (Mathf.Cos (bigCogThetaInternal * Mathf.PI * 0.75f) + bigCogOffset) * -1;
			bigCog.transform.eulerAngles = new Vector3 (180.0f, -20.0f, bigCog.transform.eulerAngles.z + t);
			if (bigCogThetaInternal < 0) {
				bigCogRollin = false;
				bigCogThetaInternal = bigCogTheta;
				bigCogThetaDegradationInternal = bigCogThetaDegradation;
			}

		}
















		//CodeProfiler.End ("GameLoopUpdate");


	} // end Update()






















	void OnGUI(){
		GUIStyle myStyle = new GUIStyle ();
		myStyle.fontSize = 42;
		GUI.Box (new Rect (100, 100, 200, 120), coinz.ToString(), myStyle);

	}

}