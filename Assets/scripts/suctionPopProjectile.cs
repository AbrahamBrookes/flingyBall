using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suctionPopProjectile : Projectile {

	public float zCollisionThreshold;

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}


	public override void killEnemy( GameObject enemy ){
		base.killEnemy( enemy );
	}


	public override void OnCollisionEnter( Collision other ){

		if ( other.contacts [0].normal.z > zCollisionThreshold || 
			other.contacts [0].normal.z < ( zCollisionThreshold * -1.0f ) ) { // if we hit dead on, stick to the thingy

			Destroy (gameObject.GetComponent<ConstantForce> ());
			Destroy (gameObject.GetComponent<Rigidbody> ());
			gameObject.transform.SetParent (other.gameObject.transform);

			killEnemy ( other.gameObject );
		}
	}
}
