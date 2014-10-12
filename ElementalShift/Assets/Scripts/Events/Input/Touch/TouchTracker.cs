using UnityEngine;
using System.Collections;

/**
 * TouchTracker is the data structure used for managing touch information
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class TouchTracker {
	
	public bool HasSwiped { get; set; }
	
	public int TrackerId { get; private set; }
	public TouchPhase Phase { get; private set; }
	public TouchPhase LastPhase { get; private set; }
	
	public Vector2 Pos { get; private set; }
	public Vector2 StartPos { get; private set; }
	public Vector2 DeltaPosFromStartPos { get; private set; }
	public Vector2 LastPos { get; private set; }
	public Vector2 DeltaPosFromLastPos { get; private set; }
	public Vector2 StationaryPos { get; private set; }
	public Vector2 DeltaPosFromStationaryPos { get; private set; }
	
	public float StartTime { get; private set; }
	public float ElapsedTime { get; private set; }
	
	public float StationaryStartTime { get; private set; }
	public float ElapsedTimeStationary { get; private set; }
	public float StationaryEndTime { get; private set; }
	public float ElapsedTimeSinceLastStationary { get; private set; }
	
	private bool _phaseChange = false;
	private bool _moved = false;
	
	public TouchTracker() {
		HasSwiped = false;
		
		Phase = TouchPhase.Began;
		LastPhase = TouchPhase.Began;
		
		Pos = Vector2.zero;
		
		StartPos = Vector2.zero;
		DeltaPosFromStartPos = Vector2.zero;
		
		StationaryPos = Vector2.zero;
		DeltaPosFromStationaryPos = Vector2.zero;
		
		LastPos = Vector2.zero;
		DeltaPosFromLastPos = Vector2.zero;
			
		StartTime = StationaryEndTime = Time.time;
		ElapsedTime = StationaryStartTime = ElapsedTimeStationary = ElapsedTimeSinceLastStationary = 0.0f;
	}
	
	public TouchTracker(Touch touch) {
		ResetTouch(touch);
	}
	
	public void ResetTouch(Touch touch) {
		HasSwiped = false;
		TrackerId = touch.fingerId;
		Phase = LastPhase = touch.phase;
		
		StartPos = StationaryPos = Pos = LastPos = touch.position;
		DeltaPosFromStartPos = DeltaPosFromLastPos = DeltaPosFromStationaryPos = Vector2.zero;
			
		StartTime = StationaryEndTime = Time.time;
		ElapsedTime = StationaryStartTime = ElapsedTimeStationary = ElapsedTimeSinceLastStationary = 0.0f;
	}
	
	public void Update (Touch touch) {
		LastPhase = Phase;
		Phase = touch.phase;
		
		if (Phase != LastPhase) {
			_phaseChange = true;	
		} else {
			_phaseChange = false;	
		}
		
		LastPos = Pos;
		Pos = touch.position;
		
		DeltaPosFromStartPos = Pos - StartPos;
		DeltaPosFromLastPos = Pos - LastPos;
		
		ElapsedTime = Time.time - StartTime;
		
		if (Phase == TouchPhase.Stationary) {
			if (LastPhase != TouchPhase.Stationary) {
				StationaryStartTime = Time.time;
				StationaryPos = Pos;		
			}
			ElapsedTimeStationary = Time.time - StationaryStartTime;
		} else if (LastPhase == TouchPhase.Stationary) {
			ElapsedTimeStationary = Time.time - StationaryStartTime;
			StationaryEndTime = Time.time;
		}
		
		DeltaPosFromStationaryPos = Pos - StationaryPos;
		ElapsedTimeSinceLastStationary = Time.time - StationaryEndTime;
	}
}
