using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatyShit : MonoBehaviour {

	private flingyBall flingyBall;

	private bool alive; // take a guess
	private float deathTime;

	public float moveSpeed = 1;
	private GameObject EnemyNodes;
	Transform[] EnemyNodeChildren;

	public GameObject shadowHack;
	private RaycastHit shadowHit;

	private GameObject theTower;

	private Transform selectedNode;

	private bool atNode; // ship has arrived at the node

	private bool turningBroadside; // ship is rotating to shoot at the player
	public float turnSpeed;

	public GameObject cannonball;
	public float shootInterval; // the length of time between shots
	private float lastShotTime; // the time of the last cannonball shoot, to compare against shootInterval
	public float inaccuracyRange; // introduce a little bit of inaccuracy to cannonball shots

	// Use this for initialization
	void Start () {
		flingyBall = GameObject.Find ("Manager").GetComponent<flingyBall> ();

		shadowHack = Instantiate (shadowHack, transform.position, Quaternion.identity);


		theTower = GameObject.Find ("theTower");
		alive = true;

		EnemyNodes = GameObject.Find ("EnemyNodes");
		EnemyNodeChildren = EnemyNodes.GetComponentsInChildren<Transform>();
		chooseEnemyNode ();
	}
	
	// Update is called once per frame
	void Update () {
		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 0, QueryTriggerInteraction.UseGlobal);
		shadowHack.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);


		if (alive) {
			if (!atNode) {
				transform.position = Vector3.MoveTowards (transform.position, selectedNode.transform.position, Time.deltaTime * moveSpeed);

				if (!turningBroadside) {

					Vector3 turnTo = selectedNode.position - transform.position;
					float step = turnSpeed * Time.deltaTime;

					Vector3 newDir = Vector3.RotateTowards (transform.forward, turnTo, step, 0.0f);
					transform.rotation = Quaternion.LookRotation (newDir);
				}
			} else {
				// shoot at the tower
				if (Time.time > lastShotTime + shootInterval) {
					lastShotTime = Time.time;
					fireCannon ();
				}
			}

			if (turningBroadside) {
				Vector3 turnTo = new Vector3 (90.0f, 0.0f, 0.0f);
				float step = turnSpeed * Time.deltaTime;

				Vector3 newDir = Vector3.RotateTowards (transform.forward, turnTo, step, 0.0f);
				transform.rotation = Quaternion.LookRotation (newDir);
			}


			if (Vector3.Distance (transform.position, selectedNode.position) < 15) {
				turningBroadside = true;
			} else {
				turningBroadside = false;
			}

			if (Vector3.Distance (transform.position, selectedNode.position) < 5) {
				atNode = true;
			} else {
				atNode = false;
			}

		} else {
			// despawn this object after a time
			if (Time.time - deathTime > 5.0f)
				Destroy (this.gameObject);
		}
	}




	void OnCollisionEnter( Collision bang ){
		foreach (ContactPoint contact in bang.contacts) {
			Collider other = contact.otherCollider;
			if (other.CompareTag ("killsEnemies")) { // DIE!
				if (alive) {
					gameObject.tag = "killsEnemies";
					deathTime = Time.time;
					this.GetComponent<Rigidbody> ().useGravity = true;
					alive = false;
					flingyBall.enemiesKilledThisWave++;
					selectedNode.GetComponent<EnemyNode> ().nodeSelected = false;
				}
			}
		}
	}



	void chooseEnemyNode(){
		// pick a spot in the provided EnemyNodes list
		int randIndex = Mathf.FloorToInt (Random.value * EnemyNodeChildren.Length);
		selectedNode = EnemyNodeChildren [randIndex];
		EnemyNode nodeScript = selectedNode.GetComponent<EnemyNode> ();
		if (nodeScript.nodeSelected) { // node has already been selected, choose a different one
			chooseEnemyNode ();
		} else {
			nodeScript.nodeSelected = true;
		}
			
	}


	void fireCannon(){
		Vector3 shootAt = theTower.transform.position - transform.position;

		// instantiate a cannonball
		GameObject projectile = Instantiate( cannonball, transform.position, Quaternion.identity );
		// look at the tower
		// put a bit of randomness into the direction so we're not like blam on
		Vector3 randy = new Vector3(Random.Range( inaccuracyRange * -1, inaccuracyRange), Random.Range( inaccuracyRange * -1, inaccuracyRange), 0.0f);
		shootAt = shootAt + randy;
		//projectile.transform.LookAt (shootAt);
		// apply an expolsive force
		projectile.GetComponent<Rigidbody>().AddForce( shootAt, ForceMode.Impulse );
	}



	public void OnDestroy(){
		Destroy (shadowHack);
	}
}
