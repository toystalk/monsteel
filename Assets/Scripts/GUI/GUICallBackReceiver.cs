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
}
