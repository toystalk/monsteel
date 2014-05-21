using UnityEngine;
using System.Collections;

public class ToyManager : Singleton<ToyManager> {
    [SerializeField]
    ParticleSystem showUpEffect;

	// Use this for initialization
	void Start () {
        //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DraculaFoundCallback () {
        if (showUpEffect.isPlaying == false) {
            showUpEffect.Play();
        }
        
        Debug.Log("Dracula tracking Found CallBack");
    }

    public void DraculaLostCallback () {
        showUpEffect.Stop();
        Debug.Log("Dracula tracking Lost CallBack");
    }
}
