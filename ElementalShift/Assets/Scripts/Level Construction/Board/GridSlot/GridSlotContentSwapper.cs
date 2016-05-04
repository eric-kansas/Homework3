using UnityEngine;
using System.Collections;

public class GridSlotContentSwapper {

	private static void SwapGridContents(GridSlot slot1, GridSlot slot2) {
		// Make sure both GridSlot can have content in it
		if (!slot1.CanHaveContent || !slot2.CanHaveContent) {
			return;
		}

		// Swap content of grid slots
		GridSlotContent placeHolderElement = slot1.Content;
		slot1.Content = slot2.Content;
		slot2.Content = placeHolderElement;
	}
	
	public static void PerformNorthSouthSwap(ref GridSlot[,] boardArray, Point minIndex, Point maxIndex) {
		int xCounter = 0;
		int yCounter = 0;
		int height = (maxIndex.y - minIndex.y) + 1;
		
		for (int y = minIndex.y; y <= minIndex.y + (height / 2) - 1; y++) {
			for (int x = minIndex.x; x <= maxIndex.x; x++) {
				SwapGridContents(boardArray[x,y], boardArray[x, maxIndex.y - yCounter]);
				xCounter++;
			}
			yCounter++;
		}
	}

	public static void PerformEastWestSwap(ref GridSlot[,] boardArray, Point minIndex, Point maxIndex) {
		int xCounter = 0;
		int yCounter = 0;
		int width = (maxIndex.x - minIndex.x) + 1;
		
		for (int x =  minIndex.x; x <= minIndex.x + (width / 2) - 1; x++) {
			for (int y =  minIndex.y; y <= maxIndex.y; y++) {
				SwapGridContents(boardArray[x,y], boardArray[maxIndex.x - xCounter, y]);
				yCounter++;
			}
			xCounter++;
		}
	}

	public static void PerformNorthEastSouthWestSwap(ref GridSlot[,] boardArray, Point minIndex, Point maxIndex) {
		int counter = 0;
		
		for (int y =  minIndex.y; y <= maxIndex.y; y++) {
			for (int x =  maxIndex.x; x >= minIndex.x + counter; x--) {
				SwapGridContents(boardArray[x,y], boardArray[minIndex.x + (y - minIndex.y), maxIndex.y - (maxIndex.x - x)]);
			}
			counter++;
		}
	}
}
