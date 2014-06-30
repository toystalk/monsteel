using UnityEngine;
using System.Collections;

public class ToyManager : Singleton<ToyManager> {
    [SerializeField]
    ParticleSystem showUpEffectDracula;
    [SerializeField]
    ParticleSystem showUpEffectFrank;
    [SerializeField]
    ParticleSystem batEffect;

    public enum toyRenderState {
        PreRA,
        Comic1,
        Comic2,
        Bats,
        Smoke,
        Frank
    }
    public toyRenderState currentState;

    public GameObject comic1Prefab;
    public GameObject comic2Prefab;
    public GameObject comic1;
    public GameObject comic2;

    public GameObject backDraculaPanel;
    public GameObject findDraculaPanel;
    public GameObject coverPanelDracula;
    public GameObject coverPanelFrank;
    public GameObject startObj1;
    public GameObject startObj2;
    public GameObject dracula;
    public GameObject draculaFill;
    public GameObject frank;
    public GameObject frankFill;

    Camera ARBGCamera;

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(0.2f);
        //centerVuforiaCameraRect();
        switch (GameManager.instance.currentToy) {
            case GameManager.ToyState.Dracula:
                GameManager.instance.currentToy = GameManager.ToyState.Dracula;
                coverPanelDracula.SetActive(true);
                coverPanelFrank.SetActive(false);
                dracula.SetActive(false);
                break;
            case GameManager.ToyState.Frank:
                GameManager.instance.currentToy = GameManager.ToyState.Frank;
                coverPanelDracula.SetActive(false);
                coverPanelFrank.SetActive(true);
                frank.SetActive(false);
                break;
            default:
                break;


        }
        //CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}
	
	//Update is called once per frame
	void Update () {
        // INPUT DISABLED FOR FRANK
         if (Input.touchCount > 0 && batEffect.isStopped && dracula.activeSelf==true && currentState == toyRenderState.Smoke) {
            batEffect.Play();
            AudioManager.instance.playEffect("BatRA");
        }
	}

    public void ToyFoundCallback () {
        switch (currentState) {
            case toyRenderState.PreRA:
                frank.SetActive(false);
                dracula.SetActive(false);
                switch(GameManager.instance.currentToy){
                    case GameManager.ToyState.Dracula:
                        if (draculaFill.activeSelf == false) {
                            startDraculaComic();
                        }
                        break;
                    case GameManager.ToyState.Frank:
                        if (frankFill.activeSelf == false) {
                            startDraculaComic();
                        }
                        break;
                    default:
                        break;
                }                
                break;
            case toyRenderState.Comic1:
                //comic1.SetActive(true);
                break;
            case toyRenderState.Smoke:
                findDraculaPanel.SetActive(false);
                switch(GameManager.instance.currentToy){
                    case GameManager.ToyState.Dracula:
                        if (showUpEffectDracula.isPlaying == false) {
                            showUpEffectDracula.Play();
                            batEffect.Stop();
                            AudioManager.instance.playEffect("Puff2");
                        }
                        dracula.SetActive(true);
                        break;
                    case GameManager.ToyState.Frank:
                        if (showUpEffectFrank.isPlaying == false) {
                            showUpEffectFrank.Play();
                            AudioManager.instance.playEffect("Puff2");
                        }
                        frank.SetActive(true);
                        break;
                    default:
                        break;
                }
                
                //batEffect.Stop();
                break;
            default:
                break;
        }
        
        
        Debug.Log("Dracula tracking Found CallBack");
    }

    public void ToyLostCallback () {
        switch (currentState) {
            case toyRenderState.PreRA:
                break;
            case toyRenderState.Comic1:
                break;
            case toyRenderState.Smoke:
                findDraculaPanel.SetActive(true);

                switch (GameManager.instance.currentToy) {
                    case GameManager.ToyState.Dracula:
                        showUpEffectDracula.Stop();
                        dracula.SetActive(false);
                        batEffect.Stop();
                        break;
                    case GameManager.ToyState.Frank:
                        showUpEffectFrank.Stop();
                        frank.SetActive(false);
                        break;
                    default:
                        break;
                }
                //batEffect.Stop();
                break;
            default:
                break;
        }
        Debug.Log("Dracula tracking Lost CallBack");
    }

