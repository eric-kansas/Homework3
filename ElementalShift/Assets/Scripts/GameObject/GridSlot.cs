using UnityEngine;
using System.Collections;

public class GridSlot {
	
	public Bounds Boundary;
	public Vector2 BoardIndex;

	private Element _content;
	public Element Content {
		get { return _content; }
		set {
			_content = value;
			_content.transform.position = Boundary.center;
		}
	}
	public GridSlot(Vector2 boardIndex, Bounds bounds, Element content) {
		BoardIndex = boardIndex;
		Boundary = bounds;
		Content = content;
	}
}
