using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour {

    string currentScene;
//    string nextScene;
    bool waitForScene;
    TweenAlpha fadeTween;

	// Use this for initialization
	void Awake () {
        //DontDestroyOnLoad(gameObject);
        currentScene = Application.loadedLevelName;
        fadeTween = gameObject.GetComponent<TweenAlpha>();
	}
	
    public void OnFinished () {
        //FadeScene();
    }

    public void FadeScene () {
        switch (currentScene) {
            case "Splash":
                //nextScene = "Start";
                StartCoroutine("FadeStart");
                break;
            case "Start":
                //nextScene = "Start";
                StartCoroutine("FadeAR1");
                break;
            default:
                break;
        }
    }

    IEnumerator FadeStart () {
        yield return new WaitForSeconds(3);
        fadeTween.PlayReverse(); //fadeOut

        yield return new WaitForSeconds(3);
        GameManager.instance.waitToLoad = false;
        //GameManager.instance.startGame();
    }
}
