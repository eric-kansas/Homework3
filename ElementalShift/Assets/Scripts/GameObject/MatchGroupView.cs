using UnityEngine;

public class MatchGroupView
{
	public Sprite matchGroupSprite;

	private GameObject me;

	SpriteRenderer spriteRenderer;
	
	public MatchGroupView (Vector3 minPosition, Vector3 maxPosition)
	{
		matchGroupSprite = Resources.Load <Sprite> ("Sprites/match");
		me = new GameObject();
		me.transform.position = new Vector3();
		// make guiText for selection rectangle
		spriteRenderer = (SpriteRenderer) me.AddComponent("SpriteRenderer");
		spriteRenderer.transform.localScale = new Vector3();
		spriteRenderer.sprite = matchGroupSprite;

		// Clear selection graphic scale
		spriteRenderer.transform.localScale = new Vector3();
		
		spriteRenderer.transform.localScale = minPosition - maxPosition;
		
		spriteRenderer.transform.position = minPosition - (spriteRenderer.transform.localScale / 2);
	}
	
	public void Destory() {
		GameObject.Destroy(me);
	}
}


