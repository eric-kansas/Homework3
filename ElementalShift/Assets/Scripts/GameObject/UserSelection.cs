using UnityEngine;
using System.Collections;

public class UserSelection : MonoBehaviour {

	public enum SWAP_DIRECTIONS {
		NORTH,
		NORTH_EAST,
		EAST,
		SOUTH_EAST,
		SOUTH_WEST,
		WEST,
		NORTH_WEST
	};
	
	public BoardManager boardManager;
	public Texture2D selectinRectTexture;

	UserSelectionView selctionView;

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

	Vector3 swipeStartPosition = new Vector3();
	
	void Start () {
		selctionView = new UserSelectionView(gameObject, selectinRectTexture);
	}

	void Update () {
		HandleTouchInput();
		HandleMouseInput();
	}

	private void HandleTouchInput() {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				// Get closet grid slot
				GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
				if ( clickedGridSlot != null) {
					startingIndex = lastGridIndex = minTempSelection = maxTempSelection = clickedGridSlot.BoardIndex;		
					isSelecting = true;
				} else {
					selctionView.Clear();
				}
			} else if (touch.phase == TouchPhase.Moved) {
				// Get closet grid slot
				GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
				if ( clickedGridSlot != null) {
					Vector2 currentBoardIndex = clickedGridSlot.BoardIndex;
					HandleSelection(currentBoardIndex);
				}
			}
		}
	}

	private void HandleMouseInput() {
		
		// Left mouse click
		if (Input.GetMouseButtonDown(0)) {
			
			// Get closet grid slot
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);

			// Swipe mode
			if (madeSelection && GridIndexIsInSelection(clickedGridSlot.BoardIndex)) {
				isSwiping = true;
				swipeStartPosition = Input.mousePosition;
			} else {
				madeSelection = false;
				isSquare = false;
				if ( clickedGridSlot != null) {
					startingIndex = lastGridIndex = minTempSelection = maxTempSelection = clickedGridSlot.BoardIndex;		
					isSelecting = true;
				} else {
					selctionView.Clear();
				}
			}
		} else if (Input.GetMouseButtonUp(0)) {
			if (isSelecting) {
				FinalizeSelection();
			} else if (isSwiping) {
				FinalizeSwipe(Input.mousePosition);
			}
		}
		
		if (isSelecting) {
			// Get closet grid slot
			GridSlot clickedGridSlot = boardManager.GetClosestGridSlotToPoint(Input.mousePosition);
			if ( clickedGridSlot != null) {
				Vector2 currentBoardIndex = clickedGridSlot.BoardIndex;
				HandleSelection(currentBoardIndex);
			}
		}
	}

	private void FinalizeSwipe(Vector3 endPosition) {
		Vector3 delta = swipeStartPosition - endPosition;

		float angle = Vector3.Angle(delta, Vector3.left);
		float sign = Mathf.Sign(Vector3.Dot(delta, Vector3.down));
		
		angle = sign * angle;

		if (isSquare) {
			// East
			if (angle < 22.5f && angle > -22.5f) { // EAST
				Debug.Log("EAST");
				//boardManager.performSwap();
			} else if (angle < -22.5 && angle > -67.5f) { // South East
				Debug.Log("SOUTH EAST");
			} else if (angle < -67.5f && angle > -112.5f) { // South
				Debug.Log("SOUTH");
			} else if (angle < -112.5f && angle > -157.5f) { // South West
				Debug.Log("SOUTH WEST");
			} else if (angle < -157.5f || angle > 157.5f) { // West
				Debug.Log("WEST");
			} else if (angle > 112.5f && angle < 157.5f) { // North West
				Debug.Log("NORTH WEST");
			} else if (angle > 67.5f && angle < 112.5f) { // North
				Debug.Log("NORTH");
				boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH, minSelection, maxSelection);
			} else if (angle > 22.5f && angle < 67.5f) { // North East
				Debug.Log("North East");
			}
		} else {
			if (angle < 45.0f && angle > -45.0f) { // East
				Debug.Log("EAST");
			} else if (angle < -45.0f && angle > -135.0f) { // South
				Debug.Log("SOUTH");
			} else if (angle < -135.0f || angle > 135.0f) { // West
				Debug.Log("WEST");
			} else if (angle > 45.0f && angle < 135.0f) { // North 
				Debug.Log("NORTH");
				boardManager.PerformSwap(SWAP_DIRECTIONS.NORTH, minSelection, maxSelection);
			}
		}
	}

	private void FinalizeSelection() {
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

	private void HandleSelection(Vector2 currentBoardIndex) {
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
		
		
		Vector3 topLeftPosition = boardManager.GetGridSlotAtIndex(minTempSelection).Boundary.min;
		Vector3 bottomRightPosition = boardManager.GetGridSlotAtIndex(maxTempSelection).Boundary.max;
		
		selctionView.updateSelection(topLeftPosition, bottomRightPosition);
		
		lastGridIndex = currentBoardIndex;
	}
}
