using UnityEngine;
using System.Collections;

public class SelectionRect {
	
	public Rect BoundingRect { get; private set; }
	public Vector3 StartPosition { get; private set; }
	public int XGridLength { get; private set; }
	public int YGridLength { get; private set; }
	
	public SelectionRect() {
		
	}
	
	public void SetStartPosition(Vector3 pos) {
		StartPosition = pos;
		/*
		// set selection rectangle properties
		BoundingRect.x = StartPosition.x * Globals.ELEMENT_WIDTH;
		BoundingRect.y = StartPosition.y *  Globals.ELEMENT_WIDTH;
		BoundingRect.width = Globals.ELEMENT_WIDTH;
		BoundingRect.height = Globals.ELEMENT_WIDTH;
		
		//visible = true;
*/
	}
	
	public void adjustRectangle (Vector3 currentPos) {
		/*
		Vector3 difference = currentPos - StartPosition;
		
		// check x's
		if (difference.x > 0) {
			BoundingRect.width = (difference.x + 1) * Globals.ELEMENT_WIDTH;
		} else if (difference.x < 0) {
			BoundingRect.x = (BoundingRect.x + difference.x) * Globals.ELEMENT_WIDTH;
			BoundingRect.width = ((difference.x - 1) * Globals.ELEMENT_WIDTH) * -1;
		} else {
			BoundingRect.x = selectionPoint.x * Globals.ELEMENT_WIDTH;
			BoundingRect.width = Globals.ELEMENT_WIDTH;
		}
		
		// check y's
		if (difference.y > 0) {
			BoundingRect.height = (difference.y + 1) * Globals.ELEMENT_WIDTH;
		} else if (difference.y < 0) {
			BoundingRect.y = (BoundingRect.y + difference.y) * Globals.ELEMENT_WIDTH;
			BoundingRect.height = ((difference.y - 1) * Globals.ELEMENT_WIDTH) * -1;
		} else {
			BoundingRect.y = selectionPoint.y * Globals.ELEMENT_WIDTH;
			BoundingRect.height = Globals.ELEMENT_WIDTH;
		}
		
		//grid positions
		BoundingRect.xMin = BoundingRect.x / Globals.ELEMENT_WIDTH;
		BoundingRect.xMax = (BoundingRect.x + BoundingRect.width) / Globals.ELEMENT_WIDTH;
		BoundingRect.yMin = BoundingRect.y / Globals.ELEMENT_WIDTH;
		BoundingRect.yMax = (BoundingRect.y + BoundingRect.height - 1) / Globals.ELEMENT_WIDTH;
		
		XGridLength = ((selectionRectangle.x + selectionRectangle.width - 1) / Globals.ELEMENT_WIDTH) - selectionRectangle.x / Globals.ELEMENT_WIDTH + 1;
		YGridLength = ((selectionRectangle.y + selectionRectangle.height - 1) / Globals.ELEMENT_WIDTH) - selectionRectangle.y / Globals.ELEMENT_WIDTH + 1;	
		*/
	}
	
	public void finalizeSelection() {
		/*
		if (selectionRectangle.width == selectionRectangle.height && selectionRectangle.width != Globals.ELEMENT_WIDTH) {
			isSquare = true;
		} else {
			isSquare = false;
		}
		*/
		//GameValues.mechanic.canTransform = true;
	}

}
