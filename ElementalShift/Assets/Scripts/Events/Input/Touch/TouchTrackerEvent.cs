using UnityEngine;
using System.Collections;

public class TouchTrackerEvent : InputEvent {
	public TouchTracker TouchTracker { get; set; }
		
	public TouchTrackerEvent(GameObject source, TouchTracker touchTracker) : base(source) {
		Name = "TouchTrackerEvent";
		TouchTracker = touchTracker;
	}
}