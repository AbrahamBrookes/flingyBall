using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class Projectile : PoolableGameObject {

	private GameMode flingyBall;
	public GameObject theTower;
	public float spawnTime;
	public float lifeTime;
	public float deathTime;
	public float distanceTravelled;
	public int collisionCount;
	public int killCount; // not all collisions are kills
	public int scoreMultiplier = 1;





	// Use this for initialization
	public virtual void Start () {
		// find all the references to the live objects
		theTower  = GameObject.Find( "theTower" );
		flingyBall = GameObject.Find ("Manager").GetComponent<GameMode> ();


	}




	// Update is called once per frame
	public virtual void Update () {

		if (Time.time - spawnTime > 10.0f) {
            flingyBall.Notify("projectile ready to despawn", gameObject);
		}


	}




	public virtual void OnCollisionEnter(Collision collision){
		collisionCount++;

        MonoBehaviour[] list = collision.gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is i_Attackable)
            {
                i_Attackable breakable = (i_Attackable)mb;
                breakable.attack( gameObject );
            }
        }
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

    public override void onSpawn()
    {
        // record starting state
        spawnTime = Time.fixedTime;
    }

    public override void onStash()
    {
        // record starting state
        spawnTime = Mathf.Infinity;

    }
}
