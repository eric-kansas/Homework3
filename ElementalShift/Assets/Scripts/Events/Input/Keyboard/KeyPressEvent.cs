using UnityEngine;
using System.Collections;

/**
 * KeyPressEvent is used to pass events related to a key being pressed and released
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class KeyPressEvent : InputEvent {

	public string KeyName { get; set; }
	
	public KeyPressEvent(GameObject source, string keyName) : base(source) {
		Name = "KeyPressEvent";
		KeyName = keyName;
	}
}