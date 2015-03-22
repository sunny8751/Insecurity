using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour
{

		public Vector3 scale;
		float width = 1061, height = 597;
		int stage = 1;

		void Start ()
		{
				scale = new Vector3 (Screen.width / width, Screen.height / height, 1);
		}
	
		void Update ()
		{
			if(Input.GetKeyDown(KeyCode.Space)){
			stage ++;
		}
		}

		void OnGUI ()
		{
				var svMat = GUI.matrix;
				GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scale);
				//gui begins
				GUIStyle style = new GUIStyle ();
				style.fontSize = 40;
				style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;
				if (stage == 1) {
						GUI.Label (new Rect (0, height * .1f, width, 50), "Staring at the mirror, all you see is a monster in the reflection. But you are more than that.\nI have always hated my leg. It made me different, it made me stand out. I was made fun of. I couldn’t do everything that other kids could. But today, my leg helped me make a friend. Someone liked my leg, my fake leg. Insecurity has always created a sad little monster out of me, but today, I feel better. I like myself more.", style);
				}
				GUI.Label (new Rect (0, height * .8f, width, 50), "Press SPACE to continue",style);
		
				//end of gui
				GUI.matrix = svMat;
		}
}
