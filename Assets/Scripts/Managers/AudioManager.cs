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
using Assets.Scripts.Core;

// Audio content class to act as a container of audio information, 
// such as AudioClips, specific sources, parameters, and audio names.
// Serialized to be shown in the inspector.
[System.Serializable]
public class AudioContent {

    public string name; // Name of the content.
    public AudioClip content; // Audio clip of the content
    public AudioSource source; // Set the source of the audio if there should be any
    public bool loop; // Set the audio to loop or not

    public AudioContent() {
        name = "UniqueAudioName";
        loop = false;
    }
}

// Manager class responsible for dealing with audio clips and audio sources
// TODO(danielcosta) : further implementation requires audio methods that
// attend any kind of situation.
public class AudioManager : Singleton<AudioManager> {

    // MasterSource for BGMs. It should be already attached in the inspector,
    // or be in a GameObject of name "AudioBGM". If it's not, a new one will be created.
    [SerializeField]
    private AudioSource _masterBGM;
    public AudioSource masterBGM {
        get {
            if (_masterBGM == null) {
                GameObject go = GameObject.Find("AudioBGM");
                if (go) {
                    _masterBGM = go.GetComponent<AudioSource>();
                }
                else {
                    go = new GameObject("BGM");
                    _masterBGM = go.AddComponent<AudioSource>() as AudioSource;
                }
                go.transform.parent = gameObject.transform;
            }
            return _masterBGM;
        }
    }
    // MasterSource for Effects. It should be already attached in the inspector,
    // or be in a GameObject of name "AudioEffects". If it's not, a new one will be created.
    [SerializeField]
    private AudioSource _masterEffects;
    public AudioSource masterEffects {
        get {
            if (_masterEffects == null) {
                GameObject go = GameObject.Find("AudioEffects");
                if (go) {
                    _masterEffects = go.GetComponent<AudioSource>();
                }
                else {
                    go = new GameObject("AudioEffects");
                    _masterEffects = go.AddComponent<AudioSource>() as AudioSource;
                }
                go.transform.parent = gameObject.transform;
            }
            return _masterEffects;
        }
    }

    // List of all audio content there is to be played
    // TODO(danielcosta) : the list will be used to find any audio that you want to play.
    // Search should be done through delegate or a dictionary with position reference.
    // This structure should be kept this way since it's the best one to show in the inspector.
    public List<AudioContent> audioList;

    // Use this for initialization
    void Awake() {
        initSources();
    }

    // Update is called once per frame
    void Update() {

    }

    // Initializes both master audio sources (BGM and Effects) 
    void initSources() {
        if (!masterBGM) {
            GameManager.Debugger("Failed to initialize MasterBGM AudioSource, reference : " + masterBGM);
        }
        if (!masterEffects) {
            GameManager.Debugger("Failed to initialize MasterEffects AudioSource, reference : " + masterEffects);
        }
    }

    // Searches the audio list after the string received and plays the audio from masterBGM
    public void playBGM(string audio) {
        // Find audio in the list and play from mastersource
    }

    // Searches the audio list after the string received and plays the audio from masterEffects
    public void playEffects(string audio) {
        // Find audio in the list and play from mastersource
    }


}
