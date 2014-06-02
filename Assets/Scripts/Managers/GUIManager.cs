/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, April 2014
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Content class to manipulate specific UI elements, serialized to be shown in the inspector
[System.Serializable]
public class InterfaceContent {

    public string name; // Name of the content
    public GameObject content; // Gameobject of the interface content
	public UILabel label; // Label component of a gameObject
    [SerializeField]
    bool _display; // Set the content to be shown or hidden

    public bool display {
        get {
            return _display;
        }
        set {
            //Debug.Log("Setting " + content.name + " as " + value);
			if(content){
            	content.SetActive(value);
			}
            _display = value;
        }
    }

    public InterfaceContent() {
        name = "default";
        display = false;
    }

    //Initializes an interface content with given name and it's gameobject
    public InterfaceContent(string name, GameObject content) {
        this.name = name;
        this.content = content;
    }

	//Initializes an interface content with given name and it's labelText
	public InterfaceContent(string name, UILabel label) {
		this.name = name;
		this.label = label;
	}
}

/** 
 * GUI class to manipulate all the interface, consisted basically of :
 * - MainContents : main windows of each moment in the game. They must be named
 * "Panel+GameState", like "PanelBattleMenu" to be found when updating contents,
 * since they're directly attached to a game state.
 * - GeneralContents : pop-up windows and any general content.
 * - LabelContents : labels inside any general or main content.
 * Contents inside unity also must be tagged if they're going to be used, as :
 * GeneralLabel, MainContent, or GeneralContent.
 * 
 * Updates handle any given content, and onClick is a callback receiver
 * for clicks, which requires OnClickReceiver.cs associated to the clicked object
 * and to NGUI callback for click events.
 * 
 * This manager will always be initialized by the GameManager, which will be
 * responsible for calling any MainContent transitions.
*/
public class GUIManager : Singleton<GUIManager> {

    public List<InterfaceContent> mainContents; // List of interface main contents
    public List<InterfaceContent> generalContents; // List of interface general contents
	public List<InterfaceContent> labelContents; // List of interface label contents
    InterfaceContent displayedMain; // Keep track of current displayed main content

    // Initializes content lists searching for any objects assigned with given tags
    // and deactivating them right after. (If they are already disabled, they can't be found)
	// Doesn't deactivate labels since they are already inside some content
    public void initContents() {
		labelContents = new List<InterfaceContent>();
		GameObject [] foundContent = GameObject.FindGameObjectsWithTag ("GeneralLabel");
		foreach(GameObject go in foundContent){
			labelContents.Add (new InterfaceContent(go.name, go.GetComponent<UILabel>()));
		}

        generalContents = new List<InterfaceContent>();
        foundContent = GameObject.FindGameObjectsWithTag("GeneralContent");
        foreach (GameObject go in foundContent) {
            generalContents.Add(new InterfaceContent(go.name, go));
            go.SetActive(false);
        }

		mainContents = new List<InterfaceContent>();
        foundContent = GameObject.FindGameObjectsWithTag("MainContent");
        foreach (GameObject go in foundContent) {
            mainContents.Add(new InterfaceContent(go.name, go));
            go.SetActive(false);
        }
    }

    // Callback receiver for click events.
    // Tests clicked button's name to handle what should be done.
    public void onClick(string button) {
        switch (button) {
			case "buttonName":
				break;
            case "StartButton":
                ToyManager.instance.startComic();
                break;
            case "MenuButton":
                ToyManager.instance.updateState(ToyManager.draculaState.PreRA);
                break;
            case "NextButton":
                ToyManager.instance.updateState(ToyManager.draculaState.Comic2);
                AudioManager.instance.stopBGM();
                AudioManager.instance.playEffect("Pageturn");
                break;
            case "BackButton":
                if (ToyManager.instance.currentState == ToyManager.draculaState.Smoke) {
                    ToyManager.instance.updateState(ToyManager.draculaState.Comic2);
                }
                else {
                    ToyManager.instance.updateState(ToyManager.draculaState.Comic1);
                }
                AudioManager.instance.playEffect("Pageturn");
                break;
            case "Dracula":
                ToyManager.instance.updateState(ToyManager.draculaState.Smoke);
                break;
            default:
                break;
        }
    }

	// Callback receiver for tween events.
	// Tests the tweened object's name to handle what should be done.
    public void onTweenEnd(GameObject content) {
        switch (content.name) {
            case "contentName":
                break;
            case "FadeSplash":
                GameManager.instance.waitToLoad=false;
                //GameManager.instance.startGame();
                break;
            case "FadeStart":
                Destroy(content);
                break;
        }
    }

    // Updates the given main content, setting to false the current one and true the next one,
    // after searching the list for the Panel + given State name, then sets the updated one to 
    // be the current displayedContent. If mainContent is not found, searches for general content.
    public void updateMainContent(string nextDisplay) {

        string displayName = "Panel" + nextDisplay;
        InterfaceContent displayContent = mainContents.Find(delegate(InterfaceContent ic) {
            return ic.name == displayName;
        });

        if (displayContent != null) {
             if (displayedMain != null) {
                displayedMain.display = false;
            }
            displayContent.display = true;
            displayedMain = displayContent;

            //Debug.Log("Displaying " + nextDisplay + " main content");
        }
    }

    // Updates the given general content, setting it to display if found, and attaching to the
    // current mainDisplay so it deactivates on mainContent change.
    public void updateGeneralContent(string nextContent, bool display) {
        InterfaceContent displayContent = generalContents.Find(delegate(InterfaceContent ic) {
            return ic.name == nextContent;
        });

        if (displayContent != null) {
            displayContent.display = display;
            displayContent.content.transform.parent = displayedMain.content.transform;
        }

       //Debug.Log("Displaying " + nextContent + " general content");
    }
	
	// Updates the given label if found, changing it's text to the given text
	public void updateLabelContent(string content, string text){

		InterfaceContent displayContent = labelContents.Find(delegate(InterfaceContent ic) {
			return ic.name == content;
		});

		if (displayContent != null) {
			displayContent.label.text = text;
		}
	}

}
