using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyEnemy : Floatable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}


    public virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.contacts[0].thisCollider.name);
    }
}
