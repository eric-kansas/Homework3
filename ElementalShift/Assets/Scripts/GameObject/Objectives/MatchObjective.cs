using UnityEngine;
using System.Collections;

public class MatchObjective : MonoBehaviour {

	// A delegate type for hooking up change notifications.
	public delegate void OnObjectiveCompleteHandler();

	public event OnObjectiveCompleteHandler OnObjectiveComplete;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
