using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour {

	private string prefabsPath = "Prefabs/Elements/";
		
	[SerializeField] 
	private int width = 9;

	[SerializeField] 
	private int height = 9;

	[SerializeField]
	private float padding = 0.1f;
	
	[SerializeField]
	private int z = 0;

	private Element[] elementPrefabs;

	private GridSlot[,] board;
	private float cellWidth;
	private float cellHeight;
	private Vector3 cellSize;
	private float halfWidth;
	private float halfHeight;

	void Start () {
		Resources.LoadAll(prefabsPath);
		// Grab elements 
		elementPrefabs = (Element[]) Resources.FindObjectsOfTypeAll(typeof(Element));

		board = new GridSlot[width, height];

		cellWidth = (Element.ELEMENT_WIDTH + padding);
		cellHeight = (Element.ELEMENT_HEIGHT + padding);
		cellSize = new Vector3(cellWidth, cellHeight, 0);

		halfWidth = (width / 2);
		halfHeight = (height / 2);

		for (int yIndex = 0; yIndex < height; yIndex++) {
			for (int xIndex = 0; xIndex < width; xIndex++) {
				// Get next grid slot position
				Vector3 position = new Vector3(((xIndex - halfWidth) * cellWidth), ((yIndex - halfHeight) * cellHeight), z);

				// Generate element
				Element gridElement = GenerateElement();

				// Build grid slot
				Bounds gridBounds = new Bounds(position, cellSize);
				GridSlot slot = new GridSlot(new Vector2(xIndex, yIndex), gridBounds, gridElement);

				board[xIndex, yIndex] = slot;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private Element GenerateElement() {
		int rand = Random.Range(0, elementPrefabs.Length);
		string elementType = elementPrefabs[rand].name;
		Element gridElement = (Element) Instantiate(Resources.Load(prefabsPath + elementType, typeof(Element)), new Vector3(), Quaternion.identity);
		return gridElement;
	}

	
	public GridSlot GetClosestGridSlotToPoint(Vector3 position) {
		Ray ray = Camera.main.ScreenPointToRay(position);    
		Vector3 point = ray.origin + (ray.direction * (Camera.main.transform.position.z-1) *-1);
		int xIndex = (int)((point.x / cellWidth) + (cellWidth / 2) + halfWidth);
		int yIndex = (height - 1) - (int)((point.y / cellHeight) + (cellHeight / 2) + halfHeight);
		if ( xIndex >= 0 && xIndex < width && yIndex >= 0 && yIndex < height)
		{
			Debug.Log("xIndex: " + xIndex + ", yIndex: " + yIndex);
			GridSlot temp = board[xIndex, yIndex];
			return temp;
		}
		return null;
	}

	public GridSlot GetGridSlotAtIndex(Vector2 position) {
		if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height) {
			return null;
		}
		return board[(int)position.x, (int)position.y];
	}

	public GridSlot GetGridSlotAtIndex(int xIndex, int yIndex) {
		if (xIndex < 0 || xIndex >= width || yIndex < 0 || yIndex >= height) {
			return null;
		}
		return board[xIndex, yIndex];
	}

	// Swaping logic
	public void PerformSwap(UserSelection.SWAP_DIRECTIONS direction, Vector2 minIndex, Vector2 maxIndex) {
		switch(direction) {
			case UserSelection.SWAP_DIRECTIONS.NORTH: {
				PerformNorthSwap(minIndex, maxIndex);
				break;
			}
		}
	}

	private void SwapGridContents(Vector2 index1, Vector2 index2) {
		Element temp = board[(int)index1.x, (int)index1.y].Content;
		board[(int)index1.x, (int)index1.y].Content = board[(int)index2.x, (int)index2.y].Content;
		board[(int)index2.x, (int)index2.y].Content = temp;
	}

	public void PerformNorthSwap(Vector2 minIndex, Vector2 maxIndex) {
		int xCounter = 0;
		int yCounter = 0;
		int width = (int)(maxIndex.x - minIndex.x);
		int height = (int)(maxIndex.y - minIndex.y);
		for (int y = (int)minIndex.y; y < minIndex.y + (height / 2); y++) {
			for (int x = (int)minIndex.x; x < minIndex.x + (width / 2); x++) {
				Debug.Log("here");
				SwapGridContents(new Vector2(x,y), new Vector2(x, maxIndex.y - yCounter));
				xCounter++;
			};
			yCounter++;
		}
	}
}
