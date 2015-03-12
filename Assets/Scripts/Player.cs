using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float speed = 3f, jumpSpeed = 400f;

	void Start () {
	
	}
	
	void Update () {
		movement();
	}

	void movement(){
		// left and right movement
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.transform.position += Vector3.right*speed*Time.deltaTime;
			if(Information.scrollable){
			Camera.main.transform.position += Vector3.right*speed*Time.deltaTime;
			}
		} else if(Input.GetKey(KeyCode.LeftArrow)){
			transform.transform.position += Vector3.left*speed*Time.deltaTime;
			if(Information.scrollable){
			Camera.main.transform.position += Vector3.left*speed*Time.deltaTime;
			}
		}
		// player wants to jump
		if(Input.GetKeyDown(KeyCode.Space)){
			//check to see if the player is on the ground
			//use a raycast

			//jump
			rigidbody2D.AddForce(Vector2.up*jumpSpeed*Information.jumpScale);
		}
	}
}
