using UnityEngine;
using System.Collections;

public class UserSelection : MonoBehaviour {

	public enum SWAP_DIRECTIONS {
		NORTH_SOUTH,
		NORTH_EAST_SOUTH_WEST,
		EAST_WEST,
		SOUTH_EAST_NORTH_WEST
	};

	private const float SWIPE_LENGTH = 20.0f;
	public bool resetSelectionAfterSwipe = true;

	public Texture2D selectinRectTexture;

	private UserSelectionView selctionView;

	Bounds selectionBounds = new Bounds();
	Vector2 minTempSelection = new Vector2();
	Vector2 maxTempSelection = new Vector2();

	Vector2 minSelection = new Vector2();
	Vector2 maxSelection = new Vector2();

	Vector2 startingIndex = new Vector2();
	Vector2 lastGridIndex = new Vector2();

	bool isSelecting = false;
	bool madeSelection = false;
	bool isSwiping = false;
	bool isSquare = false;
	bool isValidSelection = false;

	int selctionTapCount = 0;

	Vector3 swipeStartPosition = new Vector3();
	public bool running = false;
	
	void Awake () {
		running = true;
		selctionView = new UserSelectionView(gameObject, selectinRectTexture);
	}

	void Update () {
		//HandleTouchInput();
		if (GameRound.running != running) {
			running = GameRound.running;
		}
		if (running) {
			HandleMouseInput();
		}
	}

	/*
	private void HandleTouchInput() {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				// Get closet grid slot
				GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(touch.position);
				Debug.Log(clickedGridSlot.BoardIndex);
				if ( clickedGridSlot != null) {
					if (madeSelection && GridIndexIsInSelection(clickedGridSlot.BoardIndex)) {
						isSwiping = true;
						swipeStartPosition = touch.position;
					} else {
						startingIndex = lastGridIndex = minTempSelection = maxTempSelection = clickedGridSlot.BoardIndex;		
						isSelecting = true;
						Vector2 currentBoardIndex = clickedGridSlot.BoardIndex;
						HandleSelection(currentBoardIndex);
					}
				} else {
					ResetSelectionState();
				}
			} else if (touch.phase == TouchPhase.Moved) {
				// Get closet grid slot
				GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
				if ( clickedGridSlot != null) {
					if (isSelecting) {
						Vector2 currentBoardIndex = clickedGridSlot.BoardIndex;
						HandleSelection(currentBoardIndex);
					}
				}
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				if (isSelecting) {
					FinalizeSelection();
				} else if (isSwiping) {
					FinalizeSwipe(touch.position);
				}
			}
		}
	}
	*/

	private void HandleMouseInput() {
		// Left mouse click
		if (Input.GetMouseButtonDown(0)) {
			// Get closet grid slot to mouse
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
			if (clickedGridSlot != null) {
				if (madeSelection && GridIndexIsInSelection(clickedGridSlot.BoardIndex)) {
					// Swipe mode
					isSwiping = true;
					swipeStartPosition = Input.mousePosition;
				} else {
					//New Selection
					madeSelection = false;
					isSquare = false;
					isSelecting = true;
					startingIndex = lastGridIndex = minTempSelection = maxTempSelection = clickedGridSlot.BoardIndex;
				}
			} else {
				ResetSelectionState();
			}
		} else if (Input.GetMouseButtonUp(0)) {
			if (isSelecting) {
				FinalizeSelection();
			} else if (isSwiping) {
				FinalizeSwipe(Input.mousePosition);
			}
		}
		
		if (isSelecting) {
			// Get closet grid slot to mouse
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
			if (clickedGridSlot != null) {
				Vector2 currentBoardIndex = clickedGridSlot.BoardIndex;
				HandleSelection(currentBoardIndex);
			}
		}
	}

	private void ResetSelectionState() {
		selctionView.Clear();
		madeSelection = false;
		isSwiping = false;
		isSelecting = false;
		isSquare = false;
		selctionTapCount = 0;
	}
	
