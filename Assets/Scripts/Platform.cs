using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	public Collider2D platform;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		platform.isTrigger = true;
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		platform.isTrigger = false;
	}
}
