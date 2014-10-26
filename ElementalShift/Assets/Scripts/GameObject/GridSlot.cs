using UnityEngine;
using System;
using System.Collections;

public class GridSlot {

	private Vector3 _position;

	private Bounds _boundary;
	public Bounds Boundary {
		get { 
			return _boundary;
		} 
		set {
			_boundary = value;
		}
	}
	public Vector2 BoardIndex;

	private Element _content;
	public Element Content {
		get { return _content; }
		set {
			_content = value;
			if (_content != null) {
				_content.AnimateTo(_position);
			}  
		}
	}

	private bool _isInMatch = false;
	public bool IsInMatch {
		get { return _isInMatch; }
		set { _isInMatch = value; }
	}

	private string _matchGuid;
	public string MatchGuid {
		get { return _matchGuid; } 
		set { 
			_matchGuid = value;
			if (String.IsNullOrEmpty(_matchGuid)) {
				_isInMatch = false;
			} else {
				_isInMatch = true;
			}
		}
	}

	public GridSlot(Vector2 index, Vector3 position, Element content, GameObject boardGameObject) {
		_position = position;
		BoardIndex = index;
		Boundary = new Bounds(position, BoardManager.cellSize);
		Content = content;
		Content.transform.parent = boardGameObject.transform;
	}

	public string GetContentType() {
		if (_content != null) {
			return Content.Type;
		} else {
			return "";
		}
	}

	public void ClearContent() {
		if (_content != null) {
			GameObject.DestroyImmediate(_content.gameObject);
			_content = null;
		}
	}
	
}
