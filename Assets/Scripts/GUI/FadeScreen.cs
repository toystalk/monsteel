using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour {

    string currentScene;
    string nextScene;
    bool waitForScene;
    TweenAlpha fadeTween;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        currentScene = Application.loadedLevelName;
        fadeTween = gameObject.GetComponent<TweenAlpha>();
	}
	
	// Update is called once per frame
	void Update () {
        if (waitForScene) {

        }
	}

    public void OnFinished () {
        FadeScene();
    }

    void FadeScene () {
        switch (currentScene) {
            case "Splash":
                nextScene = "Start";
                StartCoroutine("FadeStart");
                break;
            default:
                break;
        }
    }

    IEnumerator FadeStart () {
        yield return new WaitForSeconds(2);
        fadeTween.PlayReverse();
        yield return new WaitForSeconds(2);
        GameManager.instance.startGame();
        yield return new WaitForSeconds(4);
        fadeTween.PlayForward();
    }
}
