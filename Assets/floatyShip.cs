using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatyShip : MonoBehaviour {

	private MeshCollider phy;
	private Rigidbody rb;
	private ConstantForce constForce;

	private float deathWaitDur = 10; // seconds after we crash that we destroy this object
	private float deathTimer;

	private bool goneDown = false; // to track when we've been dealt the fatal blow

	// Use this for initialization
	void Start () {
		phy = gameObject.GetComponent<MeshCollider> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		constForce = gameObject.GetComponent<ConstantForce> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!goneDown) {
			constForce.relativeForce = new Vector3 (0.0f, Mathf.Cos (Time.time), 0.1f);
		} else {
			if (Time.time > deathTimer)
				Destroy (gameObject);
		}
		
	}

	void OnCollisionEnter(Collision collision)
	{
		// crash
		rb.useGravity = true;
		goneDown = true;
		constForce.relativeForce = Vector3.zero;
		// burn

		// die
		deathTimer = Time.time + deathWaitDur;
	}
}
