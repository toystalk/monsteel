using UnityEngine;
using System.Collections;

public class PageContent : MonoBehaviour {

    bool childActive = false;

	void OnEnable () {
        childActive = true;
        Invoke("ChildActiveAll", 3.0f);    
	}

    void OnDisable () {
        childActive = false;
        ChildActiveAll();
        CancelInvoke();
    }

    void ChildActiveAll () {
        foreach (Transform t in transform) {
            t.gameObject.SetActive(childActive);
        }
    }
}
