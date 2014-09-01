/* 
 * Copyright (C) TOYS TALK - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 *
 * Written by Daniel Costa <danielcosta@toystalk.com>, April 2014
 */

using UnityEngine;
using System.Collections;

// Generic class to be extended in any class that should be a singleton
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    // Private instance reference of the singleton
    private static T _instance;

    // Public access to the instance of this singleton
    public static T instance {
        get {
            if (_instance == null) {
                // Finds the instance of type T
                _instance = (T)FindObjectOfType(typeof(T));

                // Instantiate a gameObject if there is none, with a singleton of type T and name T.Name
                if (_instance == null) {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<T>() as T;
                }
                // Set this singleton's container name to type T name on first access
                _instance.gameObject.name = typeof(T).Name;
                // Set this singleton to be persistent through the scenes
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // Make this singleton child of a gameobject
    public static void makeChildOf(GameObject parent) {
        instance.transform.parent = parent.transform;
    }
}