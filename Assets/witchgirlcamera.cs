using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class witchgirlcamera : MonoBehaviour {

	GameObject witchgirlobject;
    GameObject witchgirlcameraobject;
	float mousehorizontal;
	float mousevertical;
	Vector3 mousedirection;
	Vector3 smoothmousedirection;
	Vector3 smoothmousevelocity;
	float smoothmousetime=0.2f;
	float smoothmousespeed=Mathf.Infinity;
	float minimummousevertical=-12f;
	float maximummousevertical=4f;
	float witchgirlandcameradistance=-1f;
	float reversemouse=-1f;
	float mousespeed=2f;
	gamemanager camera_game_manager;

	// Use this for initialization
	void Start () {
		// witchgirlobject = GameObject.Find("witchgirl");
		// witchgirlandcameradistance = gameObject.transform.position-witchgirlobject.transform.position;
        witchgirlcameraobject = GameObject.Find("witchgirlcamera");
		camera_game_manager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
	}
	
	// Update is called once per frame
	void Update () {
		movecamera();
	}

	void movecamera(){
		mousehorizontal = mousehorizontal+Input.GetAxis("Mouse X");
		mousevertical = mousevertical+Input.GetAxis("Mouse Y");
		mousevertical = Mathf.Clamp(mousevertical,minimummousevertical,maximummousevertical);
		mousedirection = new Vector3(mousevertical*reversemouse,mousehorizontal,0);
		smoothmousedirection = Vector3.SmoothDamp(mousedirection,smoothmousedirection,ref smoothmousevelocity,smoothmousetime,smoothmousespeed,Time.deltaTime);
		if(camera_game_manager.getPauseGame() == false){
			gameObject.transform.eulerAngles = smoothmousedirection*mousespeed;
			gameObject.transform.position = witchgirlcameraobject.transform.position+transform.forward*witchgirlandcameradistance;
		}
	}
}
