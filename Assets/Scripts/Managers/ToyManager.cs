using UnityEngine;
using System.Collections;

public class ToyManager : Singleton<ToyManager> {
    [SerializeField]
    ParticleSystem showUpEffect;

    public enum draculaState {
        PreRA,
        Comic1,
        Comic2,
        Bats,
        Smoke
    }
    public draculaState currentState;

    public GameObject comic1Prefab;
    public GameObject comic2Prefab;
    public GameObject comic1;
    public GameObject comic2;

    public GameObject findDraculaPanel;
    public GameObject coverPanel;
    public GameObject startObj;
    public GameObject dracula;
    public GameObject draculaFill;

    Camera ARBGCamera;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(0.2f);
        centerVuforiaCameraRect();
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
        if (Input.GetKeyDown(KeyCode.Return)) {
            updateState(currentState);
        }
	}

    public void DraculaFoundCallback () {
        switch (currentState) {
            case draculaState.PreRA:
                startObj.SetActive(true);
                break;
            case draculaState.Comic1:
                //comic1.SetActive(true);
                break;
            case draculaState.Smoke:
                findDraculaPanel.SetActive(false);
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
            case draculaState.PreRA:
                break;
            case draculaState.Comic1:
                break;
            case draculaState.Smoke:
                findDraculaPanel.SetActive(true);
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
            case draculaState.PreRA:
                resetComic("comic1");
                resetComic("comic2");
                centerVuforiaCameraRect();
                startObj.SetActive(false);
                draculaFill.SetActive(false);
                showCoverPanel(1);
                break;
            case draculaState.Comic1:
                resetComic("comic2");
                comic1.SetActive(true);
                comic2.SetActive(false);
                break;
            case draculaState.Comic2:
                resetComic("comic1");
                comic2.SetActive(true);
                comic1.SetActive(false);
                break;
            case draculaState.Smoke:
                resetVuforiaCameraRect();
                findDraculaPanel.SetActive(true);
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

    void centerVuforiaCameraRect () {
        Camera temp = FindObjectOfType<SetBGCameraLayerBehaviour>().GetComponent<Camera>();
        temp.rect = new Rect(0.26f, 0.17f, 0.49f, 0.42f);
    }

    void resetVuforiaCameraRect () {
        Camera temp = FindObjectOfType<SetBGCameraLayerBehaviour>().GetComponent<Camera>();
        temp.rect = new Rect(0, 0, 1, 1);
    }

    public void startDraculaComic () {
        StartCoroutine("comicInitAnimation");
    }

    IEnumerator comicInitAnimation(){
        //Dracula clock
        UISprite clock = draculaFill.GetComponent<UISprite>();
        clock.fillAmount = 0;
        draculaFill.SetActive(true);
        do{
            yield return new WaitForSeconds(0.01f);
            clock.fillAmount+=0.01f;
        }while(clock.fillAmount<1);
        yield return new WaitForSeconds(1f);
        updateState(draculaState.Comic1);
        showCoverPanel(0);
        yield return new WaitForSeconds(2);


        yield return null;
    }

    void showCoverPanel (float show) {
        startObj.SetActive(false);
        foreach (Transform child in coverPanel.transform) {
            TweenAlpha.Begin(child.gameObject, 0.5f, show);
        }
    }

    void resetComic (string comic) {
        if (comic == "comic1") {
            Destroy(comic1);
            comic1 = Instantiate(comic1Prefab, comic1Prefab.transform.position, comic1Prefab.transform.rotation) as GameObject;
            comic1.transform.parent = GameObject.Find("UI Root").transform;
            comic1.transform.position = comic1Prefab.transform.position;
            comic1.transform.localScale = comic1Prefab.transform.localScale;
            comic1.SetActive(false);
        }
        else if(comic == "comic2") {
            Destroy(comic2);
            comic2 = Instantiate(comic2Prefab, comic2Prefab.transform.position, comic2Prefab.transform.rotation) as GameObject;
            comic2.transform.parent = GameObject.Find("UI Root").transform;
            comic2.transform.position = comic2Prefab.transform.position;
            comic2.transform.localScale = comic2Prefab.transform.localScale;
            comic2.SetActive(false);
        }
    }
}
