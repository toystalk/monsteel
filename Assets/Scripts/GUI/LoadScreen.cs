using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour {

    TweenPosition loadTween;

    public static void loadIn () {
       LoadScreen [] go = FindObjectsOfType<LoadScreen>();
       foreach (LoadScreen load in go) {
           load.OpenLoad();
       }
    }

    public static void loadOut () {
        LoadScreen[] go = FindObjectsOfType<LoadScreen>();
        foreach (LoadScreen load in go) {
            load.CloseLoad();
        }
    }

    public static void updateRoot () {
        LoadScreen[] go = FindObjectsOfType<LoadScreen>();
        GameObject temp = null;
        foreach (LoadScreen load in go) {
            temp = load.transform.parent.transform.parent.gameObject;
            load.moveRoot(GameObject.Find("_PosLoadPanel").transform);
        }
        Destroy(temp);
    }

    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(transform.parent.transform.parent.gameObject);
        loadTween = gameObject.GetComponent<TweenPosition>();
    }

    public void OpenLoad () {
        loadTween.PlayForward();
        if (this.name == "LoadTop") {
            AudioManager.playEffectAfter("Gate", 1.6f);
        }
    }

    public void CloseLoad () {
        loadTween.PlayReverse();
        if (this.name == "LoadTop") {
            AudioManager.playEffectAfter("Gatereverse", 0.01f);
        }
    }

    public void moveRoot (Transform father) {
        transform.parent = father;
    }
}

