using UnityEngine;
using System.Collections;

/**
 * InputEvent is used to pass events related to the input
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public abstract class InputEvent : BaseEvent {
	public InputEvent(GameObject source) : base(source) { 
		Name = "InputEvent";
	}
}