/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to manage NGUI buttons attributes and methods
 */

using UnityEngine;
using System.Collections;

public class ButtonContent : UIContent {

    UIButton myButton;
    UISprite mySprite;
    
    void Awake () {
        initContent();
    }

    public override void initContent () {
        myName = this.gameObject.name;
        myObj = this.gameObject;
        myButton = GetComponent<UIButton>();
        mySprite = GetComponent<UISprite>();
    }

    public override UISprite GetSprite () {
        return mySprite;
    }

    public override UIButton GetButton () {
        return myButton;
    }
}
