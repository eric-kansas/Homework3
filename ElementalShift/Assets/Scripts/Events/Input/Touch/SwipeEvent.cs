using UnityEngine;
using System.Collections;

/**
 * SwipeEvent is used to pass events related to mouse click inputs
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class SwipeEvent : TouchTrackerEvent {

	public float Angle { get; set; }
	
		
	public SwipeEvent(GameObject source, TouchTracker touchTracker, float angle) : base(source, touchTracker) {
		Name = "SwipeEvent";
		Angle = angle;
	}
}
