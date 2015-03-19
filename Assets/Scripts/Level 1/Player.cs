using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public LayerMask layerMask;
		float speed = 3f, jumpSpeed = 400f, playerSizeX, playerSizeY, cameraExtent;
		string command = "";

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				playerSizeX = GetComponent<SpriteRenderer> ().sprite.bounds.size.x * transform.localScale.x;
				playerSizeY = GetComponent<SpriteRenderer> ().sprite.bounds.size.y * transform.localScale.y;
				layerMask = ~layerMask;
		changeLevel(Application.loadedLevel);
		}

		void changeLevel (int level)
		{
				if (level == 0) {
						cameraExtent = 6.15f;
		} else if (level == 1) {
						cameraExtent = 9.1f;
						transform.position = new Vector3 (-13.5f, -3.1f, 0);
		} else if (level == 2) {
						cameraExtent = 5.15f;
			transform.position = new Vector3 (-11.3f, -3.4f, 0);
		} else if (level == 3) {
			cameraExtent = 9.83f;
						transform.position = new Vector3 (-12.2f, -2.8f, 0);
				}

		Camera.main.transform.position = new Vector3 (-cameraExtent, 0, -10);
		}

		void Update ()
		{
				movement ();
				Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, -cameraExtent, cameraExtent), 0, -10);
		}

		void movement ()
		{
				// left and right movement
				if (Input.GetKey (KeyCode.RightArrow)) {
						//change position of player
						transform.position += Vector3.right * speed * Time.deltaTime;
						//change direction of player
						transform.localRotation = Quaternion.Euler (0, 180, 0);
						if (!(Camera.main.WorldToScreenPoint (transform.position).x < Screen.width / 2)) {
								Camera.main.transform.position += Vector3.right * speed * Time.deltaTime;
						}
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						//change position of player
						transform.position += Vector3.left * speed * Time.deltaTime;
						//change direction of player
						transform.localRotation = Quaternion.Euler (0, 0, 0);
						if (!(Camera.main.WorldToScreenPoint (transform.position).x > Screen.width / 2)) {
								Camera.main.transform.position += Vector3.left * speed * Time.deltaTime;
						}
				}
				//player goes down a platform
				if (Input.GetKey (KeyCode.DownArrow)) {
						bool done = false;
						RaycastHit2D hit = Physics2D.Raycast (transform.position + new Vector3 (-playerSizeX / 2, 0, 0), -Vector2.up, 20, layerMask);
						if (hit.collider.gameObject.layer == 10) {
								float distance = Mathf.Abs (hit.point.y - transform.position.y);
								if (distance < .22f + playerSizeY / 2) {
										//let player go down
										hit.collider.isTrigger = true;
										done = true;
								}
						}
						//check other side
						if (!done) {
								hit = Physics2D.Raycast (transform.position + new Vector3 (playerSizeX / 2, 0, 0), -Vector2.up, 20, layerMask);
								if (hit.collider.gameObject.layer == 10) {
										float distance = Mathf.Abs (hit.point.y - transform.position.y);
										if (distance < .22f + playerSizeY / 2) {
												//let player go down
												hit.collider.isTrigger = true;
										}
								}
						}
				}
				// player wants to jump
				if (Input.GetKeyDown (KeyCode.Space) && checkGrounded ()) {
						//check to see if the player is on the ground, then jump
						rigidbody2D.velocity = Vector2.zero;
						rigidbody2D.AddForce (Vector2.up * jumpSpeed * Information.jumpScale);
				}
				//commands
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
						if (command == "open door") {
								Application.LoadLevel (Application.loadedLevel + 1);
				changeLevel (Application.loadedLevel + 1);
								command = "";
						}
				}
				if (Input.GetKeyDown (KeyCode.A)) {
						Application.LoadLevel (Application.loadedLevel + 1);
			changeLevel (Application.loadedLevel + 1);
						command = "";
				}
				if (Input.GetKeyDown (KeyCode.Q)) {
						Application.Quit ();
				}
		}

		bool checkGrounded ()
		{
				RaycastHit2D hit = Physics2D.Raycast (transform.position + new Vector3 (-playerSizeX / 2, 0, 0), -Vector2.up, 20, layerMask);
				if (hit.collider.tag == "Ground") {
						float distance = Mathf.Abs (hit.point.y - transform.position.y);
						if (distance < .22f + playerSizeY / 2) {
								//is grounded
								return true;
						}
				}
				hit = Physics2D.Raycast (transform.position + new Vector3 (playerSizeX / 2, 0, 0), -Vector2.up, 20, layerMask);
				if (hit.collider.tag == "Ground") {
						float distance = Mathf.Abs (hit.point.y - transform.position.y);
						if (distance < .22f + playerSizeY / 2) {
								//is grounded
								return true;
						}
				}
				return false;
		}
	
		void OnTriggerStay2D (Collider2D other)
		{
				//if in front of door
				if (other.tag == "Door") {
						command = "open door";
				}
		}

		void OnTriggerExit2D (Collider2D other)
		{
				//if in front of door
				if (other.tag == "Door") {
						command = "";
				}
		}

		void OnGUI ()
		{
				GUIStyle style = new GUIStyle ();
				style.fontSize = 40;
				style.alignment = TextAnchor.MiddleCenter;
				if (command != "") {
						GUI.Label (new Rect (0, Screen.height * .4f, Screen.width, 200), "Press UP to " + command, style);
				}
		}
}
