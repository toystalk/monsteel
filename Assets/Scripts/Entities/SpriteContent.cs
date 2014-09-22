/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, September 2014
 * 
 * Class to manage NGUI's unity sprites attributes and methods
 */

using UnityEngine;
using System.Collections;

public class SpriteContent : UIContent {

    public UI2DSprite mySpr;

    void Awake () {
        initContent();
    }

    public override void initContent () {
        myName = this.gameObject.name;
        myObj = this.gameObject;
        mySpr = GetComponent<UI2DSprite>();
    }

    public override UI2DSprite Get2DSprite () {
        return mySpr;
    }
}
