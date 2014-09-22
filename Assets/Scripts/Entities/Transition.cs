/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to handle fade in/out transitions between screens, 
 * using a UI2DSprite with black texture.
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

public class Transition : MonoBehaviour {

    //Static method to start fading
    public static void StartFade (string fadeOption) {
        try {
            instance.startFade(fadeOption);
        }
        catch (System.Exception error) {
            GameManager.Debugger("Couldn't find fade object " + error.ToString());
        }
    }

    public static Transition _instance;
    public static Transition instance {
        get {
            if (_instance == null) {
                // Finds the instance of type T
                _instance = FindObjectOfType<Transition>();
            }
            return _instance;
        }
    }

    UI2DSprite myUI; // Black texture
    Color fadeColor; // Cached color to be used through fade process
    [SerializeField] 
    float fadeSpeed = 1; // How fast fade will play

    //TODO: maybe set fadeIn with get/set to automatically change PlayFade
    public bool fadeIn = true; // Sets fade out or fade in
    public bool playOnAwake = true; // If fade should be played when Awake is called

    public bool Playing { get; set; } // If fade is playing or not
    
    delegate IEnumerator FadeTransition (); // Delegate to play both fade in or out
    FadeTransition PlayFade; 
    
    void Awake() {
        try {
            myUI = GetComponent<UI2DSprite>();
            initConfig();
        }
        catch (System.Exception error) {
            GameManager.Debugger("Couldn't find UI for this transition : \n" + error.ToString());
        }
    }

    // Configure fade according to properties values
    void initConfig () {
        if (fadeIn) {
            fadeColor = new Color(1, 1, 1, 0);
            myUI.color = fadeColor;
            PlayFade += FadeIn;
        }
        else {
            fadeColor = new Color(1, 1, 1, 1);
            myUI.color = fadeColor;
            PlayFade += FadeOut;
        }

        if (PlayFade != null && playOnAwake) {
            StartCoroutine(PlayFade());
        }
    }

    // Method to start fade out or in through static method call
    void startFade (string fadeOption) {
        switch (fadeOption) {
            case "In":
                PlayFade += FadeIn;
                break;
            case "Out":
                PlayFade += FadeOut;
                break;
            default :
                GameManager.Debugger("Not a valid type of fade");
                break;
        }

        StartCoroutine(PlayFade());
    }
    
    // FadeIn Coroutine
    IEnumerator FadeIn () {
        GameManager.Debugger("Init Fade in...");
        OnTransitionStart();
        while(fadeColor.a < 1){
            fadeColor.a += (0.01f*fadeSpeed);
            myUI.color = fadeColor;
            yield return new WaitForEndOfFrame();
        }
        OnTransitionEnd();
        PlayFade -= FadeIn;
    }

    // FadeOut Coroutine
    IEnumerator FadeOut () {
        GameManager.Debugger("Init Fade out...");
        OnTransitionStart();
        while (fadeColor.a > 0) {
            fadeColor.a -= (0.01f*fadeSpeed);
            myUI.color = fadeColor;
            yield return new WaitForEndOfFrame();
        }
        OnTransitionEnd();
        PlayFade -= FadeOut;
    }

    // Fade Coroutine start callback
    void OnTransitionStart () {
        Playing = true;
        GameManager.Debugger("Transition started");
    }

    // Fade Coroutine ended callback
    void OnTransitionEnd () {
        Playing = false;
        GameManager.Debugger("Transition ended");
    }
}
