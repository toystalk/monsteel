using UnityEngine;
using System.Collections;

public class PageContent : MonoBehaviour {
    
    void OnDisable () {
    }

	void OnEnable () {
        ChildActiveAll();    
	}
    
    void ChildActiveAll () {
        Animator ac = GetComponent<Animator>();

        if (ac != null) {
            ac.SetTrigger("Play");
        }
    }
}
