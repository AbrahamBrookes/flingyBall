#pragma strict

	var bobRange : int;
	var rb = gameObject.GetComponent(Rigidbody);

function Start () {
	
}

function Update () {
	rb.AddForce(0, Mathf.Sin(Time.time) * bobRange, 0);
}
