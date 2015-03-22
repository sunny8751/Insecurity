using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public LayerMask layerMask;
		public Texture2D textbox;
		public Sprite[] sprites;
		float speed = 3f, jumpSpeed = 400f, playerSizeX, playerSizeY, cameraExtent;
		string command = "", text = "";
		bool pause = true, prompting = false, cutscene = false;
		int action = -1;
		IList actionsText, decisions;
		int storyIndex = -1, personality = 3;
		Vector3 scale;
		float width = 1061, height = 597;
		GUIStyle style;
		Animator anim;

		void Start ()
		{
				DontDestroyOnLoad (gameObject);
				anim = transform.GetComponent<Animator> ();
				actionsText = new ArrayList ();
				decisions = new ArrayList ();
				playerSizeX = GetComponent<SpriteRenderer> ().sprite.bounds.size.x * transform.localScale.x;
				playerSizeY = GetComponent<SpriteRenderer> ().sprite.bounds.size.y * transform.localScale.y;
				layerMask = ~layerMask;
				scale = new Vector3 (Screen.width / width, Screen.height / height, 1);
				style = new GUIStyle ();
				//decisions.Add(action);
				changeLevel (Application.loadedLevel);
				Debug.Log ("crying");
				StartCoroutine ("Wait", .5f);
		}

		IEnumerator Wait (float time)
		{
				cutscene = true;
				yield return new WaitForSeconds (time);
				cutscene = false;
				next ();
		}

		public void next ()
		{
				storyIndex++;
				text = story [storyIndex];
				if (text == "italics") {
						style.fontStyle = FontStyle.Italic;
						storyIndex++;
						text = story [storyIndex];
				} else if (text == "normal") {
						style.fontStyle = FontStyle.Normal;
						if (storyIndex == 57) {
								//offered to help him- good
								storyIndex = 62;
								text = story [storyIndex];
						} else {
								storyIndex++;
								text = story [storyIndex];
						}
				}
				if (text == "") {
						pause = false;
				} else if (text.Substring (1) == "action") {
						//action text
						action = 0;
						int numb = int.Parse (text [0].ToString ());
						for (int i =0; i< numb; i++) {
								storyIndex++;
								actionsText.Add (story [storyIndex]);
						}
				} else {
						pause = true;
				}
				if (storyIndex == 16) {
						GameObject.FindWithTag ("Mom").GetComponent<SpriteRenderer> ().enabled = true;
				}
		}
	
		void changeAppearance (int i)
		{
				personality += i;
		anim.SetInteger ("Character", personality);
		//create poof
		Instantiate((GameObject) Resources.Load("Poof"), transform.position, Quaternion.identity);
				gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [personality];
				playerSizeX = gameObject.GetComponent<SpriteRenderer> ().sprite.bounds.size.x * transform.localScale.x;
				playerSizeY = gameObject.GetComponent<SpriteRenderer> ().sprite.bounds.size.y * transform.localScale.y;
				float dy = .3f;
				if (personality == 3) {
						dy = 0;
				}
				transform.position = new Vector3 (transform.position.x, transform.position.y + dy, 0);
				Destroy (GetComponent<BoxCollider2D> ());
				gameObject.AddComponent<BoxCollider2D> ();
		}

		void doActions ()
		{
				decisions.Add (action);
				if (decisions.Count == 1) {
						//first decision
						if ((int)decisions [0] == 0) {
								//hide toy and try to leave
								//hide the toy
								GameObject.FindWithTag ("Toy").GetComponent<SpriteRenderer> ().enabled = false;
								GameObject.FindWithTag ("Brother").GetComponent<BoxCollider2D> ().enabled = true;
								changeAppearance (-1);
								storyIndex++;
								text = story [storyIndex];
								command = "mom";
						} else if ((int)decisions [0] == 1) {
								//give toy back to brother
								GameObject.FindWithTag ("Toy").GetComponent<SpriteRenderer> ().enabled = false;
								GameObject.FindWithTag ("Brother").GetComponent<BoxCollider2D> ().enabled = true;
								storyIndex += 2;
								text = story [storyIndex];
								command = "brother";
						}
						prompting = true;
						pause = false;
						action = -1;
				} else if (decisions.Count == 2) {
						if ((int)decisions [1] == 0) {
								//carry cat to safety
								GameObject.FindWithTag ("Cat").GetComponent<SpriteRenderer> ().enabled = false;
								GameObject.FindWithTag ("Cat").GetComponent<BoxCollider2D> ().enabled = false;
								storyIndex = 29;
								text = story [storyIndex];
						} else if ((int)decisions [1] == 1) {
								//leave cat
								GameObject.FindWithTag ("Cat").GetComponent<BoxCollider2D> ().enabled = false;
								storyIndex = 30;
								text = story [storyIndex];
								//worse
								changeAppearance (-1);
						} else if ((int)decisions [1] == 2) {
								GameObject.FindWithTag ("Cat").GetComponent<SpriteRenderer> ().enabled = false;
								GameObject.FindWithTag ("Cat").GetComponent<BoxCollider2D> ().enabled = false;
								//keep cat
								storyIndex = 31;
								text = story [storyIndex];
						}
						action = -1;
				} else if (decisions.Count == 3) {
						if ((int)decisions [2] == 0) {
								//give hw to him
								changeAppearance (-1);
								storyIndex = 45;
								text = story [storyIndex];
						} else if ((int)decisions [2] == 1) {
								//write wrong answers
								changeAppearance (-1);
								storyIndex = 47;
								text = story [storyIndex];
						} else if ((int)decisions [2] == 2) {
								//will help him, but he cant copy
								changeAppearance (1);
								storyIndex = 53;
								text = story [storyIndex];
						}
						action = -1;
				} else if (decisions.Count == 4) {
						if ((int)decisions [3] == 0) {
								//ignore bully
								changeAppearance (-1);
								storyIndex = 79;
								next ();
						} else if ((int)decisions [3] == 1) {
								//give bully a hug
								changeAppearance (1);
								storyIndex = 85;
								text = story [storyIndex];
						} else if ((int)decisions [3] == 2) {
								//make fun of bully
								changeAppearance (-1);
								storyIndex = 90;
								text = story [storyIndex];
						}
						action = -1;
				} else if (decisions.Count == 5) {
						action = -1;
						next ();
				}
		}

		void Update ()
		{
				if (Application.loadedLevel == 3) {
						if (storyIndex == 69 && (int)decisions [1] != 2) {
								GameObject.FindWithTag ("Cat").GetComponent<SpriteRenderer> ().enabled = true;
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
				if (pause) {
						//paused
						//dialogue
						if (Input.GetKeyDown (KeyCode.Space) && !cutscene) {
								if (action == -1) {
										//just dialogue
										if (storyIndex == 14) {
												//gave back toy, so good
												changeAppearance (1);
												storyIndex = 15;
												text = story [storyIndex];
										} else if (storyIndex == 17) {
												//get caught by mom
												storyIndex = 12;
												text = story [storyIndex];
												pause = false;
												prompting = true;
												command = "brother1";
										} else if (storyIndex == 18) {
												//gave toy back to bro after mom told you to
												storyIndex = 15;
												text = story [storyIndex];
												//gave back toy, so g1
												changeAppearance (1);
										} else if (storyIndex == 19) {
												//mom told you to go to school
												storyIndex = 20;
												text = story [storyIndex];
												pause = false;
												prompting = true;
												command = "";
										} else if (storyIndex == 15 && (int)decisions [0] == 0) {
												storyIndex = 19;
												text = story [storyIndex];
										} else if (storyIndex == 16 && (int)decisions [0] == 1) {
												//mom just came in
												GameObject.FindWithTag ("Mom").GetComponent<SpriteRenderer> ().enabled = true;
												storyIndex = 19;
												text = story [storyIndex];
										} else if (storyIndex == 29) {
												//get cat to other side
												storyIndex = 33;
												text = story [storyIndex];
												pause = false;
												prompting = true;
										} else if (storyIndex == 30 || storyIndex == 31 || storyIndex == 34) {
												storyIndex = 32;
												text = story [storyIndex];
												pause = false;
												prompting = true;
										} else if (storyIndex == 46) {
												//gave him the hw- bad
												storyIndex = 62;
												text = story [storyIndex];
										} else if (storyIndex == 52) {
												//gave him the wrong answers
												storyIndex = 62;
												text = story [storyIndex];
										} else if (storyIndex == 59) {
												transform.position = new Vector3 (-19.6f, -1.8f, 0);
												Camera.main.transform.position = new Vector3 (-cameraExtent, 0, -10);
												storyIndex = 60;
												text = story [storyIndex];
										} else if (storyIndex == 63) {
												storyIndex = 64;
												text = story [storyIndex];
												prompting = true;
												pause = false;
										} else if (storyIndex == 66) {
												storyIndex = 67;
												text = story [storyIndex];
												prompting = true;
												pause = false;
										} else if (storyIndex == 70) {
												cutscene = true;
												text = "";
												StartCoroutine (Bully (.3f, -15.5f));
										} else if (storyIndex == 84 || storyIndex == 89 || storyIndex == 91) {
												storyIndex = 92;
												text = story [storyIndex];
										} else if (storyIndex == 104) {
												if ((int)decisions [1] == 2) {
														//if has a cat inside
														storyIndex = 105;
												} else {
														storyIndex = 111;
												}
												text = story [storyIndex];
										} else if (storyIndex == 111) {
												//cat got someone's attention
												//cutscene where adults walk in
												cutscene = true;
												StartCoroutine ("Adults");
										} else if (storyIndex == 114) {
												//go to mom
												storyIndex = 115;
												text = story [storyIndex];
												prompting = true;
												pause = false;
										} else if (storyIndex == 118) {
												//adults come and they go home
												cutscene = true;
												StartCoroutine ("Home");
										} else {
												next ();
										}
								} else {
										//choose the action
										actionsText.Clear ();
										doActions ();
								}
						}
						//switch between actions
						if (action != -1) {
								if (Input.GetKeyDown (KeyCode.DownArrow)) {
										action++;
										action = action % actionsText.Count;
								} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
										action--;
										if (action == -1) {
												action = actionsText.Count - 1;
										}
								}
						}
				} else {
						//not paused
						movement ();
						Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, -cameraExtent, cameraExtent), 0, -10);
				}
		}

		void OnGUI ()
		{
				var svMat = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);
				//gui begins
				style.fontSize = 20;
				style.alignment = TextAnchor.MiddleLeft;
				if (text != "") {
						//has text
						GUI.DrawTexture (new Rect (0, 8, width, height / 5), textbox);
						if (action != -1) {
								for (int i=0; i<actionsText.Count; i++) {
										if (i == action) {
												style.fontStyle = FontStyle.Bold;
										} else {
												style.fontStyle = FontStyle.Normal;
										}
										GUI.Label (new Rect (30, -5 + i * 25, width, 80), (string)actionsText [i], style);
								}
								style.fontStyle = FontStyle.Normal;
								style.alignment = TextAnchor.MiddleCenter;
								GUI.Label (new Rect (0, height / 5 - 30, width, 40), "UP or DOWN to choose switch. SPACE to choose action.", style);
						} else {
								//dialogue/prompts only
								GUI.Label (new Rect (30, 5, width, 80), text, style);
								if (!prompting) {
										FontStyle beforeStyle = style.fontStyle;
										style.fontStyle = FontStyle.Normal;
										style.alignment = TextAnchor.MiddleCenter;
										GUI.Label (new Rect (0, height / 5 - 30, width, 40), "SPACE to continue", style);
										style.fontStyle = beforeStyle;				
								}
						}
				}
				//end of gui
				GUI.matrix = svMat;
		}

		string[] story = {
				"Me: What's that sound? My brother crying because he lost his toy? It's too loud for me to sleep.",
				"Me: Maybe I should find his toy for him.",
				"Me: I should try looking under these clothes. \n      Maybe if I jumped on them multiple times I could see if they are hiding the toy.",
				"",
				"Me: I've found the toy! Now what should I do with it?",
				"italics",
				"(The toy that your brother lost, a stuffed bear, is your favorite,even though it doesn’t belong to you. What do you\ndo?)",
				"normal",
				"2action",
				"1: Hide the toy behind your back and try to walk out of the room. The toy is now yours!!! (Choose this one)",
				"2: You should really return it, his crying is getting really loud, and your mom is trying to sleep.",
				"Walk towards the door...",
				"Give toy back to your brother...", //12
				"Press UP to talk to your brother", //13
				"Brother: Thanks big bro! You're so nice. I thought I lost this for a second.",//14
				"Brother: Whoa! You look nicer all of a sudden! Maybe it's because you decided to give me back my toy.",//15
				"Mom: Oh no! I forgot to set an alarm yesterday. Thank goodness your brother woke me up in time for me to send\nyou to school!",//16
				"Mom: Hey... isn't that your brother's toy? Give that back right now!",
				"Brother: Thanks for giving me that back! That was really nice.",//18
				"Mom: OMG! You're late for school! Hurry to class!",
				"Exit through the door...",//20
				"Press UP to enter door",//21 and end of level 1
		"It looks like it rained a lot yesterday! I better jump over those puddles.",//22
		"",
		"What is this cat doing here? Hmm...",
		"3action",//25
		"Pick up the cat and carry it to safety",//26
		"Leave the cat here... it was stupid to get trapped by puddles",
		"Put the cat in your bookbag... It could be useful later",//28
		"Ok. Let's get to the other side now.",//29
		"Stupid cat... You can stay here; I won't help you!",
		"Maybe this cat will help me later...",
		"Get to school...",//32
		"Get the cat to the other side!",
		"Ok, let's go to school now.",
		"Press UP to enter door",//35 and end of level 2
		"Bully: Hey Peg-Leg! You're late! The teacher is making you stay inside during recess. HAHA!\n      Why didn't you come earlier? It was your prosthetic leg, wasn't it?",
		"Bully: Anyways, I need the homework.",//37
		"italics",
		"You don’t want to give it to him because he is a jerk. He’s also not smart and the teacher will find out easily.",
		"normal",//40
		"3action",
		"Give it to him.",
		"Tell him you'll make him a copy of your homework, and then write down completely wrong answers.",
		"Tell him that you'll help him but not let him have yours",//44
		"Me: Fine. Here it is...",
		"Bully: Haha! See you later, Peg-Leg",
		"Me: Fine. Let me copy down my answers on another sheet of paper for you.",//47
		"italics",
		"(Writing down wrong answers)",
		"normal",//50
		"Me: Here you go! I'm pretty sure all the answers are correct.",
		"Bully: Haha! Good thing you're smart and clever, isn't it, Peg-Leg?",
		"Me: No way! I am NOT giving you my homework. I'm willing to help you if you want though.",//53
		"Bully: I don't want help! I just want your answers.",
		"italics",
		"(Bully snatches homework away)",//56
		"normal",//57
		"",//58
		"Me: Oh no! I stepped in the puddle and got my shoes ruined... Let me go back and change them.",//from level 2- the puddle
		"Me: Ok! I should be more careful this time.",//60
		"",
		"Me: ... ",
		"Me: Ok. I should probably sit down at my desk before the teacher comes in.",//63
		"Sit down at your desk...",
		"Press UP to sit down and start school...", //65
		"Me: Oh no! The talk I had with my teacher after school today because I was late this morning took a long time.\nIt's already dark outside. I should go back home now.",
		"Exit through the door...",
		"Press UP to exit...",
		"Me: Its too dark outside. I think I'm lost.",//69 and beginning of level 4
		"Me: Wait... I hear something. Who's there?",
		"Bully: It's me... D-d-d-do you know where we are?",
		"Me: Are you lost too?",//72
		"Bully: N-n-n-no. I know where I'm going... It's just that the darkness is getting in my way!",
		"Me: So you are lost. And are you shivering? Are you scared of the dark?",//74
		"Bully: W-w-w-what?! NO I’M FINE- shut it Peg-Leg! You’re the one who’s lost, so hurry up and find a way home.",
		"3action",//76
		"Ignore him and stare into the trees",
		"Comfort the bully; hugs make everything better",
		"Make fun of the bully",
		"italics",//80
		"(Ignores the bully and searches for a way out past the trees)",
		"normal",
		"Bully: Hey! Hey? HEY! Are you even paying attention to me?",
		"Bully: Ignoring me like that was really rude. Now hurry up and find a way out of here.",//84
		"Me: Hey... everything is going to be alright. Ok?",//85
		"italics",
		"(Gives the bully a hug)",
		"normal",
		"Me: Now let's get out of here.",//89
		"Me: Hah! You stutter when you're scared! If only the others at school knew about THIS!",//90
		"Bully: STOP! Quit being annoying and hurry up and find a way out of here",
		"Me: Ok, fine. Let's go.",//92
		"",
		"Me: Aghh!",//94
		"Bully: What happened? Are you ok?",
		"Me: No. I fell in this puddle and broke my prosthetic leg. I don't think I can move anywhere... Stupid leg!",
		"Bully: What's wrong with it?",
		"Me: Every one always makes fun of me! I’m no pirate! It’s not like I wanted a peg leg! I can’t play like everyone\nelse!",
		"Bully: I’m… Sorry. I actually thought it was really cool, but I never had the courage to tell you. I mean, it is like you\nhave a piece of iron man on you. Guess I was pretty jealous.",
		"Me: ... … ...Thanks. That’s the first time someone was nice about my leg.",
		"Unknown adult: HELLOOOO! ANYONE THERE??",
		"Bully: YES WE ARE HERE! HELP!",
		"...",
		"Me: I don't think they can hear us.",//104
		"Me: Oh wait! I know! I have a cat inside my bookbag.",//105
		"1action",
		"Take cat out of bookbag",
		"italics",
		"Takes out the cat and puts the cat on the ground",
		"normal",
		"Cat: MEEEEEOOOOOOWWWWWWWWWWW! MEEEEOOOOWWW! MEEEOOWW!",//111
		"",
		"Mom: Oh, thank goodness you guys are there! I was worried that you guys were lost!",
		"Mom: It's lucky that you two were together because if you guys weren't, I might have not heard you. Come here\nand I'll take you home.",//114
		"Go to mom...",
		"Press UP",
		"Bully: Wait... Hey, before you go, I just wanted to tell you that I'm sorry for everything I did. Can we start over?",//117
		"Me: Yeah, thanks for apologizing. That means a lot to me. I'll see you at school tomorrow!"
	};
		//_______________________________________________________________________________
	
		void changeLevel (int level)
		{
				if (level == 0) {
						cameraExtent = 6.15f;
				} else if (level == 1) {
						storyIndex = 22;
						text = story [storyIndex];
						pause = true;
						cameraExtent = 16.25f;
						transform.position = new Vector3 (-19.6f, -1.8f, 0);
				} else if (level == 2) {
						//classroom
						storyIndex = 36;
						text = story [storyIndex];
						pause = true;
						prompting = false;
						cameraExtent = 6.2f;
						transform.position = new Vector3 (-11.2f, -3.5f, 0);
				} else if (level == 3) {
						storyIndex = 69;
						text = story [storyIndex];
						pause = true;
						prompting = false;
						cameraExtent = 9.83f;
						transform.localRotation = Quaternion.Euler (0, 180, 0);
						transform.position = new Vector3 (-17.4f, -2.8f, 0);
				}
				Camera.main.transform.position = new Vector3 (-cameraExtent, 0, -10);
		}
	
		void movement ()
		{
				// left and right movement
				if (Input.GetKey (KeyCode.RightArrow)) {
						anim.SetBool ("Walking", true);
						//change position of player
						transform.position += Vector3.right * speed * Time.deltaTime;
						//change direction of player
						transform.localRotation = Quaternion.Euler (0, 180, 0);
						if (!(Camera.main.WorldToScreenPoint (transform.position).x < Screen.width / 2)) {
								Camera.main.transform.position += Vector3.right * speed * Time.deltaTime;
						}
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
						anim.SetBool ("Walking", true);
						//change position of player
						transform.position += Vector3.left * speed * Time.deltaTime;
						//change direction of player
						transform.localRotation = Quaternion.Euler (0, 0, 0);
						if (!(Camera.main.WorldToScreenPoint (transform.position).x > Screen.width / 2)) {
								Camera.main.transform.position += Vector3.left * speed * Time.deltaTime;
						}
				} else {
						//not either left or right
						//not moving
						anim.SetBool ("Walking", false);
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
						if (command == "brother") {
								//talk to brother
								prompting = false;
								GameObject.FindWithTag ("Brother").GetComponent<BoxCollider2D> ().enabled = false;
								//brother happy DEBUG
								GameObject.FindWithTag ("Toy").transform.position = new Vector3 (4.64f, -4, 0);
								GameObject.FindWithTag ("Toy").GetComponent<SpriteRenderer> ().enabled = true;
								next ();
						} else if (command == "brother1") {
								// give toy back to brother cuz mom told you to
								storyIndex = 18;
								text = story [storyIndex];
								GameObject.FindWithTag ("Brother").GetComponent<BoxCollider2D> ().enabled = false;
								//brother happy DEBUG
								GameObject.FindWithTag ("Toy").transform.position = new Vector3 (4.64f, -4, 0);
								GameObject.FindWithTag ("Toy").GetComponent<SpriteRenderer> ().enabled = true;
								pause = true;
								prompting = false;
						} else if (storyIndex == 21 || storyIndex == 35) {
								Application.LoadLevel (Application.loadedLevel + 1);
								changeLevel (Application.loadedLevel + 1);
								command = "";
						} else if (storyIndex == 65) {
								//sit down at the desk and start school
								pause = true;
								StartCoroutine ("School");
						} else if (storyIndex == 68) {
								//leave school
								Application.LoadLevel (Application.loadedLevel + 1);
								changeLevel (Application.loadedLevel + 1);
						} else if (storyIndex == 116) {
								//talk to mom
								prompting = false;
								text = "";
								cutscene = true;
								pause = true;
								StartCoroutine (Bully (-5, 2));
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
						if (storyIndex == 20) {
								storyIndex = 21;
						} else if (storyIndex == 32 || storyIndex == 61) {
								storyIndex = 35;
						} else if (storyIndex == 67) {
								storyIndex = 68;
						}
						text = story [storyIndex];
				} else if (other.tag == "Brother") {
						if ((int)decisions [0] == 1 || (command == "brother1")) {
								//prompt to talk to brother
								storyIndex = 13;
								text = story [storyIndex];
						}
				}
		}
	
		void OnTriggerExit2D (Collider2D other)
		{
				//if in front of door
				if (other.tag == "Door") {
						if (storyIndex == 20) {
								command = "";
						} else if (storyIndex == 21) {
								storyIndex = 20;
						} else if (storyIndex == 35) {
								//level 2
								storyIndex = 32;
						} else if (storyIndex == 68) {
								storyIndex = 67;
						}
						text = story [storyIndex];
				} else if (other.tag == "Brother") {
						if (((int)decisions [0] == 1 && storyIndex == 13) || command == "brother1") {
								//prompt to go to brother
								if (storyIndex != 11) {
										storyIndex = 12;
										text = story [storyIndex];
								}
						}
				} else if (other.tag == "Desk" && storyIndex == 65) {
						//sit down at desk
						storyIndex = 64;
						text = story [storyIndex];
				} else if (other.tag == "Mom" && storyIndex == 116) {
						storyIndex = 115;
						text = story [storyIndex];
				}
		}

		IEnumerator School ()
		{
				//make classroom dark and to signal time passing
				cutscene = true;
				SpriteRenderer classroomB = GameObject.FindWithTag ("Background").GetComponent<SpriteRenderer> ();
				text = "";
				float time = 0;
				while (time<1f) {
						float value = Mathf.Lerp (1, 0, time / 1);
						classroomB.color = new Color (value, value, value, 1);
						time += Time.deltaTime;
						yield return new WaitForEndOfFrame ();
				}
				GameObject.FindWithTag ("Bully").SetActive (false);
				yield return new WaitForSeconds (1.5f);
				time = 0;
				while (time<1f) {
						float value = Mathf.Lerp (0, 1, time / 1);
						classroomB.color = new Color (value, value, value, 1);
						time += Time.deltaTime;
						yield return new WaitForEndOfFrame ();
				}
				storyIndex = 66;
				text = story [storyIndex];
				cutscene = false;
		}
	
		IEnumerator Bully (float startPos, float endPos)
		{
				float time = 0;
				Transform bullyT = GameObject.FindWithTag ("Bully").transform;
				if (storyIndex == 116) {
						GameObject.FindWithTag ("Bully").transform.localRotation = Quaternion.Euler (new Vector3 (0, 180, 0));
						transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));
						text = "";
				}
				while (time<6f) {
						float pos = Mathf.Lerp (startPos, endPos, time / 6f);
						bullyT.position = new Vector3 (pos, -2.59f, 0);
						time += Time.deltaTime;
						yield return new WaitForEndOfFrame ();
				}
				if (storyIndex == 116) {
						next ();
				} else {
						//bully done approaching you
						storyIndex = 71;
						text = story [storyIndex];
				}
				cutscene = false;
		}

		IEnumerator Adults ()
		{
				text = "";
				float time = 0;
				float totalTime = 1f;
				Transform cat = GameObject.FindWithTag ("Cat").transform;
				while (time<totalTime) {
						float pos = Mathf.Lerp (-7.5f, 0, time / totalTime);
						cat.position = new Vector3 (pos, -3.27f, 0);
						time += Time.deltaTime;
						yield return new WaitForEndOfFrame ();
				}
				cat.gameObject.SetActive (false);
				storyIndex = 113;
				text = story [storyIndex];
				cutscene = false;
		}

		IEnumerator Home ()
		{
				//make scene dark
				SpriteRenderer forestB = GameObject.FindWithTag ("Background").GetComponent<SpriteRenderer> ();
				SpriteRenderer playerB = GameObject.FindWithTag ("Player").GetComponent<SpriteRenderer> ();
				SpriteRenderer momB = GameObject.FindWithTag ("Mom").GetComponent<SpriteRenderer> ();
				SpriteRenderer bullyB = GameObject.FindWithTag ("Bully").GetComponent<SpriteRenderer> ();
				text = "";
				float time = 0;
				while (time<3f) {
						float value = Mathf.Lerp (1, 0, time / 3);
						forestB.color = new Color (value, value, value, 1);
						playerB.color = new Color (value, value, value, 1);
						momB.color = new Color (value, value, value, 1);
						bullyB.color = new Color (value, value, value, 1);
						time += Time.deltaTime;
						yield return new WaitForEndOfFrame ();
				}
				Destroy (gameObject);
				Application.LoadLevel (Application.loadedLevel + 1);
		}
	
		void OnTriggerEnter2D (Collider2D other)
		{
				if (other.tag == "Mom" && storyIndex == 115) {
						storyIndex = 116;
						text = story [storyIndex];
				} else 
				if (other.tag == "CatCheck") {
						//level 2
						//do whatever with the cat and leave
						if ((int)decisions [1] == 0 && (storyIndex == 33 || storyIndex == 61)) {
								//leave cat safely on other side
								GameObject.FindWithTag ("Cat").transform.position = new Vector3 (19.84408f, -1.975335f, 0);
								GameObject.FindWithTag ("Cat").GetComponent<SpriteRenderer> ().enabled = true;
								changeAppearance (1);
								prompting = false;
								pause = true;
								storyIndex = 34;
								text = story [storyIndex];
								Destroy (other.gameObject);
						}
				} else if (other.tag == "Cat") {
						storyIndex = 24;
						text = story [storyIndex];
						pause = true;
				} else if (other.tag == "Puddle") {
						//stepped in puddle
						if (storyIndex == 93) {
								storyIndex = 94;
								text = story [storyIndex];
								other.collider2D.enabled = false;
								pause = true;
						} else {
								storyIndex = 59;
								text = story [storyIndex];
								pause = true;
						}
				} else if (other.tag == "Brother") {
						if ((int)decisions [0] == 0 && storyIndex == 11) {
								//mom comes in while trying to walk out of door with toy
								GameObject.FindWithTag ("Mom").GetComponent<SpriteRenderer> ().enabled = true;
								storyIndex = 16;
								text = story [storyIndex];
								pause = true;
								prompting = false;
						}
				} else if (other.tag == "Desk" && storyIndex == 64) {
						//sit down at desk
						storyIndex = 65;
						text = story [storyIndex];
				}
		}
}
