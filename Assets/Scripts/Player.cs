using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public LayerMask layerMask;
	public Texture2D textbox;
		float speed = 3f, jumpSpeed = 400f, playerSizeX, playerSizeY, cameraExtent;
		string command = "", text= "";
	bool pause = true;
	int storyIndex = -1;
	Vector3 scale;
	float width = 1061, height = 597;
	GUIStyle style;

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				playerSizeX = GetComponent<SpriteRenderer> ().sprite.bounds.size.x * transform.localScale.x;
				playerSizeY = GetComponent<SpriteRenderer> ().sprite.bounds.size.y * transform.localScale.y;
				layerMask = ~layerMask;
		scale = new Vector3(Screen.width / width, Screen.height / height,1);
		style = new GUIStyle ();
		changeLevel(Application.loadedLevel);
		Debug.Log("crying");
		StartCoroutine("Wait", .5f);
		}

	IEnumerator Wait(float time){
		yield return new WaitForSeconds(time);
		next();
	}

	public void next(){
		storyIndex++;
		text = story[storyIndex];
		if(text=="italics"){
			style.fontStyle = FontStyle.Italic;
		}else if(text=="normal"){
			style.fontStyle = FontStyle.Normal;
		}
		if(text==""){
			pause = false;
		} else if(text=="action2"){
			//2 action text

			}else{
			pause = true;
		}
	}

		void changeLevel (int level)
		{
				if (level == 0) {
						cameraExtent = 6.15f;
		} else if (level == 1) {
			cameraExtent = 16.25f;
						transform.position = new Vector3 (-19.6f, -1.8f, 0);
		} else if (level == 2) {
						cameraExtent = 6.2f;
			transform.position = new Vector3 (-11.2f, -1.5f, 0);
		} else if (level == 3) {
			cameraExtent = 9.83f;
						transform.position = new Vector3 (-12.2f, -2.8f, 0);
				}

		Camera.main.transform.position = new Vector3 (-cameraExtent, 0, -10);
		}

		void Update ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			Application.LoadLevel (Application.loadedLevel + 1);
			changeLevel (Application.loadedLevel + 1);
			command = "";
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit ();
		}
				if(pause){
			//paused
			//dialogue
			if(Input.GetKeyDown(KeyCode.Space)){
				next();
			}
		}else{
			//not paused
				movement ();
				Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, -cameraExtent, cameraExtent), 0, -10);
		}
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
										hit.collider.enabled = false;
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
						rigidbody2D.AddForce (Vector2.up * jumpSpeed);
				}
				//commands
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
						if (command == "open door") {
								Application.LoadLevel (Application.loadedLevel + 1);
				changeLevel (Application.loadedLevel + 1);
								command = "";
						}
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
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Puddle") {
			//stepped in puddle
			Debug.Log("dead");
		}
	}

		void OnGUI ()
	{
		var svMat = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);
		//gui begins
				style.fontSize = 40;
				style.alignment = TextAnchor.MiddleCenter;
				if (command != "") {
						GUI.Label (new Rect (0, height * .4f, width, 200), "Press UP to " + command, style);
				}
		if(pause){
		if(text!=""){
				//has text
				style.fontSize = 20;
				style.alignment = TextAnchor.MiddleLeft;
				GUI.DrawTexture(new Rect(0,8,width, height/5), textbox);
				GUI.Label(new Rect(30,10,width, 80), "Me: "+text, style);
				style.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect(0,height/5-40,width, 40), "SPACE to continue", style);
			}
		}
		//end of gui
		GUI.matrix = svMat;
		}
	string [] story = {"What's that sound? My brother crying because he lost his toy? It's too loud for me to sleep.", "Maybe I should find his toy for him.","I should try looking under these clothes. \n      Maybe if I jumped on them multiple times I could see if they are hiding the toy.",
		"", "I've found the toy! Now what should I do with it?", "italics", "The toy that your brother lost, a stuffed tiger, is your favorite, even though it doesn’t belong to you. What do you do?","normal", "action2", "Hide the toy behind your back and try to walk out of the room. The toy is now yours!!!\nYou should really return it, his crying is getting really loud, and your mom is trying to sleep."};
}
