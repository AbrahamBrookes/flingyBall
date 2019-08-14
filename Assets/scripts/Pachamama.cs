using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using flingyball; // the universe, in a namespace

/**
 *  Pachamama is the controller of the whole game. We need some class to hold GameModes, so we can 
 *  switch the GameMode out as we play. This will make it easier to bust out the tutorial to be 
 *  less error prone, and enables us to make more GameModes later. So far the tuorial will be one
 *  GameMode and the main game will be the other GameMode. GameModes generally control the on-screen UI,
 *  the controls and the scoring systems.
 *  However, the main menu part at the start of the game will need to exist before the GameMode, as
 *  we will use it to set the GameMode. Something must come before the existence of the universe.
 *  Pachamama.
 *  
 */

public class Pachamama : MonoBehaviour
{

    public GameMode GameMode;

    public GameUIItem startScreen;

    // Start is called before the first frame update
    void Start()
    {
        startScreen.gameObject.SetActive(true);
        startScreen.anims.Play("mainMenu-shootOut");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
