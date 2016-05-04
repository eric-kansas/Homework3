using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BoardController : Controller {

	private string prefabsPath = "Prefabs/";
	private string spritesPath = "Sprites/";
	private string elementPrefabPath = "Prefabs/Elements/";
	
	private bool running = false;
	
	private GameObject boardGameObject;

	private int _width;
	public int Width {
		get {
			return _width;
		} 
	}

	private int _height;
	public int Height {
		get {
			return _height;
		} 
	}

	// fix thesse
	private float cellWidth;
	private float cellHeight;

	private Vector3 _cellSize;
	public Vector3 CellSize {
		get {
			return _cellSize;
		} 
	}
	
	private float cellPadding = 0.1f;

	[SerializeField]
	private string levelData;

	private GridSlotContent[] elementPrefabs;
	private Sprite[] slotBackgroundSprites;

	public MatchManager matchManager = new MatchManager();
	private ContentSpawner spawner;

	public GridSlot[,] gameBoard;

	private float halfWidth;
	private float halfHeight;

	private float spawnHeight = 0.0f;

	void Awake() {
		// Load resources
		slotBackgroundSprites = Resources.LoadAll<Sprite>(spritesPath + "GridSlot/");
		elementPrefabs = (GridSlotContent[]) Resources.LoadAll<GridSlotContent>(elementPrefabPath);

		spawner = gameObject.GetComponentInChildren<ContentSpawner>();

		if (spawner == null) {
			Debug.LogError("Missing Content spawner on BoardManager");
		}

		boardGameObject = new GameObject();
		boardGameObject.name = "GameBoard";
		boardGameObject.transform.position = new Vector3(0,0,10);
	}

	override public void Init () {
		base.Init();
		//Resources.LoadAll(prefabsPath);

		// Load Level
		string[,] level = readLevel(levelData);
			
		_width = level.GetLength(0);
		_height = level.GetLength(1);

		gameBoard = new GridSlot[_width, _height];

		cellWidth = (GridSlotContent.ELEMENT_WIDTH + cellPadding);
		cellHeight = (GridSlotContent.ELEMENT_HEIGHT + cellPadding);
		_cellSize = new Vector3(cellWidth, cellHeight, 0);

		halfWidth = (_width / 2);
		halfHeight = (_height / 2);

		spawnHeight = cellHeight * (_height - halfHeight + 1);

		for (int yIndex = 0; yIndex < _height; yIndex++) {
			for (int xIndex = 0; xIndex < _width; xIndex++) {
				Vector2 currentIndex = new Vector2(xIndex, yIndex);

				// Get next grid slot position
				float xPosition = (xIndex - halfWidth) * cellWidth;
				float yPosition = (yIndex - halfHeight) * cellHeight * -1;

				if (_width % 2 == 0) {
					xPosition += (cellWidth / 2);
				}

				if (_height % 2 == 0) {
					yPosition -= (cellHeight / 2);
				}

				Vector3 position = new Vector3(xPosition, yPosition, 0);

				// Build grid slot
				GridSlot slot = (GridSlot) Instantiate(Resources.Load(prefabsPath + "GridSlot", typeof(GridSlot)), position, Quaternion.identity);
				slot.Init(currentIndex, slotBackgroundSprites[0]);
				slot.transform.parent = boardGameObject.transform;

				GridSlotContent randomElement = null;

				// Generate element from level data
				if (level[xIndex, yIndex] == "0") {
					slot.CanHaveContent = false;
					slot.BackgroundView.sprite = null;
				} else if (level[xIndex, yIndex] == "2") {
					slot.CanHaveContent = false;
					slot.IsSelectable = false;
					slot.BackgroundView.sprite = slotBackgroundSprites[1];
				} else {
					//randomElement = GenerateGridSlotContent();
					randomElement = spawner.Pull();
					randomElement.transform.position = position;
					slot.Content = randomElement;
				}

				gameBoard[xIndex, yIndex] = slot;
			}
		}
		running = true;
		matchManager.CheckMatches();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Game Controller Reset
	public void Reset() {
		running = true;	
	}

	public void Stop() {
		running = false;
	}

	
	// Creates a random element from elementPrefabs
	private GridSlotContent GenerateGridSlotContent() {
		int rand = UnityEngine.Random.Range(0, elementPrefabs.Length);
		string elementType = elementPrefabs[rand].name;
		GridSlotContent gridElement = (GridSlotContent) Instantiate(Resources.Load(elementPrefabPath + elementType, typeof(GridSlotContent)), new Vector3(), Quaternion.identity);

		return gridElement;
	}
	
	public GridSlot GetClosestGridSlotToPoint(Vector3 position) {
		Ray ray = Camera.main.ScreenPointToRay(position);    
		Vector3 point = ray.origin + (ray.direction * (Camera.main.transform.position.z - 1));

		float xPosition;
		if (_width % 2 == 0) {
			xPosition = (point.x / cellWidth) + halfWidth;
		} else {
			xPosition = (point.x / cellWidth) + halfWidth + (cellWidth / 2);
		}

		int xIndex = (int)xPosition;

		float yBoardPosition;
		if (_height % 2 == 0) {
			yBoardPosition = (point.y / cellHeight) + halfHeight;
		} else {
			yBoardPosition = (point.y / cellHeight) + halfHeight + (cellHeight / 2);
		}

		if (yBoardPosition < 0) {
			return null; 
		}

		int yIndex = (_height - 1) - (int)yBoardPosition;

		// hack: could never figure out what was wrong here
		if (_height % 2 == 0) {
			yIndex = (_height - 1) - (int)(((point.y / cellHeight) + (cellHeight / 2) + halfHeight) - (cellHeight / 2));
		}
		if ( xIndex >= 0 && xIndex < _width && yIndex >= 0 && yIndex < _height){
			GridSlot temp = gameBoard[xIndex, yIndex];
			return temp;
		}
		return null;
	}

	public GridSlot GetGridSlotAtIndex(Vector2 position) {
		if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height) {
			return null;
		}
		return gameBoard[(int)position.x, (int)position.y];
	}

	public GridSlot GetGridSlotAtIndex(int xIndex, int yIndex) {
		if (xIndex < 0 || xIndex >= _width || yIndex < 0 || yIndex >= _height) {
			return null;
		}
		return gameBoard[xIndex, yIndex];
	}

	public void RemoveGroup(GridSlotGroup matchGroup) {
		// Clear GridSlot Content
		for (int y = (int)matchGroup.MinIndex.y; y <= matchGroup.MaxIndex.y; y++) {
			for (int x = (int)matchGroup.MinIndex.x; x <= matchGroup.MaxIndex.x; x++) {
				gameBoard[x, y].ClearContent();
			}
		}

		// Shift elements above down
		for (int x = (int)matchGroup.MinIndex.x; x <= matchGroup.MaxIndex.x; x++) {
			int yIndex = (int)matchGroup.MinIndex.y - 1;
			int nextY = 0;
			while (yIndex >= 0) { //swaps elements due to gravity
				Vector2 movingIndex = new Vector2(x, yIndex - nextY);
				Vector2 newIndex = new Vector2(x, yIndex + matchGroup.Height);

				if (!gameBoard[(int)newIndex.x, (int)(newIndex.y)].CanHaveContent) {
					yIndex--;
					nextY = 0;
					continue;
				}

				if (movingIndex.x < 0 || movingIndex.x >= _width || movingIndex.y < 0 || movingIndex.y >= _height) {
					yIndex--;
					nextY = 0;
					continue;
				}

				if (gameBoard[(int)movingIndex.x, (int)(movingIndex.y)].CanHaveContent && 
				    gameBoard[(int)movingIndex.x, (int)(movingIndex.y)].Content != null) {
					SwapGridContents(movingIndex, newIndex);
					yIndex--;
					nextY = 0;
				} else {
					nextY++;
				}
			}
		}
		
		Vector2 topLeftIndex = new Vector2(matchGroup.MinIndex.x, 0);
		Vector2 bottomRightIndex = new Vector2(matchGroup.MaxIndex.x, matchGroup.MaxIndex.y);
		
		GridSlotGroup groupToFill = new GridSlotGroup(topLeftIndex, bottomRightIndex);

		// Re-populate level
		FillInArea(groupToFill);

		matchManager.CheckMatches();
	}

	private void FillInArea(GridSlotGroup group) {
		float yPosition = (halfHeight + 1) * cellHeight;
		if (_height % 2 == 0) {
			yPosition -= (cellHeight / 2);
		}

		for (int y = (int)group.MaxIndex.y; y >= group.MinIndex.y; y--) {
			for (int x = (int)group.MinIndex.x; x <= group.MaxIndex.x; x++) {
				if (gameBoard[x, y].CanHaveContent && gameBoard[x, y].Content == null) {

					float xPosition = (x - halfWidth) * cellWidth;
					
					if (_width % 2 == 0) {
						xPosition += (cellWidth / 2);
					}

					
					Vector3 position = new Vector3(xPosition, yPosition, 0);
					//GridSlotContent element = GenerateGridSlotContent();
					GridSlotContent element = spawner.Pull();
					element.transform.position = position; 
					gameBoard[x, y].Content = element;
				}
			}
		}
	}

	public void AbsorbMatch(string guid) {
		if (running) {
			matchManager.AbsorbMatch(guid);
			matchManager.CheckMatches();
		}
	}

	// Swaping logic
	public void PerformSwap(InputManager.SWAP_DIRECTIONS direction, Vector2 minIndex, Vector2 maxIndex) {


		switch(direction) {
			case InputManager.SWAP_DIRECTIONS.NORTH_SOUTH: {
				PerformNorthSouthSwap(minIndex, maxIndex);
				break;
			}
			case InputManager.SWAP_DIRECTIONS.EAST_WEST: {
				PerformEastWestSwap(minIndex, maxIndex);
				break;
			}
			case InputManager.SWAP_DIRECTIONS.NORTH_EAST_SOUTH_WEST: {
				PerformNorthEastSouthWestSwap(minIndex, maxIndex);
				break;
			}
			case InputManager.SWAP_DIRECTIONS.SOUTH_EAST_NORTH_WEST: {
				PerformSouthEastNorthWestSwap(minIndex, maxIndex);
				break;
			}
		}

		matchManager.CheckMatches();
	}

	private void SwapGridContents(Vector2 index1, Vector2 index2) {
		if (!gameBoard[(int)index1.x, (int)(index1.y)].CanHaveContent || !gameBoard[(int)index2.x, (int)(index2.y)].CanHaveContent) {
			return;
		}
		
		GridSlotContent placeHolderElement = gameBoard[(int)index1.x, (int)(index1.y)].Content;
		gameBoard[(int)index1.x, (int)(index1.y)].Content = gameBoard[(int)index2.x, (int)(index2.y)].Content;
		gameBoard[(int)index2.x, (int)(index2.y)].Content = placeHolderElement;

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

	private string[,] readLevel(string levelString) {
		string[] rows = levelString.Split(new string[] { "\r\n", "\n", "\r", " " }, StringSplitOptions.RemoveEmptyEntries);
		int levelWidth = rows[0].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;

		string[,] level = new string[levelWidth, rows.Length];

		for (int y = 0; y < rows.Length; y++) {
			string[] values = rows[y].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			for (int x = 0; x < values.Length; x++)  {
				level[x,y] = values[x];
			}

		}
	
		return level;
	}

}
