/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Manages game states and every scene/moment transition in the game
 * Everything that should alter the game flow must go through the game
 * manager first. It's also responsible for initializing other managers
 * at given moments of the game. 
 * 
 * On the unity editor it will also deal with xml generation.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Core {
    // Game states, divided by every different moment of the game
    public enum GameStateHandler {
        Loader,
        Intro,
        Comic,
        Testimonial,
        MiniGame
    }
    // Class responsible for managing the entire game and it's required components for every moment
    public class GameManager : Singleton<GameManager> {           

        public static void SetState (GameStateHandler myState) {
            GameManager.instance.updateState(myState);
        }

        #region STATIC_DEBUG

        void initDebug () {
            debugInput = (ChatInput)FindObjectOfType(typeof(ChatInput));
            if (debugInput) {
                DontDestroyOnLoad(GUIManager.instance.GetUI("DebugUI").gameObject);
                SetDebug = true;
                DebuggerEnabled();
            }
        }

        public static void Debugger (UnityEngine.Object log) {
            if (debugInput) { 
                debugInput.DebugLog(log.ToString());
            }

            Debug.Log(log);
        }

        public static void Debugger (string log) {
            if (debugInput) {
                debugInput.DebugLog(log);
            }

            Debug.Log(log);
        }

        public static void DebuggerEnabled () {
            debugInput.transform.parent.gameObject.SetActive(SetDebug = !SetDebug);
        }

        public static ChatInput debugInput;
        static bool SetDebug { get; set; }

        #endregion

        public GameState currentState;// Keeps tracking of the current State of the game
        public GameStateHandler savedState;

        public Stack<GameStateHandler> prevState;  // Keeps tracking of the previous State of the game

        public List<GameState> myStates;

        [SerializeField]
        private string currentScene; // Keeps tracking of the current scene

    // Keep track of state change in the editor, being used to trigger arbitrary changes
    #if UNITY_EDITOR
        public bool stateChanged = false;
    #endif

        // Use this for initialization
        void Awake() {
            initManager();
            StartGame();
        }

#if UNITY_EDITOR
        void Update() {
    // Keep track of state change in the editor
            if (stateChanged) {
                ForceState(currentState.myState);
            }         
        }
        // Force state through the inspector or direct calls
        void ForceState (GameStateHandler forcedState) {
            stateChanged = false;
            GameManager.Debugger("Forcing state " + forcedState.ToString());
            updateState(forcedState);
            showStateStack();
        }

        // Force state with string for debugging
        void ForceState (string forcedState) {
            GameStateHandler myState = (GameStateHandler)Enum.Parse(typeof(GameStateHandler), forcedState);
            GameManager.Debugger("Forcing state " + forcedState.ToString());
            updateState(myState);
        }
#endif

        // Show previous states stack 
        void showStateStack () {
            string stateStack = "";
            Stack<GameStateHandler> temp = prevState;
            foreach(GameStateHandler gs in temp){
                stateStack += (gs.ToString()+ ", ");
            }
            GameManager.Debugger("Showing previous states stack : \n" + stateStack);
        }

        /* Initialize all Managers   
            * Check GameManager existence and make sure it's container is created 
            * with the correct name, if it's not, Singleton will handle it.
            * Once GameManager is ok, check all other managers while making sure they are childs of GameManager;
        */
        void initManager() {
            if (GameManager.instance) {
                //Initialize GUIManager
                GUIManager.instance.initContents();
                GUIManager.makeChildOf(gameObject);
                //Initialize AudioManager
                //AudioManager.makeChildOf(gameObject);
                OnManagersInitialized("GUI");
                initDebug();
            }
            else {
                GameManager.Debugger("Failed to initialize GameManager");
            }
        }

        void initRAManager () {
            RAManager.instance.initRA();
            RAManager.makeChildOf(gameObject);

            OnManagersInitialized("Vuforia");
        }

        void initComicManager () {
            ComicManager.instance.OnComicStart();
            ComicManager.makeChildOf(gameObject);

            OnManagersInitialized("Comic");
        }

        public void SetAutoFocus () {
            StartCoroutine("StartAutoFocus");
        }

        IEnumerator StartAutoFocus () {
            yield return new WaitForSeconds(1.0f);
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }

        // Starts the game updating a current state to begin
        void StartGame() {
            currentScene = Application.loadedLevelName;
            prevState = new Stack<GameStateHandler>();
            currentState = FindState(currentState.myState);

            OnGameStart();
            loadScene();      
        }


        void StartRA () {
            //updateState(GameStateHandler.PotionRA);
        }

        // Updates the current State of the game to the received one, saving the current as a previous
        // and updating main or general content on the screen. Also may deactivate some content according
	    // to the previous state, or load a new scene.
        public void updateState(GameStateHandler nextGs) {
            if (currentState.FindNext(nextGs)) {
                GameManager.Debugger("Updating to state "+nextGs);
                currentState = FindState(nextGs);
                loadScene();
            }
        }             

        GameState FindState (GameStateHandler myState) {
            GameState found = myStates.Find(delegate (GameState gs){
                return gs.myState == myState;
            });

            return found; 
        }

	    // Start a routine to load the scene if needed, or just call StateUpdate if not.
        public void loadScene() {
            string nextScene = currentState.GetScene();
            GameManager.Debugger("=============="+nextScene);
            if ((currentScene!=nextScene) && (nextScene!="")) {
                StartCoroutine("load", currentState.GetScene());
            }
            else {
                OnStateUpdate();
            }
        }

        #region CALLBACKS

        void OnManagersInitialized(string managers){
            GameManager.Debugger("Managers initialized : " + managers);
        }

        void OnGameStart () {
            GameManager.Debugger("GameStarted Callback");
        }

        void OnStateUpdate () {
            GameManager.Debugger("State updated Callback : " + currentState);

            /*
             * Use to instantiate non-GUI objects or call methods
             * ==================================================
             * Current States :
             * 
             * Intro
             * Comic
             * Testimonial
             * MiniGame
             */
            switch (currentState.GetName()) {
                case "Intro":
                    initComicManager();
                    StartCoroutine(LoadAfterSecs(GameStateHandler.Comic, 5.0f));
                    break;
                case "Comic":
                    ComicManager.instance.StartCheck();
                    break;
                case "Testimonial":
                    break;
                default:
                    break;
            }

            prevState.Push(currentState.myState);
            GUIManager.instance.OnUpdateStateUI(currentState.GetName());
        }

        void OnSceneLoad () {
            GameManager.Debugger("New scene loaded Callback : " + currentState.GetScene());
            currentScene = currentState.GetScene();
            GUIManager.instance.initContents();

            /*
             * Use to do stuff exclusively when the state's scene loads
             * ========================================================
             * Current States :
             * 
             * Intro
             * Comic
             * Testimonial
             * MiniGame
             */
            switch (currentState.GetName()) {
                case "Intro":
                    break;
                case "Comic": 
                    GUIManager.instance.GetUI("LoaderTop").GetPositionTweener().PlayForward();
                    GUIManager.instance.GetUI("LoaderBot").GetPositionTweener().PlayForward();
                    break;
                case "Testimonial":                    
                    GUIManager.instance.GetUI("LoaderTop").GetPositionTweener().PlayForward();
                    GUIManager.instance.GetUI("LoaderBot").GetPositionTweener().PlayForward();
                    break;
                case "MiniGame":
                    GUIManager.instance.GetUI("LoaderTop").GetPositionTweener().PlayForward();
                    GUIManager.instance.GetUI("LoaderBot").GetPositionTweener().PlayForward();
                    break;
                default:
                    break;
            }

            OnStateUpdate();
        }
        
        #endregion

        IEnumerator LoadAfterSecs (GameStateHandler nextState, float delaySecs) {
            yield return new WaitForSeconds(delaySecs);
            //Transition.StartFade("In");
            updateState(nextState);
        }

        // Routine responsible for loading a level async, updating UI content after.
        IEnumerator load(string levelToLoad) {
            GameManager.Debugger("LOADING : " + levelToLoad);

            GUIManager.instance.GetUI("LoaderTop").GetPositionTweener().PlayReverse();
            GUIManager.instance.GetUI("LoaderTop").GetPositionTweener().ResetToBeginning();
            GUIManager.instance.GetUI("LoaderBot").GetPositionTweener().PlayReverse();
            GUIManager.instance.GetUI("LoaderBot").GetPositionTweener().ResetToBeginning();

            yield return new WaitForSeconds(3.0f);

            //(changed load level async to additive async while testing new load)
            AsyncOperation async = Application.LoadLevelAsync(levelToLoad);
            async.allowSceneActivation = false;
            do {
                yield return new WaitForSeconds(1.0f);
            } while (async.isDone);

            if (Transition.instance) {
                while (Transition.instance.Playing) {
                    yield return new WaitForSeconds(1.0f);
                }
            }            

            async.allowSceneActivation = true;

            do {
                yield return new WaitForSeconds(0.01f);
            } while (Application.loadedLevelName != levelToLoad);

            OnSceneLoad();            
        }

    }
}