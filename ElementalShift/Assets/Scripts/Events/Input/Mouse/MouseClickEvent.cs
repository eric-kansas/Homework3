using UnityEngine;
using System.Collections;

/**
 * MouseClickEvent is used to pass events related to mouse click inputs
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class MouseClickEvent : MouseEvent {

	public struct MOUSE_ACTION {
	   	public const string LEFT_CLICK = "LeftClick";
   		public const string RIGHT_CLICK = "RightClick";
    	public const string MIDDLE_CLICK = "MiddleClick";
	}

	public string MouseAction { get; set; }
		
	public MouseClickEvent(GameObject source, string mouseAction, Vector3 position) : base(source, position) {
		Name = "MouseClickEvent";
		MouseAction = mouseAction;
	}
}
