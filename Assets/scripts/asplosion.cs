using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asplosion : MonoBehaviour {


	private Transform puffs;
	private Transform boomring;

	private SkinnedMeshRenderer puffSkn;
	private SkinnedMeshRenderer boomringSkn;

	private float puffDelta = 0f;
	private float boomringDelta = 0f;

	// Use this for initialization
	void Start () {

		puffs = gameObject.transform.Find ("puffs");
		boomring = transform.Find ("boomring");
		puffSkn = puffs.GetComponent<SkinnedMeshRenderer> ();
		boomringSkn = boomring.GetComponent<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (boomringDelta < 100) {
			boomringSkn.SetBlendShapeWeight (0, boomringDelta);
			boomringDelta += 8; 
		} else {
			boomring.GetComponent<Renderer> ().enabled = false;
		}

		if (puffDelta < 100) {
			puffSkn.SetBlendShapeWeight (0, puffDelta);
			puffDelta += 10;
		} else if (puffDelta < 200) {
			puffSkn.SetBlendShapeWeight (0, 200 - puffDelta);
			puffSkn.SetBlendShapeWeight (1, puffDelta - 100);
			puffDelta += 2.5f;
		} else {
			Destroy (gameObject);
		}

	}
}
