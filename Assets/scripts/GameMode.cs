using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



namespace flingyball {
    public class GameMode : MonoBehaviour
    {
        [Header("The Good Stuff")]
        public int coinz = 0;
        public int numEnemies = 0; // how many enemies are on screen, tracked manually so to avoid polling the scene to count
        public List<GameObject> enemyList;
        public int enemiesKilledThisWave = 0;
        public int totalEnemiesKilled = 0;
        public float clickTime = 0.25f; // the number of seconds between mouseDown and mouseUp that will be considered when firing Click
        [Tooltip("Set the length of the hearts array to set the number of lives. Fill with gui elements that will be disabled or w/evs to represent lives")]
        public GameObject[] hearts; // we'll store UI heart elements in an array and disable the ones past life
        private int fullHealth;
        private int curHealth;
        private bool gamePaused;

        [Header("Assets")]
        public Camera cam;
        public GameObject controlsPivot;
        public GameObject wpnPivot;
        public GameObject theBall;
        public GameObject theEnemy;
        public GameObject weaponModel;
        public GameObject weaponModelBase;
        public GameObject cog_01;
        public GameObject cog_02;
        public GameObject cog_03;
        public GameObject cog_04;
        public GameObject bigCog;

        [Header("Weapon Feel")]
        public float forceMultiplier = 1000.0f;
        public float zlingDepth = 10.0f;
        public float spawnInterval = 3.0f;
        public float chargeMeterMultiplier = 3.0f;
        private int maxEnemiesThisWave = 4;

        private GameObject springBase;
        private SpringJoint springy;
        private Rigidbody projectileRb;
        private Rigidbody ctrlsPivotRb; // controls pivot   -- we're decoupling the touch location pivot point
        private Rigidbody wpnPivotRb; // controls pivot		-- from the in-game weapon pivot point
        private GameObject curBall;
        public List<GameObject> projectiles;

        private Vector3 screenPoint;
        private Vector3 offset;
        private Vector3 springVec;
        private Vector3 curScreenPoint;
        private Vector3 curPosition;
        private Vector3 curPivot;
        private Vector3 wpnPosition; // position of our weapon model, to be adjusted throughout the frame and then set at the end
        private Vector3 prjPosition; // position of our projectile model, as above
        private float spawnTimer = 0.0f;
        private int wpnStatus = 0; // 0 = idle, 1 = reloading, 2 = pulling back mode, 3 = aiming mode
        private float chargeMeter = 0.0f;
        private float touchStart = 0.0f;
        private float chargeHeight = Screen.height * 0.65f;
        private float aimHeight = Screen.height * 0.35f;
        private float distanceDragged = 0.0f;
        private SkinnedMeshRenderer wpnSkinMeshRenderer;
        private Vector3 lastWpnPos;
        private Vector3 lastWpnDir;
        private bool firstFrame; // to help with hiding things until they're ready


        // animating the tower cogs
        [Header("Tower Cogs")]
        private bool flingBackCogs = false;
        private bool bigCogRollin = false;
        public float cogBounceTheta = 0; // we'll modulate the the theta to control the speed of the rotation
        public float cogBounceThetaDegredation;
        public float cogBounceAmplitude = 4;
        public float cogBounceAmplitudeDegredation = -0.5f;
        private float cogBounceAmplitudeInternal;
        private float cogBounceAmplitudeDegredationInternal;
        private float cogBounceThetaInternal;
        private float cogBounceThetaDegredationInternal;
        private float rotateAmount;
        public float bigCogTheta;
        private float bigCogThetaInternal;
        public float bigCogThetaDegradation;
        private float bigCogThetaDegradationInternal;
        public float bigCogOffset;


        // wave flipout vars
        [Header("Wave Flipout")]
        public int waveNumber = 1;
        private float waveMultiplier = 1;



        [Header("Pickups")]
        public List<GameObject> pickupTypes;
        public List<GameObject> livePickups;
        public float pickupSpawnInterval;
        private float nextPickupSpawnTime;




        // 2D UI
        [Header("2D UI")]
        public GameObject mainMenu;
        public Text pointsUI;
        public Text waveNumberUI;
        public Text baloonsShotUI;
        public Text wagonsSavedUI;
        public GameObject[] waveFlipoutUI;
        public GameObject[] waveFlipoutUI2;
        public GameObject inGameUIGroup;
        protected enum gameModes { // the game mode we are currently in, for deciding game logic
            MainMenu, // Player is browsing the main menu
            Tutorial, // player is in the tutorial
            PlayingGame, // player is playn gaem frealz
            PlayerDiedScreen, // player just died, and is looking at the recap
            Paused // Game is paused, silly!
        };
        protected gameModes curGameMode;
        public GameObject tutorialScreen;
        public GameObject mainMenuLogo;
        public GameObject shootBalloonsText;
        public GameObject playerIsDeadScreen;

