using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class witchgirl : MonoBehaviour {

	CharacterController witchgirlcontroller;
	GameObject witchgirlcameraobject;
    Animator witchgirlanimator;
	float horizontal;
	float vertical;
	Vector3 direction;
	float rotatedirection;
	float smoothrotatedirection;
	float smoothrotatevelocity;
	float smoothrotatetime=0.2f;
	float smoothrotatespeed=Mathf.Infinity;
	float speed=1f;
	float jump=4f;
	float gravity=0.2f;
	float airdirection;
	bool movekeypress=false;
	[SerializeField]BoxCollider arm_attack_collider;
	[SerializeField]BoxCollider basket_attack_collider;
    int witchgirlattackrandom;
	gamemanager witch_girl_game_manager;
	uimanager witch_girl_ui_manager;
	int witch_girl_health = 100;
	bool witch_girl_death = false;
	[SerializeField]AudioSource run_sound;
	[SerializeField]AudioSource attack_sound;
	[SerializeField]AudioSource fall_sound;
	bool witch_girl_hit = false;

	// Use this for initialization
	void Start () {
		witchgirlcontroller = gameObject.GetComponent<CharacterController>();
		arm_attack_collider.enabled = false;
		basket_attack_collider.enabled = false;
		witchgirlcameraobject = GameObject.Find("Main Camera");
        witchgirlanimator = gameObject.GetComponent<Animator>();
		witch_girl_game_manager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
		witch_girl_ui_manager = GameObject.Find("UICanvas").GetComponent<uimanager>();
	}
	
	// Update is called once per frame
	void Update () {
		move();
        attack();
		check_death();
	}

	void move(){
		checkwitchgirlmove();
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		direction = new Vector3(horizontal,0,vertical);
		if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
			if(witch_girl_game_manager.getPauseGame() == false && witch_girl_death == false){
				rotatedirection = Mathf.Atan2(horizontal,vertical)*Mathf.Rad2Deg+witchgirlcameraobject.transform.eulerAngles.y;
				smoothrotatedirection = Mathf.SmoothDamp(rotatedirection,smoothrotatedirection,ref smoothrotatevelocity,smoothrotatetime,smoothrotatespeed,Time.deltaTime);
				gameObject.transform.eulerAngles = Vector3.up*smoothrotatedirection;
			}
		}
		direction=transform.forward;
		if(witchgirlcontroller.isGrounded==true && Input.GetKeyDown(KeyCode.Space)){
			// direction.y=jump;
			// airdirection=jump; // Use for jumping
		}
		// direction.y = direction.y-gravity;
		airdirection=airdirection-gravity;
		direction.y=airdirection;
		if(movekeypress==true && witch_girl_game_manager.getPauseGame() == false && witch_girl_death == false){
            playrunanimation();
			playRunSound();
		}else if(movekeypress == false && witch_girl_game_manager.getPauseGame() == false && witch_girl_death == false){
			direction = new Vector3(0,direction.y,0);
            stoprunanimation();
			stopRunSound();
        }
		if(witch_girl_game_manager.getPauseGame() == false && witch_girl_death == false){
			witchgirlcontroller.Move(direction*speed*Time.deltaTime);
		}else if(witch_girl_game_manager.getPauseGame() == true || witch_girl_death == true){
			witchgirlcontroller.Move(new Vector3(0, direction.y, 0) * speed * Time.deltaTime);
		}
	}

    void attack(){
        if(Input.GetKeyDown(KeyCode.Mouse0) && witch_girl_game_manager.getPauseGame() == false && witch_girl_hit==false && witch_girl_death == false){
            int witchgirlattackrandom = Random.Range(1,3);
            if(witchgirlattackrandom==1){
				arm_attack_collider.enabled = true;
                playattackanimation();
				playAttackSound();
				StartCoroutine(resetAttack(1));
            }else if(witchgirlattackrandom==2){
				basket_attack_collider.enabled = true;
                playattackanimation2();
				playAttackSound();
				StartCoroutine(resetAttack(2));
            }
        }
    }

	void checkwitchgirlmove(){
		if(Input.GetKeyDown(KeyCode.W)){
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.A)){
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.S)){
			movekeypress=true;
		}
		if(Input.GetKeyDown(KeyCode.D)){
			movekeypress=true;
		}
		if(Input.GetKeyUp(KeyCode.W)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				movekeypress=true;
			}else{
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.A)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				movekeypress=true;
			}else{
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.S)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				movekeypress=true;
			}else{
				movekeypress=false;
			}
		}
		if(Input.GetKeyUp(KeyCode.D)){
			if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.D)){
				movekeypress=true;
			}else{
				movekeypress=false;
			}
		}
	}

    void playrunanimation(){
        witchgirlanimator.SetBool("Run",true);
    }

    void stoprunanimation(){
        witchgirlanimator.SetBool("Run",false);
    }

    void playattackanimation(){
        witchgirlanimator.SetTrigger("Attack1");
    }

    void playattackanimation2(){
        witchgirlanimator.SetTrigger("Attack2");
    }

	void playhitanimation(){
		witchgirlanimator.SetTrigger("Hit");
	}

	void playdeathanimation(){
		witchgirlanimator.SetTrigger("Death");
	}

	void playRunSound(){
		run_sound.Play();
	}

	void stopRunSound(){
		run_sound.Stop();
	}

	void playAttackSound(){
		attack_sound.Play();
	}

	void playFallSound(){
		fall_sound.Play();
	}

	void check_death() {
		if(witch_girl_health <= 0 && witch_girl_death == false) {
			witch_girl_death = true;
			playdeathanimation();
			playFallSound();
			witch_girl_game_manager.gameOver();
		}
	}
	
	public void takeDamage(int damage_amount) {
		if(witch_girl_death == false){
			witch_girl_health = witch_girl_health - damage_amount;
			witch_girl_hit = true;
			playhitanimation();
			StartCoroutine(resetWitchGirlHit());
			witch_girl_ui_manager.updateHealthBar(witch_girl_health, damage_amount);
		}
	}

	IEnumerator resetAttack(int reset_attack_type){
		float waitTime = 0f;
		if(reset_attack_type == 1){
			waitTime = 1.0f;
		}else if(reset_attack_type == 2){
			waitTime = 1.4f;
		}
		yield return new WaitForSeconds(waitTime);
		if(reset_attack_type == 1){
			arm_attack_collider.enabled = false;
		}else if(reset_attack_type == 2){
			basket_attack_collider.enabled = false;
		}
	}

	IEnumerator resetWitchGirlHit(){
		yield return new WaitForSeconds(1);
		witch_girl_hit = false;
	}

	void OnTriggerEnter(Collider other_collider){
		
	}
}