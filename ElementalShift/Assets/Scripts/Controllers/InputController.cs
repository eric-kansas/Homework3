using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * Input Controller for handling all mouse, touch, and keyboard events
 * @author  Eric Heaney, Dan Sternfeld
 * @version 1.0, June 29, 2013
 */
public class InputController : EventDispatcherBehaviour, IController {   
	
	private Pooling.Pool<TouchTracker> _touchPool = new Pooling.Pool<TouchTracker>(1);
	private Dictionary<int, TouchTracker> _touchTrackers = new Dictionary<int, TouchTracker>();
	private bool _escapeHeld = false;
	
	[SerializeField] private float _swipeDistance = 100.0f;
	
	public void Init() {
		//What has science done????
	}
	
	void Update () {
		HandleMouse();
		HandleTouch();
		HandleKeyPress();
	}
	
	/** Handle mouse input and fire corresponding events */
	private void HandleMouse() {
		// check for left mosue button click
		if (Input.GetMouseButtonDown(0)) {
			FireEvent<MouseClickEvent>(new MouseClickEvent(gameObject, MouseClickEvent.MOUSE_ACTION.LEFT_CLICK, Input.mousePosition));
		} 
		// check for right mosue button click
		if (Input.GetMouseButtonDown(1)) {
			FireEvent<MouseClickEvent>(new MouseClickEvent(gameObject, MouseClickEvent.MOUSE_ACTION.RIGHT_CLICK, Input.mousePosition));
		}
		// check for middle mosue button click
		if (Input.GetMouseButtonDown(2)) {
			FireEvent<MouseClickEvent>(new MouseClickEvent(gameObject, MouseClickEvent.MOUSE_ACTION.MIDDLE_CLICK, Input.mousePosition));
		}
	}
	
	/** Handle touch input and fire corresponding events */
	private void HandleTouch() {
		foreach(Touch touch in Input.touches) {
			switch (touch.phase) {
	            case TouchPhase.Began:
					// get touchTracker and add it to dictionary
					_touchTrackers.Add(touch.fingerId, _touchPool.Acquire());
					_touchTrackers[touch.fingerId].ResetTouch(touch);
					
					FireEvent<DragEvent>(new DragEvent(gameObject, DragEvent.DRAG_STATE.START, touch.position));
					FireEvent<TouchTrackerEvent>(new TouchTrackerEvent(gameObject, _touchTrackers[touch.fingerId]));
	                break;
	            case TouchPhase.Moved:
	            case TouchPhase.Stationary:
					// update touchTracker
					_touchTrackers[touch.fingerId].Update(touch);
					FireEvent<DragEvent>(new DragEvent(gameObject, DragEvent.DRAG_STATE.IN_PROGRESS, touch.position));
					FireEvent<TouchTrackerEvent>(new TouchTrackerEvent(gameObject, _touchTrackers[touch.fingerId]));
					CheckSwipe(touch);
					break;
	            case TouchPhase.Ended:
					// update touchTracker
					_touchTrackers[touch.fingerId].Update(touch);
					FireEvent<DragEvent>(new DragEvent(gameObject, DragEvent.DRAG_STATE.END, touch.position));
					FireEvent<TouchTrackerEvent>(new TouchTrackerEvent(gameObject, _touchTrackers[touch.fingerId]));
					CheckSwipe(touch);
					// release tracker
					_touchPool.Release(_touchTrackers[touch.fingerId]);
					_touchTrackers.Remove(touch.fingerId);
	                break;
	        }
		}
	}
	
	private void CheckSwipe(Touch touch) {
		if (!_touchTrackers[touch.fingerId].HasSwiped) {
			Vector3 displacement = _touchTrackers[touch.fingerId].DeltaPosFromStationaryPos;
			if (displacement.magnitude > _swipeDistance) {
				float angle = Vector3.Angle(displacement.normalized, Vector3.right);
				if (displacement.y < 0 )
					angle = 180 + (180 - angle);
				FireEvent<SwipeEvent>(new SwipeEvent(gameObject, _touchTrackers[touch.fingerId], angle));
				_touchTrackers[touch.fingerId].HasSwiped = true;
			}
		}
	}
	
	/** Handle touch input and fire corresponding events */
	private void HandleKeyPress() {
		if (Input.GetKey(KeyCode.Escape)) {
			_escapeHeld = true;
		} else if (_escapeHeld) {
			_escapeHeld = false;
			FireEvent<KeyPressEvent>(new KeyPressEvent(gameObject, "Escape"));
		}
	}
}
