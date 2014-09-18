using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {
	public GameObject maskPanel;
	public GameObject messagePanel;
	public GameObject gameOverPanel;
	public GameObject hudPanel;
	
	public Sprite availableHitPoint;
	public Sprite spentHitPoint;

	public enum GameOverMode {
		Win,
		Lose
	}

	public void DisplayMessage(string message) {
		DisplayMessage(message, true);
	}

	public void DisplayMessage(string message, bool animated) {
		ShowMessages();
	
		GameObject labelObj = messagePanel.transform.FindChild("Message").gameObject;
		UILabel label = labelObj.GetComponent<UILabel>();
		label.text = message;
		label.UpdateNGUIText();
		
		if (animated) {
			labelObj.GetComponent<TweenScale>().ResetToBeginning();
			labelObj.GetComponent<TweenScale>().PlayForward();
		}
	}
	
	public void ShowMessages() {
		NGUITools.SetActive(messagePanel, true);
	}
	
	public void HideMessages() {
		NGUITools.SetActive(messagePanel, false);
	}
	
	public void HideMessages(float delay) {
		StartCoroutine(HideMessagesCoroutine(delay));
	}
	
	private IEnumerator HideMessagesCoroutine(float delay) {
		yield return new WaitForSeconds(delay);
		HideMessages();
	}
	
	public void ShowARMask() {
		NGUITools.SetActive(maskPanel, true);
		HideHUD();
	}
	
	public void HideARMask() {
		NGUITools.SetActive(maskPanel, false);
		ShowHUD();
	}
	
	public void ShowGameOverScreen(GameOverMode mode) {	
		NGUITools.SetActive(gameOverPanel, true);
	
		GameObject youLose = gameOverPanel.transform.FindChild("YouLose").gameObject;
		GameObject youWin = gameOverPanel.transform.FindChild("YouWin").gameObject;
	
		NGUITools.SetActive(youLose, false);
		NGUITools.SetActive(youWin, false);
	
		if (mode == GameOverMode.Win) {
			NGUITools.SetActive(youWin, true);
		} else {
			NGUITools.SetActive(youLose, true);
		}
		
		gameOverPanel.GetComponent<TweenAlpha>().ResetToBeginning();
		gameOverPanel.GetComponent<TweenAlpha>().PlayForward();
		
		HideHUD();
	}
	
	public void HideGameOverScreen(float delay) {
		StartCoroutine(HideGameOverScreenCoroutine(delay));
	}
	
	private IEnumerator HideGameOverScreenCoroutine(float delay) {
		yield return new WaitForSeconds(delay);
		NGUITools.SetActive(gameOverPanel, false);
	}
	
	public void UpdateHitPoints(int hps) {
		GameObject[] hpIndicators = new GameObject[] {
			hudPanel.transform.FindChild("hp_1").gameObject,
			hudPanel.transform.FindChild("hp_2").gameObject,
			hudPanel.transform.FindChild("hp_3").gameObject
		};
		
		// Activate everyone
		for (int i = 0; i < hpIndicators.Length; i++) {
			hpIndicators[i].GetComponent<UI2DSprite>().sprite2D = availableHitPoint;
		}
		
		// Deactivate spent ones
		int toDisable = hpIndicators.Length - hps;
		toDisable = Mathf.Max(toDisable, 0);
		toDisable = Mathf.Min(toDisable, hpIndicators.Length);
		
		for (int i = 0; i < toDisable; i++) {
			hpIndicators[i].GetComponent<UI2DSprite>().sprite2D = spentHitPoint;
		}
	}
	
	public void HideHUD() {
		NGUITools.SetActive(hudPanel, false);
	}
	
	public void ShowHUD() {
		NGUITools.SetActive(hudPanel, true);
	}
}
