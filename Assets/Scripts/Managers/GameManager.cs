/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, April 2014
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class responsible for managing the entire game and it's required components for every moment
public class GameManager : Singleton<GameManager> {

    // Game states, divided by every different moment of the game
    public enum GameState {
        ToySelection,
        Splash,
        Start,
    }

    public enum ToyState {
        Frank,
        Dracula
    }

    public GameState currentState;// Keeps tracking of the current State of the game
    public GameState prevState;  // Keeps tracking of the previous State of the game

    private ToyState _currentToy;
    public ToyState currentToy {
        get {
            return _currentToy;
        }
        set {
            switch (value) {
                case ToyState.Dracula:
                    //ActivateDataSet("Toystalk_MissMonster");
                    ActivateDataSet("Toystalk_Dracula");
                    break;
                case ToyState.Frank:
                    ActivateDataSet("Toystalk_Frank");
                    break;
                default:
                    break;
            }
            _currentToy = value;
        }
    }

    [SerializeField]
    private string currentScene; // Keeps tracking of the current scene

    delegate void StateUpdate(); // Delegate to update according to the state of update needed, 
                                 // being able to run multiple updates

    StateUpdate currentUpdate; // Manages wanted updates to be ran    

    public bool waitToLoad;

// Keep track of state change in the editor, being used to trigger arbitrary changes
#if UNITY_EDITOR
    public bool stateChanged = false;
#endif

    void Start () {
        startGame();
    }

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
        if (Input.GetKeyDown(KeyCode.Return)) {
            ActivateDataSet("something");
        }
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
    public void startGame () {
        //waitToLoad = true;
        //starts the game
        updateState(GameState.ToySelection);
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
            //updateGUI();
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
            case GameState.ToySelection:
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            case GameState.Start:
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            case GameState.Splash:                
                GUIManager.instance.updateMainContent(currentState.ToString());
                break;
            default:
                break;
        }
    }

	// Routine responsible for loading a level async, updating UI content after.
    IEnumerator load(string levelToLoad) {
        Debug.Log("LOADING : " + levelToLoad);
        if (currentState == GameState.Start) {
            LoadScreen.loadIn();
            yield return new WaitForSeconds(3.0f);           
        }

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
        Debug.Log("LOADING : " + currentScene + " COMPLETE.");
        if (currentState == GameState.Start) {
            yield return new WaitForSeconds(2.0f);
            LoadScreen.loadOut();
        }

        AudioManager.instance.initSources();

        if (currentState == GameState.Splash) {
            yield return new WaitForSeconds(2.0f);
            updateState(GameState.Start);
        }
    }

    private void ActivateDataSet (string datasetPath) {
        /*ImageTracker tracks ImageTargets contained in a DataSet and provides methods for creating, activating and deactivating datasets.
        ImageTracker imageTracker = TrackerManager.Instance.GetTracker<ImageTracker>();
        IEnumerable<DataSet> datasets = imageTracker.GetDataSets();

        IEnumerable<DataSet> activeDataSets = imageTracker.GetActiveDataSets();
        List<DataSet> activeDataSetsToBeRemoved = activeDataSets.ToList();

        //1. Loop through all the active datasets and deactivate them.
        foreach (DataSet ads in activeDataSetsToBeRemoved) {
            imageTracker.DeactivateDataSet(ads);
        }

        //Swapping of the datasets should not be done while the ImageTracker is working at the same time.
        //2. So, Stop the tracker first.
        imageTracker.Stop();

        //3. Then, look up the new dataset and if one exists, activate it.
        foreach (DataSet ds in datasets) {
            if (ds.Path.Contains(datasetPath)) {
                imageTracker.ActivateDataSet(ds);
            }
        }

        //4. Finally, start the image tracker.
        imageTracker.Start();*/
        //Debug.Log(TrackerManager.Instance.GetTracker<ImageTracker>());
        if (currentState == GameState.Start) {
            ImageTracker imageTracker = TrackerManager.Instance.GetTracker<ImageTracker>();
            IEnumerable<DataSet> datasets = imageTracker.GetDataSets();

            IEnumerable<DataSet> activeDataSets = imageTracker.GetActiveDataSets();
            List<DataSet> activeDataSetsToBeRemoved = activeDataSets.ToList();

            //1. Loop through all the active datasets and deactivate them.
            foreach (DataSet ads in activeDataSetsToBeRemoved) {
                //Debug.Log(ads.Path + "is active.");
                imageTracker.DeactivateDataSet(ads);
            }

            imageTracker.Stop();        

            foreach (DataSet ds in datasets) {
                Debug.Log(ds.Path);
                if (ds.Path.Contains(datasetPath)) {
                    imageTracker.ActivateDataSet(ds);
                    Debug.Log("Found "+datasetPath+" dataset.");
                }
            }

            imageTracker.Start();

            /* DEBUG current active after swapping
            activeDataSets = imageTracker.GetActiveDataSets();
                
            foreach (DataSet ads in activeDataSets) {
                Debug.Log(ads.Path + "is active.");
            }*/
        }
    }
}
