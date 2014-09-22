/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * 
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to manage scrolls and contents
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

public class ScrollContent : UIContent {

    UICenterOnChild myCenterView;
    int myScrollPos;

    public UI2DSprite mySpr; 
    
    public TweenColor myColorTween;
    public TweenAlpha myAlphaTween;
    public TweenHeight myHeightTween;
    public TweenWidth myWidthTween;

    void Awake () {
        initContent();
    }

    public override void initContent () {
        myName = this.gameObject.name;
        myScrollPos = int.Parse(myName.Substring(myName.Length-2,2));
        myObj = this.gameObject;
        mySpr = GetComponent<UI2DSprite>();
        myColorTween = GetComponent<TweenColor>();
        myAlphaTween = GetComponent<TweenAlpha>();
        myHeightTween = GetComponent<TweenHeight>();
        myWidthTween = GetComponent<TweenWidth>();
        myCenterView = FindObjectOfType<UICenterOnChild>();
    }

    public override TweenColor GetColorTweener () {
        return myColorTween;
    }

    public override TweenAlpha GetAlphaTweener () {
        return myAlphaTween;
    }

    public override TweenHeight GetHeightTweener () {
        return myHeightTween;
    }

    public override TweenWidth GetWidthTweener () {
        return myWidthTween;
    }

    public override UI2DSprite Get2DSprite () {
        return mySpr;
    }

    public override void OnClick()
    {
        ComicManager.instance.PageActive(myScrollPos);
    }

    public void PlayAlphaForward () {
        if (mySpr.color.a < 0.5f) {
            myAlphaTween.PlayForward();
        }
    }

    public void PlayResizeForward () {
        myColorTween.PlayForward();
        myHeightTween.PlayForward();
        myWidthTween.PlayForward();
    }

    public void PlayResizeReverse () {
        myColorTween.PlayReverse();
        myHeightTween.PlayReverse();
        myWidthTween.PlayReverse();
    }

    public void CenterView () {
        myCenterView.CenterOn(gameObject.transform);
    }
}
