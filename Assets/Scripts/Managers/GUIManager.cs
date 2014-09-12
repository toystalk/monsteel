/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class responsible for managing all UI elements. No UI elements should be changed
 * by other classes other than GUIManager. It also contains fast acccess to UI 
 * elements with the UIContent class as a component; (or a class that derives from it)
 * At last, it will handle all button callbacks through object name received on the
 * callbacks.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Scripts.Core {

    public class GUIManager : Singleton<GUIManager> {

        //public List<UIContent> guiContents; // List of interface contents
        public Dictionary<string,UIContent> UIContents; //Dictionary of all UIContent
        public Dictionary<string, Sprite> UIEntities; //Dicitionary of unity Sprites (for specific animations)
                
        // Initializes content lists searching for any objects assigned with given tags
        // and deactivating them right after. (If they are already disabled, they can't be found)
        // Doesn't deactivate labels since they are already inside some content
        public void initContents () {
            UIContent[] foundContent = GameObject.FindObjectsOfType<UIContent>();
            UIContents = new Dictionary<string, UIContent>();
            foreach (UIContent go in foundContent) {
                GameManager.Debugger(go.name+" added to UIContents.");
                UIContents.Add(go.name,go);
            }
        }

        #region BUTTONS_CALLBACKS : Every button callback name must be ObjectCallingName+Callback()

        public void ButtonCallback (string buttonName) {
            GameManager.Debugger("Button click callback : " + buttonName);
            string methodName = buttonName + "Click";

            MethodInfo myMethod = this.GetType().GetMethod(methodName);
            myMethod.Invoke(this,null);
        }
        public void FrankDepoimentoClick () {
            Application.LoadLevel("Depoimento");
        }

        public void PageRightButtonClick () {
            ComicManager.instance.OnPageNext();
        }

        public void PageLeftButtonClick () {
            ComicManager.instance.OnPagePrevious();
        }

        public void DebugButtonClick () {
            GameManager.DebuggerEnabled();
        }

        #endregion

        #region REGULAR CALLBACKS : Regular callbacks for UI change on given moments of the game

        public void OnPotionMakingStart () {
            // Start make potion flow callback
        }

        

        // Use to update GUI objects when entering new states
        public void OnUpdateStateUI (string myState) {
            /*
            * ==================================================
            * Current state names defined@GameManager prefab (5)
            * ==================================================
            * Intro
            * Comic
            * Testimonial
            * MiniGame
             * 
            */
            GameManager.Debugger("Updating UI for " + myState + " state.");

            switch (myState) {
                case "Intro":
                    break;
                case "Comic":
                    ComicManager.instance.OnPageLoad();
                    break;
                default:
                    break;
            }
        }

        #endregion
       
        
        // Updates the given main content, setting to false the current one and true the next one,
        // after searching the list for the Panel + given State name, then sets the updated one to 
        // be the current displayedContent. If mainContent is not found, searches for general content.
        public UIContent GetUI (string guiElement) {
            return (UIContents[guiElement]);
        }
    }
}