using UnityEngine;
using System.Collections;

public class ElementGroup {

	private Vector2 _minIndex = new Vector2(); // Top - left
	public Vector2 MinIndex {
		get { return _minIndex; }
		set {
			_minIndex = value;
			CalculateDimensions();
		}
	}

	private Vector2 _maxIndex = new Vector2(); // Botton - right
	public Vector2 MaxIndex {
		get { return _maxIndex; }
		set {
			_maxIndex = value;
			CalculateDimensions();
		}
	}

	private int _width = 0;
	public int Width {
		get { return _width; }
	}

	private int _height = 0;
	public int Height {
		get { return _height; }
	}

	public ElementGroup(Vector2 minIndex) : this(minIndex, minIndex) {
	}

	public ElementGroup(Vector2 minIndex, Vector2 maxIndex) {
		_minIndex = minIndex;
		_maxIndex = maxIndex;

		CalculateDimensions();
	}

	private void CalculateDimensions() {
		_width  = (int)(_maxIndex.x - _minIndex.x) + 1;
		_height = (int)(_maxIndex.y - _minIndex.y) + 1;
	}
}
