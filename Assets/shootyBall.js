#pragma strict

	var flingyBall : GameObject;
	var killFlingyLimit : int;
	var flingyBallList = new Array();

function Start () {
	
}

function Update () {

    if(Input.GetMouseButtonDown(0)){
    	//spawn a flingyBall
		var shootyBall = Instantiate(flingyBall);

		// get the rigidbody for applying force
		var rb = shootyBall.GetComponent(Rigidbody) as Rigidbody;

		// add velocity
		var randForce : Vector3 = new Vector3(Random.Range(-1000.0f, 1000.0f), Random.Range(750.0f, 25000.0f), Random.Range(1000.0f, 20000.0f));
		rb.AddForce(randForce);
		// add torque
		var randTorque : Vector3 = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-10.0f, 10.0f), Random.Range(-30.0f, 30.0f));
		rb.AddTorque(randTorque);

		// add to our flingyBallList
		flingyBallList.Push(shootyBall);

		// check how many flingyBalls we have and despawn them after a limit
		if(flingyBallList.length >= killFlingyLimit){
			var killFlingy = flingyBallList.Shift();
			Destroy(killFlingy);
		}

    }
	
}
