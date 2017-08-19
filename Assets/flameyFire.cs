using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameyFire : MonoBehaviour {

	private Camera sceneCam;

	public float timeCutoff = 5000;
	public float flameTimeStep = 0.5f;

	private float flameTimer = 0;
	private float flameTimerScalar; // to be used to lerp throughout the effect (0.0f - 1.0f)
	private float flameLerpVal = 0;


	public Color flameStartingColor;
	public Color smokeFinalColor;
	private Color smokeEmissionFinalColor = Color.black;
	private Color flameEmissionStartingColor = new Color(1.0f, 0.2689655f, 0.0f);
	private float smokeAdjust;
	private float flameSmokeFade;

	private Vector3 floatDirection;
	private float startingScale;
	private float curScale;

	// Use this for initialization
	void Start () {
		flameTimer = flameTimer;
		timeCutoff = timeCutoff;
		sceneCam = GameObject.Find ("inWorldUICamera").GetComponent<Camera>();






		// randomize the colors a bit
		flameStartingColor.r += Random.Range(flameStartingColor.r - 0.2f, flameStartingColor.r + 0.1f);
		flameStartingColor.g += Random.Range(flameStartingColor.g - 0.45f, flameStartingColor.g + 0.2f);
		// cap them at 1
		flameStartingColor.r = flameStartingColor.r > 1.0f ? 1.0f : flameStartingColor.r;
		flameStartingColor.g = flameStartingColor.g > 1.0f ? 1.0f : flameStartingColor.g;

		// keep the smoke grey
		smokeAdjust = Random.Range(smokeFinalColor.r - 0.1f, smokeFinalColor.r + 0.1f);
		smokeFinalColor.r += smokeAdjust;
		smokeFinalColor.g += smokeAdjust;
		smokeFinalColor.b += smokeAdjust;
		// cap them at 1
		smokeFinalColor.r = smokeFinalColor.r > 1.0f ? 1.0f : smokeFinalColor.r;
		smokeFinalColor.g = smokeFinalColor.g > 1.0f ? 1.0f : smokeFinalColor.g;
		smokeFinalColor.b = smokeFinalColor.b > 1.0f ? 1.0f : smokeFinalColor.b;

		flameEmissionStartingColor.r += Random.Range(flameEmissionStartingColor.r - 0.2f, flameEmissionStartingColor.r + 0.1f);
		flameEmissionStartingColor.g += Random.Range(flameEmissionStartingColor.g - 0.45f, flameEmissionStartingColor.g + 0.1f);
		// cap them at 1
		flameEmissionStartingColor.r = flameEmissionStartingColor.r > 1.0f ? 1.0f : flameEmissionStartingColor.r;
		flameEmissionStartingColor.g = flameEmissionStartingColor.g > 1.0f ? 1.0f : flameEmissionStartingColor.g;




		// find a random direction in which to float, tending upwards
		floatDirection = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(0.4f, 3.2f), Random.Range(-1.0f, 1.0f));
		// random scales
		startingScale = Random.Range(50.0f, 80.0f);
		gameObject.transform.localScale = new Vector3(startingScale, startingScale, startingScale);


	}
	
	// Update is called once per frame
	void Update () {



		// always face the screen
		gameObject.transform.LookAt (sceneCam.transform);






		
		if (flameTimer < timeCutoff) { // WE BURNIN

			flameTimerScalar = flameTimer / timeCutoff; // our location in time

			// fade the material color from flame to smoke
			// we'll need to fade the emission also
			gameObject.GetComponent<Renderer> ().material.color = Color.Lerp(flameStartingColor, smokeFinalColor, flameTimerScalar);
			gameObject.GetComponent<Renderer> ().material.SetColor("_EmissionColor", Color.Lerp(flameEmissionStartingColor, Color.black, flameTimerScalar));

			// animate the flame using a layer of morph targets
			flameLerpVal = Mathf.Sin (flameTimer);
			flameLerpVal *= 1 - flameTimerScalar;
			gameObject.GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (0, flameLerpVal * 100);
			flameSmokeFade = flameTimerScalar;
			gameObject.GetComponent<SkinnedMeshRenderer> ().SetBlendShapeWeight (1, flameSmokeFade * 100);

			// move the flame outwards and upwards
			gameObject.transform.Translate(floatDirection * 0.01f);
			// scale the flame
			curScale = Mathf.Lerp(startingScale, 0.0f, Mathf.Tan(Mathf.Sin(flameTimerScalar + 0.05f)));
			gameObject.transform.localScale = new Vector3(curScale, curScale, curScale);



			flameTimer += flameTimeStep;




		} else {
			Destroy (gameObject);
		}










	}
}
