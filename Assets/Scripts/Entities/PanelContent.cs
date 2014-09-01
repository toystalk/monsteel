/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * 
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to manage NGUI labels attributes and methods
 */

using UnityEngine;
using System.Collections;

public class PanelContent : UIContent {

    UIPanel myPanel;

    void Awake () {
        initContent();
    }

    public override void initContent () {
        myName = this.gameObject.name;
        myObj = this.gameObject;
        myPanel = GetComponent<UIPanel>();
    }

    public override UIPanel GetPanel () {
        return myPanel;
    }
}
