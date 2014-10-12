using UnityEngine;
using System.Collections;

public class GameBoardController : EventDispatcherBehaviour, IController  {
	
	[SerializeField] private GameObject elementPrefab;
	[SerializeField] private Vector2 _dimensions;
	[SerializeField] private int _numOfElementTypes; 
	
	public Element[,] Board { get; set; }
	
	private Vector3 _startDragPos = Vector3.zero;
	private Vector3 _currentDragPos = Vector3.zero;
	
	private bool _dragging = false;
	
	/** Init for the game board controller */
	public void Init() {
		Globals.Instance.GetController<InputController>().WireEvent<DragEvent>(DragHandler);
		
		BuildBoard();
	}
	
	private void DragHandler(DragEvent dragEvent) {
		switch (dragEvent.State) {
			case DragEvent.DRAG_STATE.START:
				_dragging = true;
				_startDragPos = _currentDragPos = SnapToPos(dragEvent.Position);
				break;
			case DragEvent.DRAG_STATE.IN_PROGRESS:
				_currentDragPos = SnapToPos(dragEvent.Position);
				break;
			case DragEvent.DRAG_STATE.END:
				_currentDragPos = SnapToPos(dragEvent.Position);
				_dragging = false;
				break;
		}
	}
	
	private Vector3 SnapToPos(Vector3 position) {
		RaycastHit hit;
		Physics.Raycast( Camera.main.ScreenPointToRay( position ), out hit, 1000f );
				
		position = new Vector3((int) hit.transform.position.x, (int) hit.transform.position.y, (int) hit.transform.position.z);
		
		position = Camera.main.WorldToScreenPoint(position);
		position = new Vector3(position.x, position.y, 0);
		
		return position;
	}
	
	private void BuildBoard() {
		Board = new Element[(int)_dimensions.y, (int)_dimensions.x];
		
		for (int y = 0; y < _dimensions.y; y++) {
			for (int x = 0; x < _dimensions.x; x++) {
				int randomNumber = Random.Range(0, _numOfElementTypes);
				GameObject myNewObject = (GameObject) Instantiate (elementPrefab, new Vector3(x * elementPrefab.transform.localScale.x, 0, y * elementPrefab.transform.localScale.y), Quaternion.identity);
				Color color;
				switch (randomNumber) {
					case 0 : color = Color.red; break;
					case 1 : color = Color.blue; break;
					case 2 : color = Color.green; break;
					case 3 : color = Color.yellow; break;
					default: color = Color.red; break;
				}
				myNewObject.renderer.material.color = color;
			}
		}
	}
	
	public void OnGUI() {
		if (_dragging) {
			Vector3 difference = _startDragPos - _currentDragPos;
			DrawQuad(new Rect(_startDragPos.x, Screen.height - _startDragPos.y, -difference.x, difference.y), new Color(10,10,10,0.5f));	
		} else {
			_startDragPos = _currentDragPos = Vector3.zero;
		}
	}
	
	void DrawQuad(Rect position, Color color) {
	    Texture2D texture = new Texture2D(1, 1);
	    texture.SetPixel(0,0,color);
	    texture.Apply();
	    GUI.skin.box.normal.background = texture;
	    GUI.Box(position, GUIContent.none);
	}
}
