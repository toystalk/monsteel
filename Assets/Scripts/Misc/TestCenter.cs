using UnityEngine;
using System.Collections;

public class TestCenter : MonoBehaviour {

	// Use this for initialization
    void Start () {
        Debug.Log("Centering");
        //FindObjectOfType<UICenterOnChild>().CenterOn(GameObject.Find("IconPage7").transform);	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log("Centering");
            FindObjectOfType<UICenterOnChild>().CenterOn(GameObject.Find("IconPage2").transform);
        }
	}
}
