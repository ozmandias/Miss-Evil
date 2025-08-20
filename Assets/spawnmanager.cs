using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnmanager : MonoBehaviour {

    [SerializeField] Transform[] spawn_locations = new Transform[2];
    [SerializeField] GameObject spider_enemy;
	int spider_enemy_count = 0;
	bool game_start = false;
	IEnumerator spider_coroutine;
	gamemanager spawn_game_manager;

	// Use this for initialization
	void Start () {
		game_start = true;
		spawn_game_manager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
		spawn_enemy();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn_enemy() {
		spider_coroutine = spawn_spider();
		StartCoroutine(spider_coroutine);
    }

	public void set_spider_enemy_count(int new_enemy_count) {
		spider_enemy_count = new_enemy_count;
	}

	public void resize_spider_enenmy_count(int resize_number) {
		spider_enemy_count = spider_enemy_count - resize_number;
	}

	IEnumerator spawn_spider() {
		while(game_start == true){
			if(spider_enemy_count < 10) {
				if(spawn_game_manager.getPauseGame() == false){
					spider_enemy_count = spider_enemy_count + 1;
					int random_index = Random.Range(0,2);
					Instantiate(spider_enemy, spawn_locations[random_index].position, Quaternion.identity);
				}
			}
			yield return new WaitForSeconds(4);
		}
	}
}
