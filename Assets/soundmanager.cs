using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundmanager : MonoBehaviour {

	[SerializeField]AudioSource background_sound;
	[SerializeField]AudioSource button_click_sound;
	[SerializeField]AudioSource game_over_sound;

	// Use this for initialization
	void Start () {
		play_background_sound();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void play_background_sound(){
		background_sound.Play();
	}

	public void play_button_click_sound(){
		button_click_sound.Play();
	}

	public void play_game_over_sound(){
		game_over_sound.Play();
	}
}
