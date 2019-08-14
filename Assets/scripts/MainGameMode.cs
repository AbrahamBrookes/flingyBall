using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flingyball
{
    public class MainGameMode : GameMode
    {

        void Start()
        {
            base.Start();

        }


        public void prePlayGame()
        {
            base.prePlayGame();
        }






        public void PlayGame()
        { // starts the game, either from main menu or after losing a match

            base.PlayGame();

        }




        public void prePlayTutorial()
        {
            base.prePlayTutorial();
        }




        public void PlayTutorial()
        {
            base.PlayTutorial();
        }




        public void backToMenu()
        {
            base.backToMenu();
        }



        public void pauseGame()
        {
            base.pauseGame();
        }

        public void unpauseGame()
        {
            base.unpauseGame();
        }

        public void togglePauseGame()
        {
            base.togglePauseGame();
        }






        public void scorePoints(int points)
        {
            base.scorePoints(points);

        }



        public void spawnRandomPickup()
        {
            base.spawnRandomPickup();
        }


        public void cleanupEnemies()
        {
            base.cleanupEnemies();
        }


        public void cleanupPickups()
        {
            base.cleanupPickups();
        }


        public void cleanupProjectiles()
        {

            base.cleanupProjectiles();
        }



        public void SetWaveNumber(int setTo)
        {
            base.SetWaveNumber(setTo);
        }









        void Update()
        {

            base.Update();


        } // end Update()





        public void loseLife()
        {
            base.loseLife();
        }



        public void gainLife()
        {
            base.gainLife();
        }





        public void loseGame()
        {
            base.loseGame();

        }




        public void restartRound()
        {
            base.restartRound();
        }










        void OnGUI()
        {
            base.OnGUI();

        }

    }
}