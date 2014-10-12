using UnityEngine;
using System.Collections;

/**
 * KeyStateChangeEvent is used to pass events related to key press and key release
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class KeyStateChangeEvent : InputEvent {
	
	public enum KEY_STATE {
		RELEASED,
		PRESSED,
		NUM_KEY_STATES
	}
	
	public KEY_STATE KeyState { get; set; }
	public string KeyName { get; set; }
	
	public KeyStateChangeEvent(GameObject source, string keyName, KEY_STATE keyState) : base(source) {
		Name = "KeyStateChangeEvent";
		KeyName = keyName;
		KeyState = keyState;
	}
}