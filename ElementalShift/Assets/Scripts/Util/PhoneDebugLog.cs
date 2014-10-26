using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// manages game logic
public class PhoneDebugLog : MonoBehaviour {
	
	private static string myLog = "";
	private static Vector2 scrollPosition = Vector2.zero;
	
	void Start()
	{
		//Globals.Instance.GetController<InputController>().WireEvent<KeyPressEvent>(handleKeyEvent);
	}

	void handleKeyEvent(KeyPressEvent e)
	{
		HandleLog("OH GOD");
	}
	
	public static void HandleLog (string logString) {
		myLog +="\n" + logString;
		scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
	}
	
	
	void OnGUI () {
		if (Input.GetKey(KeyCode.Escape)) {
			myLog = "";
		}
		GUILayout.BeginArea(new Rect(10, 10, Screen.width-10, Screen.height/4));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(Screen.width-10), GUILayout.Height(Screen.height/4));
		myLog = GUILayout.TextArea(myLog);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
	}
}
