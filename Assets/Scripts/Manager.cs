using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	public GameObject [] backgrounds, foregrounds;
	Transform playerTransform;

	void Start () {
		playerTransform = GameObject.FindWithTag("Player").transform;
		changeLevel(1);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.A)){
			changeLevel(2);
		}
	}

	void changeLevel (int l) {
		//turn off the backgrounds
		for(int i =0; i<backgrounds.Length; i++){
			backgrounds[i].SetActive(false);
		}
		Information.level = l;
		if(l==1){
			playerTransform.position = new Vector3(0,-3,0);
			Information.scrollable = false;
			Information.jumpScale = 1.1f;
			backgrounds[0].SetActive(true);
		}else if(l==2){
			playerTransform.position = new Vector3(0,-1,0);
			Information.scrollable = true;
			//set backgrounds in Background
			Background.backgrounds = new GameObject[]{backgrounds[1],backgrounds[2],backgrounds[3]};
			Background.Init();
			//make sky's ground active
			foregrounds[0].SetActive(true);
			Information.jumpScale = 1.1f;
			backgrounds[1].SetActive(true);
			backgrounds[2].SetActive(true);
			backgrounds[3].SetActive(true);
		}
	}
}
