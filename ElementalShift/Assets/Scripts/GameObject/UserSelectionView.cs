using UnityEngine;

public class UserSelectionView
{
	GUITexture selectionUIGraphic;
	Vector3 topLeftPosition = new Vector3();

	public UserSelectionView (GameObject gameObject, Texture2D selectinRectTexture)
	{
		// make guiText for selection rectangle
		selectionUIGraphic = (GUITexture) gameObject.AddComponent("GUITexture");
		selectionUIGraphic.transform.localScale = new Vector3();
		selectionUIGraphic.texture = selectinRectTexture;
		selectionUIGraphic.color = Color.black;
	}

	public void Clear() {
		selectionUIGraphic.transform.localScale = new Vector3();
	}

	public void updateSelection(Vector3 minPosition, Vector3 maxPosition) {
		minPosition.y *= -1;
		minPosition = Camera.main.WorldToViewportPoint(minPosition);
		maxPosition.y *= -1;
		maxPosition = Camera.main.WorldToViewportPoint(maxPosition);
		
		// Clear selection graphic scale
		selectionUIGraphic.transform.localScale = new Vector3();

		selectionUIGraphic.transform.localScale = minPosition - maxPosition;
		
		selectionUIGraphic.transform.position = minPosition - (selectionUIGraphic.transform.localScale / 2);

	}
}


