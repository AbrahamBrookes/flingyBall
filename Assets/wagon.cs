using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class wagon : PoolableGameObject, i_Notifiable
    {

        public float moveSpeed;
        public float turnSpeed;

        public GameObject splosion;

        public GameObject wagonWaypoints;
        public Transform[] waypoints;

        private MainGameMode flingyball;

        public int curWaypointIndex;

        private Vector3 direction;

        // Start is called before the first frame update
        void Start()
        {
            waypoints = wagonWaypoints.GetComponentsInChildren<Transform>();
            curWaypointIndex = 1;
            flingyball = GameObject.Find("Manager").GetComponent<MainGameMode>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position != waypoints[curWaypointIndex].position)
            {
                // move towards the next waypoint
                transform.position = Vector3.MoveTowards(transform.position, waypoints[curWaypointIndex].position, Time.deltaTime * moveSpeed);

                // turn to face the same as the waypoint transform
                float step = turnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, waypoints[curWaypointIndex].transform.forward, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            { // we are at a waypoint
                curWaypointIndex++;
                if (curWaypointIndex >= waypoints.Length) // we are past the last waypoint
                {
                    flingyball.wagonSaved(this); // report that we are saved
                    return;
                }
                else
                {
                    turnSpeed = waypoints[curWaypointIndex].GetComponent<WagonWaypoint>().turnspeed;
                }
            }

        }



        public void Notify(string notification, GameObject other)
        {
            if( notification == "I shot you")
            {
                if (gameObject.activeSelf) // only if we haven't already been shot (multiple incoming balls)
                {
                    // tell all the enemies to select a new target
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy"); //
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i].GetComponent<floatyShip>().Notify("please acquire a new target", gameObject);
                    }
                    // notify the gamemode that we have been dead, so it can manage its own lists
                    flingyball.Notify("wagon destroyed", gameObject);
                }
            }
        }


        public override void onSpawn()
        {
            curWaypointIndex = 1;
        }

        public override void onStash()
        {
        }
    }
}