        protected void Start()
        {
            ctrlsPivotRb = controlsPivot.GetComponent<Rigidbody>();
            wpnPivotRb = wpnPivot.GetComponent<Rigidbody>();
            spawnTimer = Time.time + spawnInterval;
            wpnSkinMeshRenderer = weaponModel.GetComponent<SkinnedMeshRenderer>();
            lastWpnPos = weaponModel.transform.position;

            cogBounceThetaInternal = cogBounceTheta;
            cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
            cogBounceAmplitudeInternal = cogBounceAmplitude;
            cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;
            bigCogThetaInternal = bigCogTheta;
            bigCogThetaDegradationInternal = bigCogThetaDegradation;

            fullHealth = hearts.Length;
            curHealth = hearts.Length;

            enemyList = new List<GameObject>();

            nextPickupSpawnTime = Time.time + pickupSpawnInterval;

            mainMenu.GetComponent<Animation>().Play("mainMenu-shootOut");
            tutorialScreen.SetActive(false);
            mainMenu.SetActive(true);
            mainMenuLogo.SetActive(true);
            inGameUIGroup.SetActive(false);

            SetWaveNumber(0);
            curGameMode = gameModes.MainMenu;

        }


        public void prePlayGame() {
            mainMenu.GetComponent<Animation>().Play("mainMenu-slideAway");

            // when this animation ends, it will call PlayGame(), to kick off the festivities.
            // You can see this in the animation editor (ctrl+6) on the anmiation mainCamera-startGame
            cam.GetComponent<Animation>().Play("mainCamera-startGame");
        }




        public void PlayGame() { // starts the game, either from main menu or after losing a match

            tutorialScreen.SetActive(false);
            inGameUIGroup.SetActive(true);
            mainMenuLogo.SetActive(false);

            shootBalloonsText.GetComponent<Animation>().Play("shootThoseBaloonsText-startGame");

            curHealth = fullHealth;
            curGameMode = gameModes.PlayingGame;
            SetWaveNumber(1);

        }




        public void prePlayTutorial() {
            mainMenu.GetComponent<Animation>().Play("mainMenu-slideAway");
            cam.GetComponent<Animation>().Play("mainCamera-startTutorial");

        }




        public void PlayTutorial() {
            mainMenuLogo.SetActive(false);
            tutorialScreen.SetActive(true);
            inGameUIGroup.SetActive(false);
            tutorialScreen.GetComponent<TutorialAnimations>().step1();
            curGameMode = gameModes.Tutorial;
        }




        public void backToMenu() {
            if (curGameMode != gameModes.Paused && curGameMode != gameModes.MainMenu) {
                mainMenu.GetComponent<Animation>().Play("mainMenu-shootOut");
                cam.GetComponent<Animation>().Play("mainCamera-backToMenu");
                tutorialScreen.SetActive(false);

                curGameMode = gameModes.MainMenu;

                SetWaveNumber(0);

                cleanupField();

                inGameUIGroup.SetActive(false);
                mainMenuLogo.SetActive(true);
            }
        }



        public void pauseGame() {
            curGameMode = gameModes.Paused;
            Time.timeScale = 0;
        }

        public void unpauseGame() {
            curGameMode = gameModes.PlayingGame;
            Time.timeScale = 1;
        }

        public void togglePauseGame() {
            if (curGameMode == gameModes.Paused) {
                unpauseGame();
            } else {
                pauseGame();
            }
        }






        public void scorePoints(int points) {
            // accumulate the coinz
            coinz += points;

        }



