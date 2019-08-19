using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wagon : MonoBehaviour, i_Attackable
{

    public float moveSpeed;
    public float turnSpeed;

    public GameObject wagonWaypoints;
    private Transform[] waypoints;

    private int curWaypointIndex;
    private Transform curWaypoint;
    private Transform prevWaypoint;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = wagonWaypoints.GetComponentsInChildren<Transform>();
        curWaypointIndex = 1;
        curWaypoint = waypoints[curWaypointIndex];
        prevWaypoint = transform;

        // get a vec3 to move along
        //direction = Vector3.(curWaypoint.position, prevWaypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != curWaypoint.position)
        {
            // move towards the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, curWaypoint.position, Time.deltaTime * moveSpeed);

            // turn to face the same as the waypoint transform
            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, curWaypoint.transform.forward, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        } else
        {
            curWaypointIndex++;
            prevWaypoint = curWaypoint;
            curWaypoint = waypoints[curWaypointIndex];
            turnSpeed = curWaypoint.GetComponent<WagonWaypoint>().turnspeed;
        }

    }

    public void attack(GameObject attacker)
    {
        Debug.Log("Attacked by smoeone!");
        Debug.Log(attacker.name);
    }

}
