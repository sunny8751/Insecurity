using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
		float interval;
		// the two background gameobjects
		public GameObject[] backgrounds;
		Transform playerTransform;
		float width;

	void Start(){
		//set world width
		width = -2 * Camera.main.ScreenToWorldPoint (Vector3.zero).x;
		//find the backgrounds
		backgrounds = GameObject.FindGameObjectsWithTag ("Background");
		//set player transform
		playerTransform = GameObject.FindWithTag ("Player").transform;
	}
		
		public void Init (bool scroll, string filename)
		{
		Information.scrollable = scroll;
		Sprite newSprite = Resources.Load<Sprite>("Sprites/"+filename);
		float spriteWidth = newSprite.bounds.size.x;
		if(Information.scrollable){
		foreach(GameObject b in backgrounds){
				b.SetActive(true);
			b.GetComponent<SpriteRenderer>().sprite = newSprite;
				b.transform.localScale = new Vector3(width/spriteWidth,width/spriteWidth,1);
		}
				interval = backgrounds [0].GetComponent<SpriteRenderer> ().sprite.bounds.size.x;
				backgrounds [0].transform.position = new Vector3 (-interval, 0, 0);
				backgrounds [2].transform.position = new Vector3 (interval, 0, 0);
		}else{
			//not Information.scrollable
			//set other backgrounds not active
			backgrounds[1].SetActive(false);
			backgrounds[2].SetActive(false);
			backgrounds[0].GetComponent<SpriteRenderer>().sprite = newSprite;
			backgrounds[0].transform.localScale = new Vector3(width/spriteWidth,width/spriteWidth,1);
		}
	}

		void Update ()
		{
		if(!Information.scrollable){
			return;
		}
				float minX = playerTransform.position.x - width / 2 - 1;
				float maxX = playerTransform.position.x + width / 2 + 1;
				if (minX < getBackground (true).position.x - interval / 2) {
						// going back
						setBackgroundPos (getBackground (false), -1);
				} else if (maxX > getBackground (false).position.x + interval / 2) {
						// going forward
						setBackgroundPos (getBackground (true), 1);
				}
		}

		void setBackgroundPos (Transform t, int dir)
		{
				//move the background
				t.position = new Vector3 (t.position.x + dir * 3 * interval, t.position.y, t.position.z);
		}

		Transform getBackground (bool first)
		{
				//want the first background in terms of x?
				Transform answer = backgrounds [0].transform;
				if (first) {
						for (int i = 1; i<backgrounds.Length; i ++) {
								//dont repeat anything or compare the same thing
								if (backgrounds [i].transform.position.x < answer.position.x) {
										answer = backgrounds [i].transform;
								}
						}
				} else {
						//dont want the first one
						for (int i = 1; i<backgrounds.Length; i ++) {
								//dont repeat anything or compare the same thing
								if (backgrounds [i].transform.position.x > answer.position.x) {
										answer = backgrounds [i].transform;
								}
						}
				}
				return answer;
		}
}