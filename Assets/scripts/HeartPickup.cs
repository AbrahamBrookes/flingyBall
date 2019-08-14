using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class HeartPickup : MonoBehaviour {

	private GameMode hearts;
	public float movementSpeed;
	public float bobFrequency;
	public float bobAmplitude;
	public GameObject shadowHack;
	private RaycastHit shadowHit;

	// Use this for initialization
	void Start () {
		hearts = GameObject.Find ("Manager").GetComponent<GameMode> ();
		shadowHack = Instantiate (shadowHack, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		// move towards the camera, bobbing
		Vector3 tempPos = new Vector3();
		// bob
		tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude;
		tempPos.z = Time.deltaTime * movementSpeed;
		transform.Translate( tempPos );


		// cast shadow hack
		Physics.Raycast(transform.position, Vector3.down, out shadowHit, Mathf.Infinity, 1 << 0, QueryTriggerInteraction.UseGlobal);
		shadowHack.transform.position = new Vector3(transform.position.x, shadowHit.point.y + 0.1f, transform.position.z);

		// despawn once past the player
		if( transform.position.z < -150.0f ) Destroy(this.gameObject);
	}


	void OnCollisionEnter( Collision bang ){
		foreach (ContactPoint contact in bang.contacts) {
			Collider other = contact.otherCollider;
			if (other.CompareTag ("killsEnemies")) {
				hearts.gainLife ();
				Destroy (this.gameObject);
			}
		}
	}


	public void OnDestroy(){
		Destroy (shadowHack);
	}
}
