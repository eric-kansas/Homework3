using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchType : MonoBehaviour {

	public string id;
	public List<string> validIdOverride;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool doesMatch(MatchType typeToCompare) {
		return typeToCompare.id == id;
	}
}
