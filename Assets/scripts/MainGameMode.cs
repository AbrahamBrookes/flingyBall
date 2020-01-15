using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class MainGameMode : GameMode
    {

        public ObjectPool wagonObjectPool;

        public int wagonsToSave;
        private int wagonsSaved;

        public float wagonSpawnInterval;
        private float nextWagonSpawnTime;

        void Start()
        {
            base.Start();
        }



        public void spawnWagon()
        {
            nextWagonSpawnTime = Time.time + wagonSpawnInterval;
            wagonObjectPool.Spawn();
        }



        public void wagonSaved(wagon wagon)
        {
            wagonsSaved++;
            wagonObjectPool.Stash(wagon.gameObject);

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
                {
                    spawnWagon();
                }
            }
        } // end Update()

        

        public override void cleanupField() {
            wagonObjectPool.StashAll();
            base.cleanupField();
        }


        public override void enemyKilled()
        {
            base.enemyKilled();
        }



        protected void OnGUI()
        {
            wagonsSavedUI.text = wagonsSaved.ToString();

            base.OnGUI();

        }


        public void Notify(string notification, GameObject other)
        {
            if( notification == "wagon destroyed")
            {
                wagonObjectPool.Stash(other);

            }

            base.Notify(notification, other);
        }

    }
}