using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpExample : MonoBehaviour {
	public float amplitude = 10f;
	public float ampDegradation = -0.01f;
	public float period = 5f;
	public float periodDegradation = -0.01f;
	public float phaseShift = 0f;

	private float startRot;
	private float initialAmplitude;
	private float initialPeriod;


	protected void Start() {
		startRot = transform.eulerAngles.x;
		initialAmplitude = amplitude;
		initialPeriod = period;
	}

	protected void Update() {

		//reset when we press spacebar
		if (Input.GetKeyDown(KeyCode.Space)) {
			amplitude = initialAmplitude;
			period = initialPeriod;
		}

		amplitude = amplitude > 0f ? amplitude + ampDegradation : 0f;
		period = period > 0f ? period + periodDegradation : 0.001f;

		float theta = (Time.timeSinceLevelLoad / period) + phaseShift;
		float distance = (amplitude * Mathf.Sin (theta));
		transform.eulerAngles = new Vector3 (startRot + distance, 90f, 90f);

	}
}
