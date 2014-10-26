using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	private string prefabsPath = "Prefabs/Elements/";
	
	public GameObject boardGameObject;
		
	public static int width = 7;
	// TODO: odd heights break element selection
	public static int height = 10;

	[SerializeField]
	private float padding = 0.1f;

	private Element[] elementPrefabs;

	public static MatchManager matchManager;

	public static GridSlot[,] gameBoard;
	
	public static float cellWidth;
	public static float cellHeight;
	public static Vector3 cellSize;
	private float halfWidth;
	private float halfHeight;

	private float spawnHeight = 0.0f;

	private static BoardManager _instance;
	public static BoardManager GetInstance {
		get { return _instance; }
	}

	void Start () {
		Resources.LoadAll(prefabsPath);
		// Grab elements 
		elementPrefabs = (Element[]) Resources.FindObjectsOfTypeAll(typeof(Element));

		gameBoard = new GridSlot[width, height];
		matchManager = new MatchManager();

		cellWidth = (Element.ELEMENT_WIDTH + padding);
		cellHeight = (Element.ELEMENT_HEIGHT + padding);
		cellSize = new Vector3(cellWidth, cellHeight, 0);

		halfWidth = (width / 2);
		halfHeight = (height / 2);

		spawnHeight = cellHeight * (height - halfHeight + 1);

		for (int yIndex = 0; yIndex < height; yIndex++) {
			for (int xIndex = 0; xIndex < width; xIndex++) {
				Vector2 currentIndex = new Vector2(xIndex, yIndex);
				// Get next grid slot position
				float xPosition = (xIndex - halfWidth) * cellWidth;
				float yPosition = (yIndex - halfHeight) * cellHeight * -1;
				Vector3 position = new Vector3(xPosition, yPosition, 0);

				// Generate element
				Element randomElement = GenerateElement();
				randomElement.transform.position = new Vector3(xPosition, spawnHeight, 0);

				// Build grid slot with element
				GridSlot slot = new GridSlot(currentIndex, position, randomElement, boardGameObject);
				gameBoard[xIndex, yIndex] = slot;
			}
		}
		matchManager.CheckMatches();
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Creates a randome element from elementPrefabs
	private Element GenerateElement() {
		int rand = UnityEngine.Random.Range(0, elementPrefabs.Length);
		string elementType = elementPrefabs[rand].name;
		Element gridElement = (Element) Instantiate(Resources.Load(prefabsPath + elementType, typeof(Element)), new Vector3(), Quaternion.identity);
		gridElement.Type = elementType;
		return gridElement;
	}

	
	public GridSlot GetClosestGridSlotToPoint(Vector3 position) {
		Ray ray = Camera.main.ScreenPointToRay(position);    
		Vector3 point = ray.origin + (ray.direction * (Camera.main.transform.position.z - 1));
		int xIndex = (int)((point.x / cellWidth) + (cellWidth / 2) + halfWidth);
		int yIndex = (height) - (int)((point.y / cellHeight) + (cellHeight / 2) + halfHeight);
		if ( xIndex >= 0 && xIndex < width && yIndex >= 0 && yIndex < height){
			GridSlot temp = gameBoard[xIndex, yIndex];
			return temp;
		}
		return null;
	}

	public static GridSlot GetGridSlotAtIndex(Vector2 position) {
		if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height) {
			return null;
		}
		return gameBoard[(int)position.x, (int)position.y];
	}

	public static GridSlot GetGridSlotAtIndex(int xIndex, int yIndex) {
		if (xIndex < 0 || xIndex >= width || yIndex < 0 || yIndex >= height) {
			return null;
		}
		return gameBoard[xIndex, yIndex];
	}

	public void RemoveGroup(ElementGroup matchGroup) {

		// Clear GridSlot Content
		for (int y = (int)matchGroup.MinIndex.y; y <= matchGroup.MaxIndex.y; y++) {
			for (int x = (int)matchGroup.MinIndex.x; x <= matchGroup.MaxIndex.x; x++) {
				gameBoard[x, y].ClearContent();
			}
		}

		// Shift elements above down
		int yIndex = (int)matchGroup.MinIndex.y - 1;
		while (yIndex >= 0) { //swaps elements due to gravity
			for (int x = (int)matchGroup.MinIndex.x; x <= matchGroup.MaxIndex.x; x++) {
				Vector2 index1 = new Vector2(x, yIndex);
				Vector2 index2 = new Vector2(x, yIndex + matchGroup.Height);
				SwapGridContents(index1, index2);
			}
			yIndex--;
		}
		Vector2 topLeftIndex = new Vector2(matchGroup.MinIndex.x, 0);
		Vector2 bottomRightIndex = new Vector2(matchGroup.MaxIndex.x, matchGroup.Height - 1);
		
		ElementGroup groupToFill = new ElementGroup(topLeftIndex, bottomRightIndex);

		// Re-populate level
		FillInArea(groupToFill);

		matchManager.CheckMatches();
	}

	private void FillInArea(ElementGroup group) {
		for (int y = (int)group.MaxIndex.y; y >= group.MinIndex.y; y--) {
			for (int x = (int)group.MinIndex.x; x <= group.MaxIndex.x; x++) {
				Element element = GenerateElement();
				float xPosition = (x - halfWidth) * cellWidth;
				element.transform.position = new Vector3(xPosition, spawnHeight, 0);
				gameBoard[x, y].Content = element;
			}
		}
	}

	public void AbsorbMatch(string guid) {
		matchManager.AbsorbMatch(guid);
	}

	// Swaping logic
	public void PerformSwap(UserSelection.SWAP_DIRECTIONS direction, Vector2 minIndex, Vector2 maxIndex) {
		switch(direction) {
			case UserSelection.SWAP_DIRECTIONS.NORTH_SOUTH: {
				PerformNorthSouthSwap(minIndex, maxIndex);
				break;
			}
			case UserSelection.SWAP_DIRECTIONS.EAST_WEST: {
				PerformEastWestSwap(minIndex, maxIndex);
				break;
			}
			case UserSelection.SWAP_DIRECTIONS.NORTH_EAST_SOUTH_WEST: {
				PerformNorthEastSouthWestSwap(minIndex, maxIndex);
				break;
			}
			case UserSelection.SWAP_DIRECTIONS.SOUTH_EAST_NORTH_WEST: {
				PerformSouthEastNorthWestSwap(minIndex, maxIndex);
				break;
			}
		}

		matchManager.CheckMatches();
	}

	private void SwapGridContents(Vector2 index1, Vector2 index2) {
		Element temp = gameBoard[(int)index1.x, (int)(index1.y)].Content;
		gameBoard[(int)index1.x, (int)(index1.y)].Content = gameBoard[(int)index2.x, (int)(index2.y)].Content;
		gameBoard[(int)index2.x, (int)(index2.y)].Content = temp;

		if (gameBoard[(int)index1.x, (int)(index1.y)].IsInMatch) {
			matchManager.RemoveMatch(gameBoard[(int)index1.x, (int)(index1.y)].MatchGuid);
		}

		if (gameBoard[(int)index2.x, (int)(index2.y)].IsInMatch) {
			matchManager.RemoveMatch(gameBoard[(int)index2.x, (int)(index2.y)].MatchGuid);
		}
	}

	public void PerformNorthSouthSwap(Vector2 minIndex, Vector2 maxIndex) {
		int xCounter = 0;
		int yCounter = 0;
		int height = (int)(maxIndex.y - minIndex.y) + 1;

		for (int y = (int) minIndex.y; y <= minIndex.y + (height / 2) - 1; y++) {
			for (int x = (int) minIndex.x; x <= maxIndex.x; x++) {
				SwapGridContents(new Vector2(x,y), new Vector2(x, maxIndex.y - yCounter));
				xCounter++;
			}
			yCounter++;
		}
	}

	public void PerformEastWestSwap(Vector2 minIndex, Vector2 maxIndex) {
		int xCounter = 0;
		int yCounter = 0;
		int width = (int)(maxIndex.x - minIndex.x) + 1;
		
		for (int x = (int) minIndex.x; x <= minIndex.x + (width / 2) - 1; x++) {
			for (int y = (int) minIndex.y; y <= maxIndex.y; y++) {
				SwapGridContents(new Vector2(x,y), new Vector2(maxIndex.x - xCounter, y));
				yCounter++;
			}
			xCounter++;
		}
	}

	public void PerformNorthEastSouthWestSwap(Vector2 minIndex, Vector2 maxIndex) {
		int counter = 0;
		
		for (int y = (int) minIndex.y; y <= maxIndex.y; y++) {
			for (int x = (int) maxIndex.x; x >= minIndex.x + counter; x--) {
				SwapGridContents(new Vector2(x,y), new Vector2(minIndex.x + (y - minIndex.y), maxIndex.y - (maxIndex.x - x)));
			}
			counter++;
		}
	}

	public void PerformSouthEastNorthWestSwap(Vector2 minIndex, Vector2 maxIndex) {
		int counter = 0;

		for (int y = (int) minIndex.y; y <= maxIndex.y; y++) {
			for (int x = (int) minIndex.x; x <= maxIndex.x - counter; x++) {
				SwapGridContents(new Vector2(x,y), new Vector2(maxIndex.x - counter, maxIndex.y - (x - minIndex.x)));
			}
			counter++;
		}
	}
}
