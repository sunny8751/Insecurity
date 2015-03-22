using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour
{
	float x,y=-250;
		 Vector3 scale;
	float width = 1061, height = 597;
	public float x1 = .35f;
	Texture2D image;
		int stage = 1;
	GUIStyle style;

		void Start ()
		{
				scale = new Vector3 (Screen.width / width, Screen.height / height, 1);
		style = new GUIStyle();
		image = (Texture2D) Resources.Load("Sprites/Mirror");
		}
	
		void Update ()
		{
			if(Input.GetKeyDown(KeyCode.Space)){
			if(stage == 1){
			stage ++;
			StartCoroutine("Move");
		}else if(stage == 4){
				stage ++;
		}
	}
		if (Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit ();
		}
		}

	IEnumerator Move(){
		yield return new WaitForSeconds(2);
		float time = 0;
		while (time<2f) {
			x = Mathf.Lerp (0, -362.85f, time / 2);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		stage = 3;
		StartCoroutine("Move1");
	}
	
	IEnumerator Move1(){
		float time = 0;
		while (time<1f) {
			x = Mathf.Lerp (0, -350, time / 1);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		yield return new WaitForSeconds(1);
		stage = 4;
		StartCoroutine("Move2");
	}
	
	IEnumerator Move2(){
		float time = 0;
		while (time<1f) {
			y = Mathf.Lerp (-365, 0, time / 1);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}
	
		void OnGUI ()
		{
				var svMat = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);
				//gui begins
		if (stage == 1) {
			style.fontSize = 20;
			style.alignment = TextAnchor.MiddleLeft;
			style.normal.textColor = Color.white;
			GUI.DrawTexture(new Rect(width*.1f,height*.15f, width/2, height*.5f), image);
						GUI.Label (new Rect (width*.4f, height * .3f, width, 50), "I have always hated my leg.\nIt made me different, it made me stand out.\nI was made fun of. I couldn’t do everything that other kids could.\nBut today, my leg helped me make a friend.\nSomeone liked my leg, my fake leg.\nInsecurity has always created a sad little monster out of me, but today,\nI feel better. I like myself more.", style);
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 30;
			GUI.Label (new Rect (0, height * .8f, width, 50), "Press SPACE to continue",style);		
		}else if(stage==2){
		//insecurity displayed
			style.fontSize = 80;
			style.fontSize = 80;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (-53.84f, height * .5f+x, width, 60), "INSECURI",style);
			GUI.Label (new Rect (159.24f, height * .5f, width, 60), "T",style);
			GUI.Label (new Rect (211.23f, height * .5f, width, 60), "Y",style);
		}else if (stage==3){
			//t and y expand out
			style.fontSize = 80;
			style.fontSize = 80;
			GUI.Label (new Rect (159.24f+x, height * .5f, width, 60), "T",style);
			GUI.Label (new Rect (211.23f+x*.35f, height * .5f, width, 60), "Y",style);
		} else if (stage == 4){
			style.fontSize = 80;
			style.fontSize = 80;
			GUI.Label (new Rect (159.24f-350, height * .5f, width, 60), "T",style);
			GUI.Label (new Rect (211.23f+x*.35f, height * .5f, width, 60), "Y",style);
			GUI.Label (new Rect (159.24f-235.25f, height * .5f+y, width, 60), "hank",style);
			GUI.Label (new Rect (159.24f, height * .5f+y, width, 60), "ou",style);
			style.fontSize = 30;
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (0, height * .8f, width, 50), "Press SPACE to continue",style);		
		}else if (stage==5){
			style.fontSize = 20;
			GUI.Label(new Rect (0, -height*.05f, width, height), "Music\n"+
@"""Marty Gots a Plan"" Kevin MacLeod (incompetech.com)"+" \nLicensed under Creative Commons: By Attribution 3.0\n"+@"""http://creativecommons.org/licenses/by/3.0/"+"\n\n"+@"""Two Finger Johnny"" Kevin MacLeod (incompetech.com)"+"\nLicensed under Creative Commons: By Attribution 3.0\n"+
			          @"http://creativecommons.org/licenses/by/3.0/"+"\n\n"+@"""Monkeys Spinning Monkeys"" Kevin MacLeod (incompetech.com)"+"\nLicensed under Creative Commons: By Attribution 3.0\n"+
			          @"http://creativecommons.org/licenses/by/3.0/"+"\n\n"+@"""Lord of the Land"" Kevin MacLeod (incompetech.com)"+"\nLicensed under Creative Commons: By Attribution 3.0\n"+
			          @"http://creativecommons.org/licenses/by/3.0/"+"\n\n"+@"""Montauk Point"" Kevin MacLeod (incompetech.com) "+"\nLicensed under Creative Commons: By Attribution 3.0\n"+
			          @"http://creativecommons.org/licenses/by/3.0/",style);
			style.fontSize = 30;
			GUI.Label (new Rect (0, height * .9f, width, 30), "THE END",style);	
		}
				//end of gui
				GUI.matrix = svMat;
		}}
