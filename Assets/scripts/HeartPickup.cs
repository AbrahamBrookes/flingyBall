using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class HeartPickup : PoolableGameObject {

	private GameMode hearts;
	public float movementSpeed;
	public float bobFrequency;
    public float bobAmplitude;

	// Use this for initialization
	void Start () {
		hearts = GameObject.Find ("Manager").GetComponent<GameMode> ();
	}
	
	// Update is called once per frame
	void Update () {
		// move towards the camera, bobbing
		Vector3 tempPos = new Vector3();
		// bob
		tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude;
		tempPos.z = Time.deltaTime * movementSpeed;
		transform.Translate( tempPos );

        // despawn once past the player
        if (transform.position.z < -150.0f) hearts.Notify("heartPickup ready to despawn", gameObject);
	}


	void OnCollisionEnter( Collision bang ){
		foreach (ContactPoint contact in bang.contacts) {
			Collider other = contact.otherCollider;
			if (other.CompareTag ("killsEnemies")) {
				hearts.gainLife ();
                hearts.Notify("heartPickup ready to despawn", gameObject);
            }
		}
	}

    public override void onSpawn()
    {
        
    }

    public override void onStash()
    {
        
    }
}
