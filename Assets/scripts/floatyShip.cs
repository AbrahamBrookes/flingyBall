﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class floatyShip : PoolableGameObject, i_Notifiable {

	private MainGameMode flingyBall;
	private GameObject theTower;
    private GameObject manager;

    public bool alive; // take a guess
	private float deathTime;
	public int killCount;
	public int scoreMultiplier = 1;

	public float moveSpeed = 1;
	private GameObject EnemyNodes;
	Transform[] EnemyNodeChildren;


	private Transform selectedNode;

	private bool atNode; // ship has arrived at the node
    private bool lookAtNode = true; // wehter or not the ship should point at the node

	private bool turningBroadside; // ship is rotating to shoot at the player
	public float turnSpeed;

    public GameObject shootyshoot;
    private GameObject my_shootyshoot;
    public GameObject cannonball;
	public List<GameObject> myProjectiles;
	public float shootInterval; // the length of time between shots
	private float lastShotTime; // the time of the last cannonball shoot, to compare against shootInterval
	public float inaccuracyRange; // introduce a little bit of inaccuracy to cannonball shots

    private GameObject shootingAt;

    private CapsuleCollider[] physmeshes;

	// Use this for initialization
	void Start () {
        flingyBall = GameObject.Find("Manager").GetComponent<MainGameMode> ();




		theTower = GameObject.Find ("theTower");
		alive = true;

		EnemyNodes = GameObject.Find ("EnemyNodes");
		EnemyNodeChildren = EnemyNodes.GetComponentsInChildren<Transform>();
		
        physmeshes = GetComponents<CapsuleCollider>();

        chooseEnemyNode();

    }
	
	// Update is called once per frame
	void Update () {
		

		if (alive) {
			if (!atNode) {
				transform.position = Vector3.MoveTowards (transform.position, selectedNode.transform.position, Time.deltaTime * moveSpeed);
                // not at the node, we shouldn't be shooting
                shootingAt = null;

                if (lookAtNode) {

					Vector3 turnTo = selectedNode.position - transform.position;
					float step = turnSpeed * Time.deltaTime;

					Vector3 newDir = Vector3.RotateTowards (transform.forward, turnTo, step, 0.0f);
					transform.rotation = Quaternion.LookRotation (newDir);
				}
			} else {


                if (shootingAt == null) // find something to shoot at
                {
                    // select a wagon or the tower if there are no wagons
                    shootingAt = GetNewTarget();
                }

                if (Time.time > lastShotTime + shootInterval)
                {
                    lastShotTime = Time.time;
                    // spawn a shootyshoot
                    my_shootyshoot = GameObject.Instantiate(shootyshoot, transform.position, Quaternion.identity);
                    my_shootyshoot.transform.SetParent(this.transform); // parent the floatyship to the shootyshoot
                }
            }


            if (my_shootyshoot != null)
            {
                if (shootingAt == null)
                    shootingAt = GetNewTarget();
                // point the shootyshoot at the target
                my_shootyshoot.transform.LookAt(shootingAt.transform);
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

			if (Vector3.Distance (transform.position, selectedNode.position) < 1) {
				atNode = true;
                lookAtNode = false;
            } else {
				atNode = false;
			}

		} else {
            // despawn this object after a time
            if (Time.time - deathTime > 5.0f)
                flingyBall.Notify("floatyShip requesting permission to die, sir", gameObject);
		}
	}


    GameObject GetNewTarget()
    {
        GameObject toReturn;

        GameObject target = GameObject.FindGameObjectWithTag("wagon");

        if ( target == null)
        {
            // no wagons on the field, target the tower
            toReturn = GameObject.Find("theTower");
        } else
        {
            toReturn = target;
        }
        
        return toReturn;
    }


    void OnCollisionEnter( Collision bang ){
    }


    public bool NotifyKill( Collision bang)
    {
        foreach (ContactPoint contact in bang.contacts)
        {
            if (contact.thisCollider.gameObject.GetComponent<Projectile>() != null) // we have been hit by a projectile, or a derived class of Projectile
            {
                // this collision is passed in from the attacking object, 
                // so thisCollider is the projectile
                // and otherCollider is this object
                if (contact.otherCollider == physmeshes[0]) // balloon physmesh hit
                {
                    //Debug.Log("bloon");
                    die();
                    return true;
                }
                //if (contact.otherCollider == physmeshes[1]) // base physmesh hit
                //{
                //    Debug.Log("base");
                //}
            }
        }
        return false;
    }



    public void die(){

        if (my_shootyshoot != null)
            Destroy(my_shootyshoot);

		if (alive) {
			alive = false;
			// go into kamikaze mode
			gameObject.tag = "killsEnemies";
			this.GetComponent<Rigidbody> ().useGravity = true;

			// record your data
			deathTime = Time.time;

            // inform the manager
            flingyBall.enemyKilled();
            awardScore();

			// clean up your shit
            if(selectedNode)
			    selectedNode.GetComponent<EnemyNode> ().nodeSelected = false;
		}
	}


	void awardScore(){
		int score = killCount * scoreMultiplier;
		flingyBall.scorePoints (score);
	}


	void spawnMultiplierNumbers( int numbers ){

	}



	void chooseEnemyNode( int numSearches = 5 ){
		// pick a spot in the provided EnemyNodes list
        if( numSearches == 0)
        {
            Debug.Log("floatyship could not find an enemynode!");
            die();
            return;
        }
		int randIndex = Mathf.FloorToInt (Random.value * EnemyNodeChildren.Length);
		selectedNode = EnemyNodeChildren [randIndex];
		EnemyNode nodeScript = selectedNode.GetComponent<EnemyNode> ();
		if (nodeScript == null || nodeScript.nodeSelected) { // node has already been selected, choose a different one
			chooseEnemyNode ( numSearches -- );
		} else {
			nodeScript.nodeSelected = true;
		}
			
	}


	void fireCannon(){
		GameObject projectile = Instantiate(cannonball, transform.position, Quaternion.identity);
        Cannonball cb = projectile.GetComponent<Cannonball>();
        cb.fire(shootingAt);
	}




    public void Notify(string notification, GameObject other)
    {
        switch (notification)
        {
            case "projectile fired":
                fireCannon();
                if (shootingAt != null)
                shootingAt = GetNewTarget();

                break;
            case "please acquire a new target":
                shootingAt = GetNewTarget();
                break;
            default:
                break;
        }
    }

    public override void onSpawn()
    {
        alive = true;
        atNode = false;
    }

    public override void onStash()
    {
        alive = false;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        if (my_shootyshoot != null)
            Destroy(my_shootyshoot);
    }
}
