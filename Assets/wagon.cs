using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class wagon : MonoBehaviour, i_Attackable
    {

        public float moveSpeed;
        public float turnSpeed;

        public GameObject wagonWaypoints;
        private Transform[] waypoints;

        private MainGameMode flingyball;

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
            flingyball = GameObject.Find("Manager").GetComponent<MainGameMode>();
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
            }
            else
            { // we are at a waypoint
                curWaypointIndex++;
                if (curWaypointIndex >= waypoints.Length) // we are past the last waypoint
                {
                    flingyball.wagonSaved(this); // report that we are saved
                    Destroy(this.gameObject); // remove self
                    return;
                }
                else
                {
                    prevWaypoint = curWaypoint;
                    curWaypoint = waypoints[curWaypointIndex];
                    turnSpeed = curWaypoint.GetComponent<WagonWaypoint>().turnspeed;
                }
            }

        }

        public void attack(GameObject attacker)
        {
            Debug.Log("Wagon attacked by someone!");
            Debug.Log(attacker.name);
            Destroy(this.gameObject);
        }

    }
}