using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainmenumanager : MonoBehaviour {
	[SerializeField]GameObject options_panel;
	[SerializeField]GameObject credits_panel;
	[SerializeField]GameObject confirm_exit_panel;

	// Use this for initialization
	void Start () {
		options_panel.gameObject.SetActive(false);
		credits_panel.gameObject.SetActive(false);
		confirm_exit_panel.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startGame() {
		SceneManager.LoadScene("game", LoadSceneMode.Single);
	}

	public void toggleOptions() {
		options_panel.gameObject.SetActive(!options_panel.gameObject.activeSelf);
	}

	public void toggleCredits() {
		credits_panel.gameObject.SetActive(!credits_panel.gameObject.activeSelf);
	}

	public void toggleConfirmExit() {
		confirm_exit_panel.gameObject.SetActive(!confirm_exit_panel.gameObject.activeSelf);
	}

	public void exit() {
		Application.Quit();
	}
}
