using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour {

	bool visibility = false;
	bool pause_game = false;
	bool leave_game = false;
	string leave_method = "";
	uimanager game_ui_manager;
	GameObject[] spider_objects;
	int score = 0;
	bool game_over = false;

	// Use this for initialization
	void Start () {
		Cursor.visible = visibility;
		game_ui_manager = GameObject.Find("UICanvas").GetComponent<uimanager>();
	}
	
	// Update is called once per frame
	void Update () {
		togglePause();
		toggleCursorVisibility();
		checkGameOver();
	}

	void OnGUI() {
		if(pause_game == false && game_over == false){
			Cursor.lockState = CursorLockMode.Locked;
		}else if(pause_game == true && game_over == false){
			Cursor.lockState = CursorLockMode.None;
		}
	}

	void togglePause() {
		if(Input.GetKeyDown(KeyCode.Escape) && game_over == false){
			if(pause_game == false) {
				pause_game = true;
			}else{
				pause_game = false;
			}
		}
	}

	void toggleCursorVisibility() {
		if(pause_game){
			visibility = pause_game;
			Cursor.visible = visibility;
		}
	}

	void checkGameOver() {
		if(game_over) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			game_ui_manager.showGameOverPanel();
		}
	}

	public void updateScore() {
		score = score + 1;
		string score_text = "" + score;
		game_ui_manager.updateScoreText(score_text);
	}

	public void resumeGame() {
		pause_game = false;
	}

	public void toggleLeaveGame(string game_leave_method) {
		leave_method = game_leave_method;
		if(leave_game == false) {
			if(leave_method == "mainmenu"){
				game_ui_manager.changeConfirmLeaveText("Are you sure you want to quit to main menu?");
			}else if(leave_method == "exit"){
				game_ui_manager.changeConfirmLeaveText("Are you sure you want to exit the game?");
			}
			leave_game = true;
		}else{
			leave_game = false;
			game_ui_manager.changeConfirmLeaveText("Are you sure you want to leave the game?");
		}
	}

	public void reloadGame() {
		SceneManager.LoadScene("game", LoadSceneMode.Single);
	}

	public void leaveGame() {
		if(leave_method == "mainmenu"){
			loadMainMenu();
		}else if(leave_method == "exit"){
			exitGame();
		}
	}

	public void loadMainMenu() {
		SceneManager.LoadScene("mainmenu", LoadSceneMode.Single);
	}

	public void exitGame() {
		Application.Quit();
	}

	public bool getPauseGame() {
		return pause_game;
	}

	public bool getLeaveGame() {
		return leave_game;
	}

	public void gameOver() {
		game_over = true;
	}

	public void collectSpiderObjects() {
		spider_objects = GameObject.FindGameObjectsWithTag("Enemy");
	}
}
