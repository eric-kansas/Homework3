using UnityEngine;
using System.Collections;

/**
 * SwipeEvent is used to pass events related to mouse click inputs
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class DragEvent : MouseEvent {
	
	public enum DRAG_STATE {
		START,
		IN_PROGRESS,
		END
	}
	
	public DRAG_STATE State { get; private set; }
		
	public DragEvent(GameObject source, DRAG_STATE state, Vector3 pos) : base(source, pos) {
		Name = "DragEvent";
		State = state;
	}
}
