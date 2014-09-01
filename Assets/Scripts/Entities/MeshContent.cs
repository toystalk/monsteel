/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * 
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to manage unity meshes attributes and methods
 */

using UnityEngine;
using System.Collections;

public class MeshContent : UIContent {
        
    Animator myAnimator;                
    float myAnimatorTime = 0;
    const float offSetAnimatorTime = 500;

    void Awake () {
        initContent();
    }

    public override void initContent () {
        myName = this.gameObject.name;
        myObj = this.gameObject;
        myAnimator = this.GetComponent<Animator>();
        StartCoroutine(AnimatorUpdate());
    }

    // Update the animator every frame to keep track of the given time
    IEnumerator AnimatorUpdate () {
        myAnimator.enabled = true;

        while (myAnimator) {
            if (myAnimatorTime < 0) {
                myAnimatorTime += 10;
            }
            
            myAnimator.Play("StageSelectLab", -1, myAnimatorTime);
            
            yield return null;
        }
    }

    public override void OnDrag (Vector2 dragPos) {
        myAnimatorTime -= (dragPos.x) / offSetAnimatorTime;
    }
}
