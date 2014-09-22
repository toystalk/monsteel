/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, September 2014
 * 
 * Class to manage any UI attributes or interaction, to be extended from specific content classes.
 * 
 * TODO: Change this into an interface to be implemented by other classes.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;

// Content class to manipulate specific UI elements, serialized to be shown in the inspector
[System.Serializable]
public class UIContent : MonoBehaviour {
    
    public string myName; // Name of the content
    public GameObject myObj; // Gameobject of the interface content

    public bool display; // Set the content to be shown or hidden
    public bool draggable; //Set the content to be draggable or not

    public float myFadingSpeed; // Fade speed for fading transitions
        
    // Activates and deactivates the object through setter    
    public bool Display {
        get {
            return display;
        }
        set {
			gameObject.SetActive(value);
            display = value;
        }
    }
    
    // Required method to initialize any content and it's properties
    public virtual void initContent () {
        myName = this.gameObject.name;
        myObj = this.gameObject;            
    }

    // Fade's current panel with given speed
    public void FadePanel (float fadeSpeed) {
        myFadingSpeed = fadeSpeed;
        StartCoroutine(FadePanel());
    }

    // Coroutine to fade panel
    IEnumerator FadePanel () {
        UIPanel tempPanel = GetComponent<UIPanel>();
        while (tempPanel.alpha < 1) {
            tempPanel.alpha += (0.01f * myFadingSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    // Activate all child content
    public void ChildActiveAll (bool setActive) {
        foreach (Transform t in transform) {
            t.gameObject.SetActive(setActive);
        }
    }

    // Handles when button is clicked, calling GUIManager's reflection
    public virtual void OnClick () {
        // Call guimanager to handle button click
        GUIManager.instance.ButtonCallback (myName);
    }

    // Dragging NGUI's callback to set animator time according to dragging position
    public virtual void OnDrag (Vector2 dragPos) {
    }

    // Specific components getters
    public virtual UI2DSprite Get2DSprite () {
        return GetComponent<UI2DSprite>();
    }

    public virtual UISprite GetSprite () {
        return GetComponent<UISprite>();
    }

    public virtual UIButton GetButton () {
        return GetComponent<UIButton>();
    }

    public virtual UILabel GetLabel () {
        return GetComponent<UILabel>();
    }

    public virtual UIPanel GetPanel () {
        return GetComponent<UIPanel>();
    }

    public virtual TweenColor GetColorTweener () {
        return GetComponent<TweenColor>();
    }

    public virtual TweenAlpha GetAlphaTweener () {
        return GetComponent<TweenAlpha>();
    }

    public virtual TweenHeight GetHeightTweener () {
        return GetComponent<TweenHeight>();
    }

    public virtual TweenWidth GetWidthTweener () {
        return GetComponent<TweenWidth>();
    }

    public virtual TweenPosition GetPositionTweener () {
        return GetComponent<TweenPosition>();
    }

    public virtual TweenScale GetScaleTweener () {
        return GetComponent<TweenScale>();
    }

    #region UI CALLBACKS
    public void OnEnable () {
        //GameManager.Debugger("Object " + myName + " enabled.");
    }

    public void OnDisable () {
        //GameManager.Debugger("Object " + myName + " disabled.");
    }
    #endregion
}
