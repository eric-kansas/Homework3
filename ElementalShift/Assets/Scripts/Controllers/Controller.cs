using UnityEngine;
using System.Collections;

public abstract class Controller : MonoBehaviour {

	private bool running = false;
	
	public void Init() {
		running = true;
	}
}
