using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private flingyBall flingyBall;
	public GameObject theTower;
	public float spawnTime;
	public float lifeTime;
	public float deathTime;
	public float distanceTravelled;
	public int collisionCount;
	public int killCount; // not all collisions are kills
	public int scoreMultiplier = 1;
	public GameObject projectileShadow;
	private RaycastHit shadowHit;





	// Use this for initialization
	public virtual void Start () {
		// find all the references to the live objects
		theTower  = GameObject.Find( "theTower" );
		flingyBall = GameObject.Find ("Manager").GetComponent<flingyBall> ();

		// record starting state
		spawnTime = Time.fixedTime;

		projectileShadow = Instantiate (projectileShadow, transform.position, Quaternion.identity);
	}




	// Update is called once per frame
	public virtual void Update () {

		if (Time.time - spawnTime > 10.0f) {

			Destroy (projectileShadow);
			Destroy (gameObject);
		}

		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 15, QueryTriggerInteraction.UseGlobal);
		projectileShadow.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);
		projectileShadow.transform.eulerAngles = new Vector3 (shadowHit.normal.x, transform.eulerAngles.y, transform.eulerAngles.z);


	}




	public virtual void OnCollisionEnter(Collision collision){
		collisionCount++;
	}




	public virtual void OnDestroy(){
		Destroy (projectileShadow);
	}





	public virtual void killEnemy( GameObject enemy ){
		// in this case, kamikaze
		killCount++;
		scoreMultiplier++;

		if (killCount == 1) { // first kill
			// calculate times
			deathTime = Time.fixedTime;
			lifeTime = deathTime - spawnTime;

			// distance
			distanceTravelled = Vector3.Distance (transform.position, theTower.transform.position);

			// do fun stuff with that data
		}

		enemy.GetComponent<floatyShip>().die ();
	}






	void spawnMultiplierNumbers( int numbers ){

	}

}
