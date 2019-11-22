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

	public void loseLife(){
		hearts.loseLife ();
	}

    public override void Notify(string notification, GameObject other)
    {
        if (notification == "I shot you")
            loseLife();
    }
}