    public void updateState (toyRenderState nextState) {
        switch (nextState) {
            case toyRenderState.PreRA:
                AudioManager.instance.stopBGM();
                resetComic("comic1");
                resetComic("comic2");
                //centerVuforiaCameraRect();
                startObj1.SetActive(false);
                startObj2.SetActive(false);
                draculaFill.SetActive(false);
                frankFill.SetActive(false);
                dracula.SetActive(false);
                frank.SetActive(false);
                showCoverPanel(1);
                break;
            case toyRenderState.Comic1:
                AudioManager.instance.stopBGM();
                resetComic("comic2");
                comic1.SetActive(true);
                comic2.SetActive(false);
                break;
            case toyRenderState.Comic2:
                AudioManager.instance.stopBGM();
                if (currentState == toyRenderState.Smoke) {
                    switch (GameManager.instance.currentToy) {
                        case GameManager.ToyState.Dracula:
                            showUpEffectDracula.Stop();
                            dracula.SetActive(false);
                            batEffect.Stop();
                            break;
                        case GameManager.ToyState.Frank:
                            showUpEffectFrank.Stop();
                            frank.SetActive(false);
                            break;
                        default:
                            break;
                    }
                    findDraculaPanel.SetActive(false);
                    backDraculaPanel.SetActive(false);
                }
                resetComic("comic1");
                comic2.SetActive(true);
                comic1.SetActive(false);
                break;
            case toyRenderState.Smoke:
                AudioManager.instance.stopBGM();
                //resetVuforiaCameraRect();
                resetComic("comic2");
                findDraculaPanel.SetActive(true);
                backDraculaPanel.SetActive(true);
                if (currentState == toyRenderState.Comic1 || currentState == toyRenderState.Comic2) {
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
        Debug.Log("UNITY - CAMERA VUFORIA  = " + temp);
        temp.rect = new Rect(0.26f, 0.17f, 0.49f, 0.42f);
        Debug.Log("UNITY - CAMERA VUFORIA  = " + temp.rect);
    }

    void resetVuforiaCameraRect () {
        Camera temp = FindObjectOfType<SetBGCameraLayerBehaviour>().GetComponent<Camera>();
        Debug.Log("UNITY - CAMERA VUFORIA  = " + temp);
        temp.rect = new Rect(0, 0, 1, 1);
        Debug.Log("UNITY - CAMERA VUFORIA  = " + temp.rect);
    }

    public void startComic () {
        StartCoroutine("comicInitAnimation");
    }

    void startDraculaComic () {
        StartCoroutine("comicStartAnimation");
    }

    IEnumerator comicInitAnimation () {
        updateState(toyRenderState.Comic1);
        showCoverPanel(0);
        yield return new WaitForSeconds(1.0f);
        startObj1.SetActive(false);
        startObj2.SetActive(false);
        draculaFill.SetActive(false);
        frankFill.SetActive(false);
    }

    IEnumerator comicStartAnimation(){
        switch (GameManager.instance.currentToy) {
            case GameManager.ToyState.Dracula:
                if (draculaFill.activeSelf == false) {
                    //Dracula clock
                    UISprite clock = draculaFill.GetComponent<UISprite>();
                    clock.fillAmount = 0;
                    draculaFill.SetActive(true);
                    do {
                        yield return new WaitForSeconds(0.01f);
                        clock.fillAmount += 0.01f;
                    } while (clock.fillAmount < 1);
                }
                startObj1.SetActive(true);
                break;
            case GameManager.ToyState.Frank:
                //Dracula clock
                UISprite clock2 = frankFill.GetComponent<UISprite>();
                clock2.fillAmount = 0;
                frankFill.SetActive(true);
                do {
                    yield return new WaitForSeconds(0.01f);
                    clock2.fillAmount += 0.01f;
                } while (clock2.fillAmount < 1);
                startObj2.SetActive(true);
                break;
            default:
                break;
        } 
    }

    void showCoverPanel (float show) {
        startObj1.SetActive(false);
        startObj2.SetActive(false);
        switch (GameManager.instance.currentToy) {
            case GameManager.ToyState.Dracula:
                coverPanelDracula.SetActive(true);
                coverPanelFrank.SetActive(false);
                foreach (Transform child in coverPanelDracula.transform) {
                    TweenAlpha.Begin(child.gameObject, 0.5f, show);
                }
                break;
            case GameManager.ToyState.Frank:
                coverPanelDracula.SetActive(false);
                coverPanelFrank.SetActive(true);
                foreach (Transform child in coverPanelFrank.transform) {
                    TweenAlpha.Begin(child.gameObject, 0.5f, show);
                }
                break;
            default:
                break;
        }      
    }

    void resetComic (string comic) {
        if (comic == "comic1") {
            Destroy(comic1);
            comic1 = Instantiate(comic1Prefab, comic1Prefab.transform.position, comic1Prefab.transform.rotation) as GameObject;
            comic1.transform.parent = GameObject.Find("_UI Root").transform;
            comic1.transform.position = comic1Prefab.transform.position;
            comic1.transform.localScale = comic1Prefab.transform.localScale;
            comic1.SetActive(false);
        }
        else if(comic == "comic2") {
            Destroy(comic2);
            comic2 = Instantiate(comic2Prefab, comic2Prefab.transform.position, comic2Prefab.transform.rotation) as GameObject;
            comic2.transform.parent = GameObject.Find("_UI Root").transform;
            comic2.transform.position = comic2Prefab.transform.position;
            comic2.transform.localScale = comic2Prefab.transform.localScale;
            comic2.SetActive(false);
        }
    }
}
