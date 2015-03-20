using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	public Collider2D platform;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		platform.enabled = false;
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		platform.enabled = true;
	}
}
