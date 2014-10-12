using UnityEngine;
using System.Collections;

/**
 * MouseEvent is used to pass events related to mouse inputs
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public abstract class MouseEvent : InputEvent {
	
	public Vector3 Position { get; set; }
	
	public MouseEvent(GameObject source, Vector3 position) : base(source) { 
		Name = "MouseEvent";
		Position = position;
	}

}