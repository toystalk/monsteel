using UnityEngine;
using System.Collections;

/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, April 2014
 * 
 * Mainly responsible for receiving NGUI's callback, 
 * sending the receiver's name to GUIManager.
 * Should be attached to a gameobject inside of any other gameobject
 * and associated to NGUI's callback.
*/
public class GUICallBackReceiver : MonoBehaviour {
    public void OnClick() {
        Debug.Log("OnClick callback");
        GUIManager.instance.onClick(transform.name);
    }

    public void OnFinished() {
        GUIManager.instance.onTweenEnd(gameObject);
    }

    public void TriggerAudio () {
        switch (transform.name) {
            case "Fireplace":
                AudioManager.instance.playEffectVol(transform.name,1.0f);
                break;
            case "Discoscratch":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Batsqueak":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Batfly":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Pageturn":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Puff":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Quickmusic":
                AudioManager.instance.playBGM(transform.name);
                break;
            case "Barulho":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "Teeth":
                AudioManager.instance.playEffect(transform.name);
                break;
            case "BlueBacklight":
                AudioManager.instance.playEffect("Puff2");
                break;
            case "Zombie":
                AudioManager.instance.playEffect("Discoscratch");
                break;
            case "Dracula":
                AudioManager.instance.playEffect("Teeth");
                break;
            case "MissPanta":
                AudioManager.instance.playEffect("Barulho");
                break;
        }
    }
}
