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
using System;

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
                AudioListener[] tempListeners = GameObject.FindObjectsOfType<AudioListener>();
                GameObject go = null;
                foreach (AudioListener gameObj in tempListeners) {
                    if (gameObj.enabled == true) {
                        go = gameObj.gameObject;
                    }
                }
                Debug.Log("UNITY - GameObject go ref : " + go);
                if (go != null) {
                    _masterBGM = go.GetComponent<AudioSource>();
                    if (_masterBGM == null) {
                        _masterBGM = go.AddComponent<AudioSource>() as AudioSource;
                    }
                }
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
                AudioListener[] tempListeners = GameObject.FindObjectsOfType<AudioListener>();
                GameObject go = null;
                foreach (AudioListener gameObj in tempListeners) {
                    if (gameObj.enabled == true) {
                        go = gameObj.gameObject;
                    }
                }

                if (go != null) {
                    _masterEffects = go.GetComponent<AudioSource>();
                    if (_masterEffects == null) {
                        _masterEffects = go.AddComponent<AudioSource>() as AudioSource;
                    }
                }
            }
            return _masterEffects;
        }
    }
    /*
    [SerializeField]
    private AudioListener _masterListener;
    public AudioListener masterListener {
        get {
            if (_masterListener == null) {
                _masterListener = GetComponent<AudioListener>();
                if(_masterListener == null){
                    _masterListener = this.gameObject.AddComponent<AudioListener>() as AudioListener;
                }                    
            }
            return _masterListener;
        }
    }*/

    // List of all audio content there is to be played
    // TODO(danielcosta) : the list will be used to find any audio that you want to play.
    // Search should be done through delegate or a dictionary with position reference.
    // This structure should be kept this way since it's the best one to show in the inspector.
    public List<AudioContent> audioList;

    // Use this for initialization
    void Awake () {
        initSources();
    }

    // Update is called once per frame
    void Update () {
    }

    // Initializes both master audio sources (BGM and Effects) 
    public void initSources () {
        if (!masterBGM) {
            Debug.Log("Failed to initialize MasterBGM AudioSource, reference :" + masterBGM);
        }
        if (!masterEffects) {
            Debug.Log("Failed to initialize MasterEffects AudioSource, reference :" + masterEffects);
        }

        try {
            AudioContainer tempList = GameObject.Find("AudioContainer").GetComponent<AudioContainer>();

            if (tempList != null) {
                this.audioList = tempList.audioList;
                Destroy(tempList.gameObject);
                Debug.Log("UNITY - Audio List initialized with " + audioList.Count + " audios.");
            }
        }
        catch (Exception error) {
            Debug.Log(error);
        }
    }

    // Searches the audio list after the string received and plays the audio from masterBGM
    public void playBGM (string audio) {
        try {
            // Find audio in the list and play from mastersource
            AudioContent som = audioList.Find(
                delegate(AudioContent ac) {
                    return ac.name == audio;
                });

            if (som != null) {
                if (som.source == null) {
                    masterBGM.audio.clip = som.content;
                    masterBGM.loop = som.loop;
                    masterBGM.Play();
                }
                else {
                    AudioSource tempSource = som.source;
                    tempSource.audio.clip = som.content;
                    tempSource.loop = som.loop;
                    tempSource.Play();
                }
            }
        }
        catch (Exception error) {
            Debug.Log(error.Message);
        }
    }

    // Searches the audio list after the string received and plays the audio from masterEffects
    public void playEffect (string audio) {
        Debug.Log("UNITY - Audio Effect playing at " + AudioManager.instance + " with audio " + audio);

        try {
            // Find audio in the list and play from mastersource
            AudioContent som = audioList.Find(
                delegate(AudioContent ac) {
                    return ac.name == audio;
                });

            if (som != null) {
                if (som.source == null) {
                    masterEffects.PlayOneShot(som.content);
                }
                else {
                    som.source.PlayOneShot(som.content);
                }
            }

        }
        catch (Exception error) {
            Debug.Log(error.Message);
        }
    }

    public void stopBGM () {
        masterBGM.Stop();
    }

    // Searches the audio list after the string received and plays the audio from masterEffects
    public void playEffectVol (string audio, float vol) {
        Debug.Log("UNITY - Audio Effect playing at " + AudioManager.instance + " with audio " + audio);
        try {
            // Find audio in the list and play from mastersource
            AudioContent som = audioList.Find(
                delegate(AudioContent ac) {
                    return ac.name == audio;
                });

            if (som != null) {

                if (som.source == null) {
                    masterEffects.PlayOneShot(som.content, vol);
                }
                else {
                    som.source.PlayOneShot(som.content, vol);
                }
            }

        }
        catch (Exception error) {
            Debug.Log(error.Message);
        }
    }

    public static void playEffectAfter (string audio, float afterSecs) {
        AudioManager.instance.StartCoroutine(playEffectIEnum(audio, afterSecs));
    }

    static IEnumerator playEffectIEnum (string audio, float afterSecs) {
        yield return new WaitForSeconds(afterSecs);
        AudioManager.instance.playEffect(audio);
    }

}
