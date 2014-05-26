using UnityEngine;
using System.Collections;

public class ToyManager : Singleton<ToyManager> {
    [SerializeField]
    ParticleSystem showUpEffect;

    public enum draculaState {
        Comic1,
        Comic2,
        Bats,
        Smoke
    }
    draculaState currentState;

    public GameObject comic1;
    public GameObject comic2;
    public GameObject dracula;

	// Use this for initialization
	void Start () {
        currentState = draculaState.Comic1;
        //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1) {
            updateState(draculaState.Comic1);
        }
        if (Input.touchCount == 2) {
            updateState(draculaState.Comic2);
        }
        if (Input.touchCount == 3) {
            updateState(draculaState.Smoke);
        }
	}

    public void DraculaFoundCallback () {
        switch (currentState) {
            case draculaState.Comic1:
                comic1.SetActive(true);
                break;
            case draculaState.Smoke:
                if (showUpEffect.isPlaying == false) {
                    showUpEffect.Play();
                }
                dracula.SetActive(true);
                break;
            default:
                break;
        }
        
        
        Debug.Log("Dracula tracking Found CallBack");
    }

    public void DraculaLostCallback () {
        switch (currentState) {
            case draculaState.Comic1:
                break;
            case draculaState.Smoke:
                showUpEffect.Stop();
                dracula.SetActive(false);
                break;
            default:
                break;
        }
        Debug.Log("Dracula tracking Lost CallBack");
    }

    public void updateState (draculaState nextState) {
        switch (nextState) {
            case draculaState.Comic1:
                comic1.SetActive(true);
                comic2.SetActive(false);
                break;
            case draculaState.Comic2:
                comic2.SetActive(true);
                comic1.SetActive(false);
                break;
            case draculaState.Smoke:
                if (currentState == draculaState.Comic1 || currentState == draculaState.Comic2) {
                    comic2.SetActive(false);
                    comic1.SetActive(false);
                }
                break;
            default: 
                break;
        }
        currentState = nextState;
    }
}
