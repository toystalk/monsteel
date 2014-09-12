using UnityEngine;
using System.Collections;

public class ControlFrankHead : MonoBehaviour {

    public MegaPointCache mp;
    public Animator ac;
    public AudioSource asrc;
    bool ativado = false;

	// Use this for initialization
	public void InitFrank () {
        Debug.Log("START");
        mp.animated = true;
        ac.SetTrigger("Play");
        ativado = true;
        asrc.Play();
	}

    public void StopFrank () {
        Debug.Log("STOP");
        asrc.Stop();
        mp.animated = false;
        mp.time = 0;
        ac.SetTrigger("Reset");
    }
	
	// Update is called once per frame
	void Update () {
        if (ativado) {
            if (mp.time >= mp.maxtime) {
                mp.animated = false;
                mp.time = 0;
            }
        }
	}
}
