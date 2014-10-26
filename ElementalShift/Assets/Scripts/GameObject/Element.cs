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

	private string _type;
	public string Type {
		get { return _type; }
		set { _type = value; }
	}

	bool isAnimating = false;
	Vector3 endPoint;
	float duration = 0.25f;
	
	private Vector3 startPoint ;
	private float startTime;
	
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (isAnimating) {
			transform.position = Vector3.Lerp(startPoint, endPoint, (Time.time - startTime) / duration);
			float distanceSqr = (transform.position - endPoint).sqrMagnitude;
			if (distanceSqr < 0.01f) {
				transform.position = endPoint;
				isAnimating = false;
			}
		}
	}

	public void AnimateTo(Vector3 endPosition) {
		isAnimating = true;
		startPoint = transform.position;
		startTime = Time.time;
		endPoint = endPosition;
	}
}
