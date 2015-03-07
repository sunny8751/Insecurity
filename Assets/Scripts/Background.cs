using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	float interval;
	GameObject [] backgrounds;

	void Start () {
		backgrounds = GameObject.FindGameObjectsWithTag("Background");
		interval = backgrounds[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		backgrounds[1].transform.position = new Vector3(interval, 0,0);
	}
	
	void Update () {
	}
}
