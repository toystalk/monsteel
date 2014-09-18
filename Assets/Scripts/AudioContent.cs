using UnityEngine;
using System.Collections;


// Audio content class to act as a container of audio information, 
// such as AudioClips, specific sources, parameters, and audio names.
// Serialized to be shown in the inspector.
[System.Serializable]
public class AudioContent {

    public string name; // Name of the content.
    public AudioClip content; // Audio clip of the content
    public AudioSource source; // Set the source of the audio if there should be any
    public bool loop; // Set the audio to loop or not

    public AudioContent () {
        name = "UniqueAudioName";
        loop = false;
    }
}

