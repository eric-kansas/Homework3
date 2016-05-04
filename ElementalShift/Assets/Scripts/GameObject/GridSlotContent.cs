using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour {
	public static float ELEMENT_WIDTH = 1.0f;
	public static float ELEMENT_HEIGHT = 1.0f;

	private Sprite _sprite;
	public Sprite Sprite {
		get { return _sprite;}
		set {
			GetComponent<SpriteRenderer>().sprite = value;
			_sprite = value;
		}
	}

	[SerializeField]
	private string _matchType;
	public string MatchType {
		get { return _matchType; }
		set { _matchType = value; }
	}

	[SerializeField]
	private bool _isMovable = true;
	public bool IsMovable {
		get { return _isMovable; }
		set { _isMovable = value; }
	}

	bool isAnimating = false;
	Vector3 endPoint;
	float duration = 0.25f;
	
	private Vector3 startPoint;
	private float startTime;

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

	public static bool operator ==(Element x, Element y) {
		if (object.ReferenceEquals(x, null)) {
			return object.ReferenceEquals(y, null);
		}

		if (object.ReferenceEquals(y, null)) {
			return false;
		}

		return x.MatchType == y.MatchType;
	}
	
	public static bool operator !=(Element x, Element y) {
		if (object.ReferenceEquals(x, null)) {
			return !object.ReferenceEquals(y, null);
		}

		if (object.ReferenceEquals(y, null)) {
			return true;
		}

		return x.MatchType != y.MatchType;
	}
}
