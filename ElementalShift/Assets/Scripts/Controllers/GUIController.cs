using UnityEngine;
using System.Collections;

public class GUIController : Controller {

	private int levelCounter = 1;

	void OnGUI () {
		if (running) {
			GUIStyle headerStyle = new GUIStyle();
			headerStyle.alignment = TextAnchor.MiddleLeft;
			headerStyle.normal.textColor = Color.black;

			/*
			if (timeLeft < (roundLength * 0.2f)) {
				headerStyle.normal.textColor = Color.red;
			} else if (timeLeft < (roundLength * 0.5f)) {
				headerStyle.normal.textColor = Color.yellow;
			} else {
				headerStyle.normal.textColor = Color.green;
			}
			*/

			headerStyle.fontSize = 200;

			if (GUI.Button(new Rect(Screen.width - (Screen.width / 3), Screen.height - 100, (Screen.width / 3), 100), "Next")) {
				levelCounter++;
				if (levelCounter > 8) {
					levelCounter = 1;
				}
				Debug.Log(levelCounter);
				Application.LoadLevel("Level" + levelCounter);
			}
			
			if (GUI.Button(new Rect(0, Screen.height - 100, (Screen.width / 3), 100), "Reset")) {
				GameController.Reset();
			}
		}
	}
}
