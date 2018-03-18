using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foggyCamera : MonoBehaviour {

	public bool toggleFog;
	private bool fogWas;

	void OnPreRender(){
		//fogWas = RenderSettings.fog;
		RenderSettings.fog = toggleFog;
	}

	void OnPostRender(){
		//RenderSettings.fog = fogWas;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