        public void spawnRandomPickup() {

            // select a random pickup from our list
            int randy = Random.Range(0, pickupTypes.Count);

            // spawn the pickup
            GameObject pickup = Instantiate(pickupTypes[randy], new Vector3(Random.Range(-30.0f, 30.0f), Random.Range(10.0f, 45.0f), Random.Range(160.0f, 180.0f)), Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)));
            livePickups.Add(pickup);
            // set the next spawn interval
            nextPickupSpawnTime = Time.time + pickupSpawnInterval + Random.Range(0.0f, 10.0f);

        }


        public virtual void cleanupField() {
            // kill all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            enemyList.Clear();
            

            GameObject[] cleanables = GameObject.FindGameObjectsWithTag("miscCleanable");
            foreach (GameObject cleanable in cleanables)
            {
                Destroy(cleanable);
            }
            livePickups.Clear();


            GameObject[] kilas = GameObject.FindGameObjectsWithTag("killsEnemies");
            foreach (GameObject kila in kilas)
            {
                Destroy(kila);
            }
            projectiles.Clear();
        }





        public void SetWaveNumber(int setTo) {

            Debug.Log("setting wave to " + setTo.ToString());

            waveNumberUI.text = setTo.ToString();

            // clear enemy tracking vars
            numEnemies = 0;
            enemiesKilledThisWave = 0;
            maxEnemiesThisWave = (int)Mathf.Ceil((setTo * waveMultiplier) + 4);
            waveMultiplier += 0.25f;



            /*
            // spin the numbers on the wave flipout model
            // we'll break out each number by using divisors
            int ourInt = setTo % 10;
            WF_ones.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 6),90.0f );

            if (setTo > 9) {
                ourInt = setTo / 10 % 10;
                WF_tens.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
            } else WF_tens.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);

            if (setTo > 99) {
                ourInt = setTo / 100 % 10;
                WF_hunj.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
            } else WF_hunj.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);

            if (setTo > 999) {
                ourInt = setTo / 1000 % 10;
                WF_thou.localEulerAngles = new Vector3 (180.0f, 36 * (ourInt - 5), 90.0f);
            } else WF_thou.localEulerAngles = new Vector3 (180.0f, 36  * -5, 90.0f);
            // flip him out
            waveFlipout.GetComponent<Animator>().Play("reveal");*/

            if (setTo > 0) {
                if (curGameMode == gameModes.PlayingGame) {

                    foreach (GameObject wf in waveFlipoutUI) {
                        wf.GetComponent<Animation>().Play("waveFlipout-flip_out");
                    }
                    foreach (GameObject wf in waveFlipoutUI2) {
                        wf.GetComponent<Animation>().Play("waveFlipout-flip_out2");
                    }

                    inGameUIGroup.SetActive(true);

                }
            } if (setTo == 1) { // we have just restarted the game
                cleanupField();
            } else if (setTo == 0) { // we have turned the game off
            }
        }

        public virtual void winRound()
        {

            waveNumber++;
            cleanupField();
            SetWaveNumber(waveNumber);

        }







        public virtual void Update()
        {

            if (curGameMode == gameModes.PlayingGame || curGameMode == gameModes.Tutorial) {


                // spawn pickups
                if (nextPickupSpawnTime < Time.time)
                    spawnRandomPickup();


                // handle user input 

                if (Input.GetMouseButtonDown(0)) {      //		player started touching the screen this frame


                    //if( 

                    firstFrame = true;
                    wpnStatus = 2;
                    // cancel cog animations
                    flingBackCogs = false;
                    cogBounceThetaInternal = cogBounceTheta;
                    cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
                    cogBounceAmplitudeInternal = cogBounceAmplitude;
                    cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;

                    /*
             * 		we're going to use the top two thirds of the screen
             * 		as the shot charging area - the higher up the screen
             * 		you touch and drag from, the higher the force applied
             * 		to the bolt.
             * 
             * 		once the touch hits the bottom third of the screen then 
             * 		aim mode takes over, allowing the player to aim up
             * 		as they drag down further.
             * 
             */

                    // top 'charge' area
                    if (Input.mousePosition.y > aimHeight) { // in charge area
                        touchStart = Input.mousePosition.y;
                    }

                    // find screen co-ords of touch
                    curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
                    // convert screen co-ords to a position in the world (the z of which is zlingDepth)
                    curPosition = cam.ScreenToWorldPoint(curScreenPoint);
                    // clamp the magnitude so our ball can only go so far back
                    wpnPosition = weaponModel.transform.position;
                    // spawn the projectile
                    curBall = Instantiate(theBall, wpnPosition, Quaternion.Euler(curPivot));
                    // hide the projectile
                    curBall.GetComponent<Renderer>().enabled = false;
                    firstFrame = true;
                    // freeze the projectiles rigidbody
                    projectileRb = curBall.GetComponent<Rigidbody>();
                    projectileRb.isKinematic = true;

                    projectiles.Add(curBall);

                }


                if (Input.GetMouseButtonUp(0)) {     //      LOOSE!


                    // animate cogs
                    flingBackCogs = true;
                    bigCogRollin = true;


                    // save the aiming data so we can place our arrow again
                    lastWpnPos = new Vector3(wpnPosition.x, wpnPosition.y, wpnPosition.z);

                    if (wpnStatus == 2 || wpnStatus == 3) {

                        wpnStatus = 0;

                        // launch the projectile
                        projectileRb.isKinematic = false;
                        projectileRb.AddForce(Vector3.Scale(springVec, new Vector3(forceMultiplier * chargeMeter, forceMultiplier * chargeMeter, forceMultiplier * chargeMeter)));
                        projectileRb.gameObject.tag = "killsEnemies";
                        chargeMeter = 0.0f;

                        // animnate wpn lol
                        wpnSkinMeshRenderer.SetBlendShapeWeight(0, 0);

                    }


                }


                if (Input.GetMouseButton(0)) { // 		AIM!

                    if (firstFrame == true)
                    {
                        if (curBall != null)
                            curBall.GetComponent<Renderer>().enabled = false;
                    } else
                    {
                        if (curBall != null)
                            curBall.GetComponent<Renderer>().enabled = true;
                    }

                    // track the weapon in 3d
                    // find screen co-ords of touch
                    curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zlingDepth);
                    // convert screen co-ords to a position in the world (the z of which is zlingDepth, relative to the camera)
                    curPosition = cam.ScreenToWorldPoint(curScreenPoint);
                    // get the vector between in-world touch location and pivot point which we place in editor
                    curPivot = ctrlsPivotRb.position - curPosition;
                    // record the vector between our clamped ball and the pivot point
                    // we'll use this on release to determine the shot
                    springVec = Vector3.ClampMagnitude(curPivot, 10.0f);
                    // clamp the magnitude so our projectile can only be drawn so far back
                    wpnPosition = wpnPivotRb.position;// - springVec;






















                    if (Input.mousePosition.y > aimHeight && wpnStatus != 3) { // DRAW!!

                        // once we enter aiming mode we want to be able to swipe back into the charge area without returning to charge mode, hence wpnStatus

                        wpnStatus = 2;
                        distanceDragged = touchStart - Input.mousePosition.y;
                        chargeMeter = distanceDragged / chargeHeight; // the amount we have pulled the bolt back as a percentage of the full charge allowable
                                                                      //wpnPosition = new Vector3 (wpnPosition.x - (springVec.normalized.x * 0.001f), wpnPosition.y - (springVec.normalized.y * 0.001f), wpnPosition.z - (springVec.normalized.z * 0.001f));
                                                                      // animate the weapon mesh
                        wpnSkinMeshRenderer.SetBlendShapeWeight(0, chargeMeter * 100);
                        // place the projectile
                        projectileRb.position = lastWpnPos;
                        projectileRb.transform.LookAt(wpnPivotRb.position);
                        // move the projectile back in accordance with the chargeMeter
                        prjPosition = wpnPosition + ((projectileRb.transform.forward.normalized * -1) * (chargeMeter * chargeMeterMultiplier));
                        projectileRb.position = prjPosition;

                        // animate cogs

                        cog_01.transform.localEulerAngles = new Vector3(chargeMeter * 200.0f, 90.0f, 90.0f);
                        cog_02.transform.localEulerAngles = new Vector3(chargeMeter * -170.0f, 90.0f, 90.0f);
                    }







                    if (Input.mousePosition.y <= aimHeight) { // in aim mode area
                                                              // this will only run on the first frame in aim area, as we won't have changed the wpnStatus yet


                        wpnStatus = 3;
                    }

                    if (wpnStatus == 3) {
                        wpnPosition = new Vector3(wpnPosition.x - (springVec.normalized.x * 0.0001f), wpnPosition.y - (springVec.normalized.y * 0.0001f), wpnPosition.z - (springVec.normalized.z * 0.0001f));


                        // point the weapon model at the pivot point
                        weaponModel.transform.position = wpnPosition;
                        weaponModel.transform.LookAt(wpnPivotRb.position);
                        // move the base as well
                        weaponModelBase.transform.rotation = Quaternion.Euler(-90.0f, weaponModel.transform.rotation.eulerAngles.y, 0.0f);

                        // place the projectile
                        // if we have cleaned up projectiles while the player is aiming, there will be no projectile to aim here
                        // so let's avoid an exception by checking for null
                        if(projectileRb != null)
                        {
                            projectileRb.transform.position = wpnPosition;
                            projectileRb.transform.LookAt(wpnPivotRb.position);

                            prjPosition = wpnPosition + ((projectileRb.transform.forward.normalized * -1) * (chargeMeter * chargeMeterMultiplier));
                            projectileRb.transform.position = prjPosition;

                        }

                        // animate the cogs according to movement of ballista
                        cog_03.transform.localEulerAngles = new Vector3(180.0f, springVec.x * 25, 0.0f);
                        cog_04.transform.localEulerAngles = new Vector3(0.0f, springVec.y * 25, 0.0f);
                    }





                    firstFrame = false;


                }

                // end handle user input
















                // spawn enemies
                if (spawnTimer < Time.time) {
                    // limit the amount of enemies on screen
                    if (numEnemies < maxEnemiesThisWave) {
                        // instantiate the enemy
                        GameObject newShip = Instantiate(theEnemy, new Vector3(Random.Range(-60.0f, 60.0f), Random.Range(30.0f, 100.0f), Random.Range(140.0f, 180.0f)), transform.rotation);
                        // reset the spawn timer
                        spawnTimer = Time.time + spawnInterval;
                        // point the enemy at the player
                        newShip.transform.LookAt(ctrlsPivotRb.position);
                        // increase the number of enemeies tracker
                        numEnemies++;
                        // add a reference to this enemy in our enemies list
                        enemyList.Add(newShip);
                    }
                    if (enemiesKilledThisWave == maxEnemiesThisWave) {
                        winRound();

                    }
                }
















                // handle animating the cogs decoupled from user input
                // animate the cogs flinging back
                if (flingBackCogs == true) {
                    // we'll make these cogs sprang back around the end of their rotation point, like there's a bit of elasticity in the mechanism
                    // we'll do this by lerping a decaying sine wave
                    cogBounceThetaInternal += cogBounceThetaDegredationInternal;
                    cogBounceAmplitudeInternal += cogBounceAmplitudeDegredationInternal;

                    if (cogBounceAmplitudeInternal > 0)
                        rotateAmount = cogBounceAmplitudeInternal * Mathf.Sin(cogBounceThetaInternal);

                    if (cogBounceAmplitudeInternal <= 0) {
                        // cancel cog animations
                        flingBackCogs = false;
                        cogBounceThetaInternal = cogBounceTheta;
                        cogBounceThetaDegredationInternal = cogBounceThetaDegredation;
                        cogBounceAmplitudeDegredationInternal = cogBounceAmplitudeDegredation;
                        cogBounceAmplitudeInternal = cogBounceAmplitude;
                    }

                    cog_01.transform.eulerAngles = new Vector3(rotateAmount, 90f, 90f);
                    cog_02.transform.eulerAngles = new Vector3(rotateAmount * -0.7f, 90f, 90f);

                }

                if (bigCogRollin == true) {
                    bigCogThetaInternal += bigCogThetaDegradationInternal;
                    float t = (Mathf.Cos(bigCogThetaInternal * Mathf.PI * 0.75f) + bigCogOffset) * -1;
                    bigCog.transform.eulerAngles = new Vector3(180.0f, -20.0f, bigCog.transform.eulerAngles.z + t);
                    if (bigCogThetaInternal < 0) {
                        bigCogRollin = false;
                        bigCogThetaInternal = bigCogTheta;
                        bigCogThetaDegradationInternal = bigCogThetaDegradation;
                    }

                }





            }













            //CodeProfiler.End ("GameLoopUpdate");


        } // end Update()





        public void loseLife() {
            if (curGameMode == gameModes.PlayingGame) {
                curHealth--;
                if (curHealth > 0)
                {
                    hearts[curHealth].SetActive(false);
                }
                else
                {
                    loseGame();
                }
            }
        }



        public void gainLife() {
            if (curHealth < fullHealth)
            {
                hearts[curHealth].SetActive(false);
                curHealth++;
            }
        }





        public void loseGame() {
            Debug.Log("Lost Game!");
            cleanupField();

            coinz = 0;
            numEnemies = 0; // how many enemies are on screen, tracked manually so to avoid polling the scene to count
            enemiesKilledThisWave = 0;


            //inGameUIGroup.SetActive (false);
            curGameMode = gameModes.PlayerDiedScreen;
            //SetWaveNumber (0);
            playerIsDeadScreen.SetActive(true);
            playerIsDeadScreen.GetComponent<Animation>().Play("playerIsDeadScreen-slideOut");


        }




        public void restartRound() {
            playerIsDeadScreen.SetActive(true);
            playerIsDeadScreen.GetComponent<Animation>().Play("playerIsDeadScreen-restartRound");
            totalEnemiesKilled = 0;
            SetWaveNumber(1);
        }










        protected virtual void OnGUI() {
            pointsUI.text = totalEnemiesKilled.ToString();
            // waveNumberUI.text = waveNumber.ToString();
            baloonsShotUI.text = totalEnemiesKilled.ToString();

            for (int i = 0; i < fullHealth; i++) {
                if (i < curHealth) {
                    hearts[i].SetActive(true);
                } else {
                    hearts[i].SetActive(false);
                }
            }

        }


        public virtual void enemyKilled()
        {
            enemiesKilledThisWave++;
            totalEnemiesKilled++;
        }

    }
}