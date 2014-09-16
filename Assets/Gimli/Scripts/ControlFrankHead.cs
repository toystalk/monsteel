using UnityEngine;
using System.Collections;

public class ControlFrankHead : MonoBehaviour {

    public MegaPointCache mp;
    public MegaPointCache mp2;
    public AudioSource asrc;
    bool ativado = false;

	// Use this for initialization
	public void InitFrank () {
        Debug.Log("START");
        mp.gameObject.SetActive(true);
        mp.time = 0;
        mp.animated = true;
        mp2.time = 0;
        mp.transform.parent.gameObject.SetActive(true);
        ativado = true;
        asrc.Play();
	}

    public void StopFrank () {
        Debug.Log("STOP");
        asrc.Stop();
        mp.animated = false;
        mp2.animated = false;
        mp2.gameObject.SetActive(false);
        mp.transform.parent.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (ativado) {
            if (mp.time >= mp.maxtime) {
                mp.animated = false;
                mp.time = 0;
                mp.gameObject.SetActive(false);
                mp2.gameObject.SetActive(true);
                mp2.animated = true;                
            }
        }
	}
}
