/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to handle fade in/out transitions between screens
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

public class Transition : MonoBehaviour {

    //Static method to start fading
    public static void StartFade (string fadeOption) {
        try {
            FindObjectOfType<Transition>().startFade(fadeOption);
        }
        catch (System.Exception error) {
            GameManager.Debugger("Couldn't find fade object" + error.ToString());
        }
    }

    UI2DSprite myUI;
    Color fadeColor;
    [SerializeField]
    float fadeSpeed = 1;

    //TODO: maybe set fadeIn with get/set to automatically change PlayFade
    public bool fadeIn = true;
    public bool playOnAwake = true;
    public bool _playing = false;
    public static bool playing {
        get {
            return FindObjectOfType<Transition>()._playing;
        }
    }

    delegate IEnumerator FadeTransition ();

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

    void OnTransitionStart () {
        _playing = true;
        GameManager.Debugger("Transition started");
    }

    void OnTransitionEnd () {
        _playing = false;
        GameManager.Debugger("Transition ended");
    }
}
