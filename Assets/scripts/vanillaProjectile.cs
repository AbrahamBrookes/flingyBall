using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanillaProjectile : Projectile {

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


	public override void OnCollisionEnter( Collision bang ){

        foreach (ContactPoint contact in bang.contacts)
        {
            floatyShip enemy = contact.otherCollider.gameObject.GetComponent<floatyShip>();

            if (enemy != null)
            { // we have hit an enemy

                /* code for sticking to the enemy if the collision was close to surface normal */
                //if (bang.contacts[0].normal.z > zCollisionThreshold || bang.contacts[0].normal.z < (zCollisionThreshold * -1.0f))
                //{ 
                    if( enemy.NotifyKill(bang)) { // we killed the enemy if true
                        // stick to the enemy
                        Destroy(gameObject.GetComponent<ConstantForce>());
                        Destroy(gameObject.GetComponent<Rigidbody>());
                        gameObject.transform.SetParent(enemy.gameObject.transform);
                    }

                //}

            }
        }
	}
}
