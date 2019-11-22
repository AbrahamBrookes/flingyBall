using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class MainGameMode : GameMode
    {

        public wagon wagon;
        public List<wagon> wagons;
        public int wagonsToSave;
        private int wagonsSaved;

        public float wagonSpawnInterval;
        private float nextWagonSpawnTime;

        void Start()
        {
            base.Start();
            wagons = new List<wagon>();
        }



        public void spawnWagon()
        {
            nextWagonSpawnTime = Time.time + wagonSpawnInterval;
            wagon a_wagon = GameObject.Instantiate(wagon);
            a_wagon.gameObject.SetActive(true);
            wagons.Add(a_wagon);
        }



        public void wagonSaved(wagon wagon)
        {
            wagonsSaved++;
            Debug.Log("Wagon saved!");
            wagons.Remove(wagon);
            if (wagonsSaved >= wagonsToSave)
                base.winRound();
        }

        public override void winRound()
        {

            base.winRound();

        }


        public override void Update()
        {

            base.Update();

            if (curGameMode == gameModes.PlayingGame)
            {
                // spawn wagons
                if (nextWagonSpawnTime < Time.time)
                    spawnWagon();
            }
        } // end Update()

        

        public override void cleanupField() {

            // also remove all wagons
            GameObject[] wagons = GameObject.FindGameObjectsWithTag("wagon");
            foreach (GameObject wagon in wagons) {
                Destroy(wagon);
            }

            base.cleanupField();
        }


    }
}