/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, April 2014
 */

using UnityEngine;
using System.Collections;

// Class responsible for managing the entire game and it's required components for every moment
public class GameManager : Singleton<GameManager> {

    // Game states, divided by every different moment of the game
    public enum GameState {
        Splash,
        Start,
		PreGame,
		Game,
		PosGame
    }

    public GameState currentState;// Keeps tracking of the current State of the game
    public GameState prevState;  // Keeps tracking of the previous State of the game

    [SerializeField]
    private string currentScene; // Keeps tracking of the current scene

    delegate void StateUpdate(); // Delegate to update according to the state of update needed, 
                                 // being able to run multiple updates

    StateUpdate currentUpdate; // Manages wanted updates to be ran    

// Keep track of state change in the editor, being used to trigger arbitrary changes
#if UNITY_EDITOR
    public bool stateChanged = false;
#endif

    // Use this for initialization
    void Awake() {
        if (FindObjectOfType<GUIManager>() == null) { 
            initManager();
        }
        else {
            Destroy(gameObject);
        }
    }

    void Update() {
// Keep track of state change in the editor
#if UNITY_EDITOR
        if (stateChanged) {
            stateChanged = false;
            GameState tempState = currentState;
            currentState = prevState;
            updateState(tempState);
        }
#endif
        // If there is any update set, run them
        if (currentUpdate != null) {
            currentUpdate();
        }
    }

    /* Initialize all Managers   
        * Check GameManager existence and make sure it's container is created 
        * with the correct name, if it's not, Singleton will handle it.
        * Once GameManager is ok, check all other managers while making sure they are childs of GameManager;
    */
    void initManager() {
        if (GameManager.instance) {
            //Initialize GUIManager
            GUIManager.makeChildOf(gameObject);
            GUIManager.instance.initContents();
            //Initialize AudioManager
            AudioManager.makeChildOf(gameObject);

            currentScene = Application.loadedLevelName;
            updateState(currentState);
        }
        else {
            Debug.Log("Failed to initialize GameManager");
        }
    }

    // Starts the game updating a current state to begin
    public void startGame() {
        //starts the game
        updateState(GameState.Start);
    }

    // Updates the current State of the game to the received one, saving the current as a previous
    // and updating main or general content on the screen. Also may deactivate some content according
	// to the previous state, or load a new scene.
    public void updateState(GameState nextGs) {
        prevState = currentState;
        currentState = nextGs;

        if (currentScene != currentState.ToString()) {
            loadScene(currentState.ToString());
        }
        else {
            updateGUI();
        }
    }

    public void updateStateWait(GameState nextGs, float afterSecs){
        StartCoroutine(updateStateAfter(nextGs, afterSecs));
    }

    IEnumerator updateStateAfter (GameState nextGs, float afterSecs) {
        yield return new WaitForSeconds(afterSecs);
        updateState(nextGs);
    }

	// Start a routine to load the scene received through the string
    void loadScene(string levelToLoad) {
        StartCoroutine("load", levelToLoad);
    }

    // Updates GUI according to the state of the game
    void updateGUI () {
        switch (currentState) {
            case GameState.Start:
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            case GameState.Splash:                
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            case GameState.Game:
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            default:
                break;
        }
    }

	// Routine responsible for loading a level async, updating UI content after.
    IEnumerator load(string levelToLoad) {
        Debug.Log("LOADING : " + levelToLoad);
        AsyncOperation async = Application.LoadLevelAsync(levelToLoad);
        async.allowSceneActivation = false;
        do {
            yield return new WaitForSeconds(1.0f);
        } while (async.isDone);
        
        async.allowSceneActivation = true;
        do {
            yield return new WaitForSeconds(0.01f);
        } while (Application.loadedLevelName != levelToLoad);

        currentScene = Application.loadedLevelName;
        GUIManager.instance.initContents();
        AudioManager.instance.initSources();
        updateGUI();
    }
}
