using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {
	public static float ELEMENT_WIDTH = 1.0f;
	public static float ELEMENT_HEIGHT = 1.0f;

	private Sprite _sprite;
	public Sprite Sprite
	{
		get { return _sprite;}
		set
		{
			GetComponent<SpriteRenderer>().sprite = value;
			_sprite = value;
		}
	}	
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}
}
