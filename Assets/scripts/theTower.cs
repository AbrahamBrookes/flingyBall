using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball;


public class theTower : i_Notifiable {

	private GameMode hearts;

	// Use this for initialization
	void Start () {
		hearts = GameObject.Find ("Manager").GetComponent<GameMode> ();
	}

	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionEnter( Collision bang ){
		foreach (ContactPoint contact in bang.contacts) {
			Collider other = contact.otherCollider;
			if (other.CompareTag ("enemyProjectile")) {
				loseLife ();
			}
		}
	}

	public void loseLife(){
		hearts.loseLife ();
	}

    public override void Notify(string notification, GameObject other)
    {
        if (notification == "I shot you")
            loseLife();

    }
}
