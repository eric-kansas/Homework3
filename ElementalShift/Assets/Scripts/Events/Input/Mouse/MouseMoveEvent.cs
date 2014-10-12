using UnityEngine;
using System.Collections;

/**
 * MouseMoveEvent is used to pass events related to mouse movement
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class MouseMoveEvent : MouseEvent {
		
	public MouseMoveEvent(GameObject source, Vector3 position) : base(source, position) {
		Name = "MouseMoveEvent";
	}
}
