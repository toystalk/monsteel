using UnityEngine;
using System.Collections;
using Assets.Scripts.Core;

public class RAManager : Singleton<RAManager> {
    
    [SerializeField]
    GameObject ARCamera;

    public void initRA () {
        ARCamera = GameObject.Find("VuforiaContainer").transform.GetChild(0).gameObject;
    }

    void TurnCameraOn () {
        try {
            ARCamera.SetActive(true);
        }
        catch (System.Exception error) {
            GameManager.Debugger("Invalid camera.\n" + error.Message);
        }
    }

    void TurnCameraOff () {
        try {
            ARCamera.SetActive(false);
        }
        catch(System.Exception error){
            GameManager.Debugger("Invalid camera.\n" + error.Message);
        }
    }
}
