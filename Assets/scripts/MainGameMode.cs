using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class MainGameMode : GameMode
    {

        public wagon wagon;
        private List<wagon> wagons;

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





        void Update()
        {

            base.Update();

            if (curGameMode == gameModes.PlayingGame)
            {
                // spawn wagons
                if (nextWagonSpawnTime < Time.time)
                    spawnWagon();
            }
        } // end Update()



    }
}