using UnityEngine;
using System.Collections;

public class ScrollContent : UIContent {

    UICenterOnChild myCenterView;

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
