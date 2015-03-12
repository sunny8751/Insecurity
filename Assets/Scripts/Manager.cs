using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	void Start () {
		//level 1
		//gameObject.GetComponent<Background>().Init(false, "bedroom");
		Information.scrollable = false;
		Information.jumpScale = 1.1f;
		GameObject.Find("Menu/Bedroom").SetActive(true);
	}

	void Update () {
	
	}
}
