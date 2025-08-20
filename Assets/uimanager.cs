using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uimanager : MonoBehaviour {
	[SerializeField]Image healthbar;
	[SerializeField]Text healthtext;
	[SerializeField]Text total_kill_count_text;
	[SerializeField]GameObject pause_panel;
	[SerializeField]GameObject confirm_leave_panel;
	[SerializeField]Text confirm_leave_text;
	[SerializeField]GameObject game_over_panel;
	gamemanager ui_game_manager;

	// Use this for initialization
	void Start () {
		pause_panel.gameObject.SetActive(false);
		confirm_leave_panel.gameObject.SetActive(false);
		game_over_panel.gameObject.SetActive(false);
		confirm_leave_text.text = "Are you sure you want to leave?";
		ui_game_manager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
	}
	
	// Update is called once per frame
	void Update () {
		togglePausePanel();
		toggleConfirmLeavePanel();
	}

	void togglePausePanel() {
		pause_panel.SetActive(ui_game_manager.getPauseGame());
	}

	void toggleConfirmLeavePanel() {
		if(ui_game_manager.getPauseGame() == true) {
			confirm_leave_panel.SetActive(ui_game_manager.getLeaveGame());
			pause_panel.SetActive(!ui_game_manager.getLeaveGame());
		}
	}

	public void updateHealthBar(int originalHealth, int amountToChange) {
		float healthbar_amount = amountToChange * 0.01f;
		healthbar.fillAmount = healthbar.fillAmount - healthbar_amount;
		int newHealth = originalHealth - amountToChange;
		if(newHealth < 0){
			newHealth = 0;
		}
		healthtext.text = "" + newHealth + "%";
	}

	public void updateScoreText(string textToChange) {
		total_kill_count_text.text = textToChange;
	}

	public void changeConfirmLeaveText(string textToChange) {
		confirm_leave_text.text = textToChange;
	}

	public void showGameOverPanel() {
		game_over_panel.SetActive(true);
	}
}
