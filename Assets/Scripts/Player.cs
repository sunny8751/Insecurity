using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public LayerMask layerMask;
		float speed = 3f, jumpSpeed = 400f, playerSize;

		void Start ()
		{
				playerSize = GetComponent<SpriteRenderer> ().sprite.bounds.size.x;
				layerMask = ~layerMask;
		}
	
		void Update ()
		{
				movement ();
		}

		void movement ()
		{
				// left and right movement
				if (Input.GetKey (KeyCode.RightArrow)) {
						transform.transform.position += Vector3.right * speed * Time.deltaTime;
						if (Information.scrollable) {
								Camera.main.transform.position += Vector3.right * speed * Time.deltaTime;
						}
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						transform.transform.position += Vector3.left * speed * Time.deltaTime;
						if (Information.scrollable) {
								Camera.main.transform.position += Vector3.left * speed * Time.deltaTime;
						}
				}
				//player goes down a platform
				if (Input.GetKey (KeyCode.DownArrow)) {
						bool done = false;
						RaycastHit2D hit = Physics2D.Raycast (transform.position - new Vector3 (-playerSize / 2, -playerSize, 0), -Vector2.up, 20, layerMask);
						if (hit.collider.gameObject.layer == 10) {
								float distance = Mathf.Abs (hit.point.y - transform.position.y);
								if (distance < .22f) {
										//let player go down
										hit.collider.isTrigger = true;
										done = true;
								}
						}
						//check other side
						if (!done) {
								hit = Physics2D.Raycast (transform.position - new Vector3 (playerSize / 2, -playerSize, 0), -Vector2.up, 20, layerMask);
								if (hit.collider.gameObject.layer == 10) {
										float distance = Mathf.Abs (hit.point.y - transform.position.y);
										if (distance < .22f) {
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
		}

		bool checkGrounded ()
		{
				RaycastHit2D hit = Physics2D.Raycast (transform.position - new Vector3 (-playerSize / 2, -playerSize, 0), -Vector2.up, 20, layerMask);
				if (hit.collider.tag == "Ground") {
						float distance = Mathf.Abs (hit.point.y - transform.position.y);
						if (distance < .22f) {
								//is grounded
								return true;
						}
				}
				hit = Physics2D.Raycast (transform.position - new Vector3 (+playerSize / 2, -playerSize, 0), -Vector2.up, 20, layerMask);
				if (hit.collider.tag == "Ground") {
						float distance = Mathf.Abs (hit.point.y - transform.position.y);
						if (distance < .22f) {
								//is grounded
								return true;
						}	
				}
				return false;
		}
}
