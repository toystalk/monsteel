/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, August 2014
 * 
 * Class to manage Game State entitie, with attributes relevant to every game state.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Core {
    /*
     * Current state names defined@GameManager prefab 
     * ==============================================
     * Home : home screen
     * Lab : lab island menu
     * Potion : choose potion to be made
     * Remove : remove all ingredients from the cauldron
     * InsertIgr : insert an ingredient in the cauldron
     * 
     */
    [System.Serializable]
    public class GameState {
        [SerializeField]
        string myStateName;
        [SerializeField]
        string myScene;
        public GameStateHandler myState;
        [SerializeField]
        List<GameStateHandler> Next;
        [SerializeField]
        List<GameStateHandler> Prev;

        public bool FindNext (GameStateHandler nextGs) {

            bool found = Next.Exists(delegate(GameStateHandler gs) {
                return gs == nextGs;
            });

            return found;
        }

        public string GetName () {
            return myStateName;
        }

        public string GetScene () {
            return myScene;
        }
    }

}