	private void FinalizeSwipe(Vector3 endPosition) {
		Vector3 delta = swipeStartPosition - endPosition;

		// Not long enough for swipe -- treat as tap / click
		if (delta.magnitude < SWIPE_LENGTH) {
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
			// if clicked grid slot is in a match absorb it
			if (clickedGridSlot != null && clickedGridSlot.IsInMatch) {;
				selctionTapCount++;
				if (selctionTapCount == 1) {
					boardManager.AbsorbMatch(clickedGridSlot.MatchGuid);
					ResetSelectionState();
				}
			}
		} else {
			float angle = Vector3.Angle(delta, Vector3.left);
			float sign = Mathf.Sign(Vector3.Dot(delta, Vector3.down));
			
			angle = sign * angle;
			if (isSquare) {
				// East
				if (angle < 22.5f && angle > -22.5f) { // EAST
					Debug.Log("EAST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.EAST_WEST, minSelection, maxSelection);
				} else if (angle < -22.5 && angle > -67.5f) { // South East
					Debug.Log("SOUTH EAST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.SOUTH_EAST_NORTH_WEST, minSelection, maxSelection);
				} else if (angle < -67.5f && angle > -112.5f) { // South
					Debug.Log("SOUTH");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_SOUTH, minSelection, maxSelection);
				} else if (angle < -112.5f && angle > -157.5f) { // South West
					Debug.Log("SOUTH WEST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_EAST_SOUTH_WEST, minSelection, maxSelection);
				} else if (angle < -157.5f || angle > 157.5f) { // West
					Debug.Log("WEST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.EAST_WEST, minSelection, maxSelection);
				} else if (angle > 112.5f && angle < 157.5f) { // North West
					Debug.Log("NORTH WEST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.SOUTH_EAST_NORTH_WEST, minSelection, maxSelection);
				} else if (angle > 67.5f && angle < 112.5f) { // North
					Debug.Log("NORTH");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_SOUTH, minSelection, maxSelection);
				} else if (angle > 22.5f && angle < 67.5f) { // North East
					Debug.Log("North East");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_EAST_SOUTH_WEST, minSelection, maxSelection);
				}
			} else {
				if (angle < 45.0f && angle > -45.0f) { // East
					Debug.Log("EAST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.EAST_WEST, minSelection, maxSelection);
				} else if (angle < -45.0f && angle > -135.0f) { // South
					Debug.Log("SOUTH");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_SOUTH, minSelection, maxSelection);
				} else if (angle < -135.0f || angle > 135.0f) { // West
					Debug.Log("WEST");
					boardManager.PerformSwap(SWAP_DIRECTIONS.EAST_WEST, minSelection, maxSelection);
				} else if (angle > 45.0f && angle < 135.0f) { // North 
					Debug.Log("NORTH");
					boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH_SOUTH, minSelection, maxSelection);
				}
			}

			if (resetSelectionAfterSwipe) {
				ResetSelectionState();
			}
		}
	}

	private void FinalizeSelection() {
		if (!isValidSelection) {
			ResetSelectionState();
			return;
		}

		// Handle tap a match -- auto select match for user
		if (minTempSelection == maxTempSelection) {
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
			// TODO: fix logic
			if (clickedGridSlot != null && clickedGridSlot.IsInMatch) {
				MatchGroup match = MatchManager.matches[clickedGridSlot.MatchGuid];

				Vector2 tempMin = match.Data.Group.MinIndex;
				Vector2 tempMax = match.Data.Group.MaxIndex;

				// New Selection
				startingIndex = lastGridIndex = minTempSelection = maxTempSelection = tempMin;
				HandleSelection(tempMax);
			} else {
				ResetSelectionState();
				return;
			}
		}
		isSelecting = false;
		madeSelection = true;

		minSelection = minTempSelection;
		maxSelection = maxTempSelection;
		
		int selectionWidth = (int)(maxSelection.x - minSelection.x);
		int selectionHeight = (int)(maxSelection.y - minSelection.y);
		if (selectionWidth == selectionHeight) {
			isSquare = true;
		}
	}
	
	private bool GridIndexIsInSelection(Vector2 gridIndex) {
		return gridIndex.x >= minSelection.x && gridIndex.x <= maxSelection.x
			&& gridIndex.y >= minSelection.y && gridIndex.y <= maxSelection.y;

	}

	// Determines the current selection rectangles min and max and updates selection view
	private void HandleSelection(Vector2 currentBoardIndex) {

		// Determine X bounds
		if (currentBoardIndex.x < minTempSelection.x) {
			minTempSelection.x = currentBoardIndex.x;
		} else if (currentBoardIndex.x > maxTempSelection.x) {
			maxTempSelection.x = currentBoardIndex.x;
		} else if (currentBoardIndex.x < startingIndex.x) {
			minTempSelection.x = currentBoardIndex.x;
		} else if (currentBoardIndex.x > startingIndex.x) {
			maxTempSelection.x = currentBoardIndex.x;
		} else {
			minTempSelection.x = maxTempSelection.x = currentBoardIndex.x;
		}

		// Determine Y bounds
		if (currentBoardIndex.y < minTempSelection.y) {
			minTempSelection.y = currentBoardIndex.y;
		} else if (currentBoardIndex.y > maxTempSelection.y) {
			maxTempSelection.y = currentBoardIndex.y;
		} else if (currentBoardIndex.y < startingIndex.y) {
			minTempSelection.y = currentBoardIndex.y;
		} else if (currentBoardIndex.y > startingIndex.y) {
			maxTempSelection.y = currentBoardIndex.y;
		} else {
			minTempSelection.y = maxTempSelection.y = currentBoardIndex.y;
		}

		// Board reads top-bottom from left to right -- math reads bottom to top left to right
		Vector2 bottomLeftIndex = new Vector2(minTempSelection.x, maxTempSelection.y);
		Vector2 topRightIndex = new Vector2(maxTempSelection.x, minTempSelection.y);
		Vector3 bottomLeftPosition = BoardManager.GetGridSlotAtIndex(bottomLeftIndex).Boundary.min;
		Vector3 topRightPosition = BoardManager.GetGridSlotAtIndex(topRightIndex).Boundary.max;

		isValidSelection = CheckSelectionValidity(minTempSelection, maxTempSelection);
		
		selctionView.UpdateSelection(bottomLeftPosition, topRightPosition, isValidSelection);
		
		lastGridIndex = currentBoardIndex;
	}

	private bool CheckSelectionValidity(Vector3 minPosition, Vector3 maxPosition) {
		for (int y = (int)minPosition.y; y <= maxPosition.y; y++) {
			for (int x = (int)minPosition.x; x <= maxPosition.x; x++) {
				if (!BoardManager.gameBoard[x, y].IsSelectable) {
					return false;
				}
			}
		}
		
		return true;
	}
}
