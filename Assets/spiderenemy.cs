using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spiderenemy : MonoBehaviour {
	witchgirl witch;
	Animator spider_animator;
	SphereCollider spider_collider;
	Vector3 witchgirldirection;
	[SerializeField]Transform witchgirltransform;
	float rotate_speed = 4f;
	NavMeshAgent spidernavmeshagent;
	[SerializeField]GameObject spider_target_object;
	gamemanager spider_game_manager;
	spawnmanager spider_spawn_manager;
	RaycastHit spider_raycast_hit;
	bool spider_death = false;
	int spider_hit_count = 0;
	float spider_and_witch_distance = 0f;
	bool spider_near_spider = false;
	spiderenemy friendly_spider;
	bool spider_attacking = false;
	int spider_attack_count = 0;
	[SerializeField]AudioSource spider_walk_sound;
	[SerializeField]AudioSource spider_attack_sound;
	[SerializeField]AudioSource spider_death_sound;
	[SerializeField]ParticleSystem spider_attack_particle;

	// Use this for initialization
	void Start () {
		witch = GameObject.Find("witchgirl").GetComponent<witchgirl>();
		spider_animator = gameObject.GetComponent<Animator>();
		spider_collider = gameObject.GetComponent<SphereCollider>();
		witchgirltransform = GameObject.FindWithTag("Player").transform;
		spidernavmeshagent = gameObject.GetComponent<NavMeshAgent>();
		spider_game_manager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
		spider_spawn_manager = GameObject.Find("spawnmanager").GetComponent<spawnmanager>();
	}
	
	// Update is called once per frame
	void Update () {
		checkTarget();
		followWitchGirl();
		spider_attack();
		checkSpiderDeath();
	}

	void followWitchGirl() {
		witchgirldirection = witchgirltransform.position - gameObject.transform.position;
		if(spider_death == false){
			if(spider_game_manager.getPauseGame() == false){
				if(spider_attacking == false){
					gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, witchgirldirection, rotate_speed * Time.deltaTime, 0.0f));
				}
				spidernavmeshagent.destination = witchgirltransform.position;
				if(friendly_spider){
					if(spider_and_witch_distance < friendly_spider.getSpiderAndWitchDistance()){
						spidernavmeshagent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
						stop_run_animation();
						stopWalkSound();
					}
				}
				if(spider_and_witch_distance > 1.8f){
					play_run_animation();
					playWalkSound();
				}else if(spider_and_witch_distance <= 1.8f){
					stop_run_animation();
					stopWalkSound();
				}
			}else{
				spidernavmeshagent.destination = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
				stop_run_animation();
				stopWalkSound();
			}
		}
	}

	void spider_attack() {
		if(spider_and_witch_distance <= 1.8f && spider_game_manager.getPauseGame() == false && spider_death == false){
			if(
				Physics.Raycast(
					spider_target_object.transform.position,
					spider_target_object.transform.TransformDirection(Vector3.forward),
					out spider_raycast_hit,
					1.4f
				)
			){
				if(spider_raycast_hit.collider.gameObject.CompareTag("Player")) {
					if(spider_attacking == false){
						spider_attacking = true;
						play_attack_animation();
						PlayAttackParticle();
						playAttackSound();
						StartCoroutine(resetAttackParticle());
						StartCoroutine(resetSpiderAttacking());
					}else{
						if(spider_and_witch_distance <= 1.4f){
							spider_attack_count = spider_attack_count + 1;
							if(spider_attack_count == 1){
								int damage_by_spider = Random.Range(1, 6);
								witch.takeDamage(damage_by_spider);
								StartCoroutine(resetSpiderAttackCount());
							}
						}
					}
				}
			}
			Debug.DrawRay(
				spider_target_object.transform.position,
				spider_target_object.transform.TransformDirection(Vector3.forward) * 1.4f,
				Color.white
			);
		}
	}

	void checkTarget() {
		if(witchgirltransform){
			spider_and_witch_distance = Vector3.Distance(witchgirltransform.position, gameObject.transform.position);
		}
	}

	void checkSpiderDeath() {
		if(spider_death == true){
			Destroy(gameObject, 4);
		}
	}

	void play_run_animation(){
		spider_animator.SetBool("run", true);
	}

	void stop_run_animation(){
		spider_animator.SetBool("run", false);
	}

	void play_attack_animation(){
		spider_animator.SetTrigger("attack");
	}

	void play_death_animation(){
		spider_animator.SetTrigger("death");
	}

	void playWalkSound(){
		spider_walk_sound.Play();
	}

	void stopWalkSound(){
		spider_walk_sound.Stop();
	}

	void playAttackSound(){
		spider_attack_sound.Play();
	}

	void playDeathSound(){
		spider_death_sound.Play();
	}

	void PlayAttackParticle(){
		spider_attack_particle.Play();
	}

	void StopAttackParticle(){
		spider_attack_particle.Stop();
	}

	public float getSpiderAndWitchDistance() {
		return spider_and_witch_distance;
	}

	void OnTriggerEnter(Collider other_collider) {
		if(other_collider.gameObject.CompareTag("AttackCollider")){
			spider_death = true;
			spider_hit_count = spider_hit_count + 1;
			if(spider_hit_count == 1){
				play_death_animation();
				playDeathSound();
				spider_spawn_manager.resize_spider_enenmy_count(1);
				spider_game_manager.updateScore();
			}
		}
		
		if(other_collider.gameObject.CompareTag("Enemy")){
			spider_near_spider = true;
			friendly_spider = other_collider.gameObject.GetComponent<spiderenemy>();
		}
	}

	void OnTriggerExit(Collider other_collider) {
		if(other_collider.gameObject.CompareTag("Enemy")){
			spider_near_spider = false;
			friendly_spider = null;
		}
	}

	IEnumerator resetSpiderAttacking() {
		yield return new WaitForSeconds(2);
		spider_attacking = false;
	}

	IEnumerator resetSpiderAttackCount() {
		yield return new WaitForSeconds(2);
		spider_attack_count = 0;
	}

	IEnumerator resetAttackParticle() {
		yield return new WaitForSeconds(1);
		StopAttackParticle();
	}
}
