using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MatchManager {

	public static Dictionary<string, MatchGroup> matches = new Dictionary<string, MatchGroup>();
	
	public MatchManager() {

	}

	public void CheckMatches() {
		for (int y = 0; y < BoardManager.height; y++) {
			for (int x = 0; x < BoardManager.width; x++) {
				ElementGroup match = GetBestSquare(x,y);
				if (match != null && match.Width >= 2) {
					if (CheckValidMatch(match)) {
						SetMatch(x, y, match);
					}
				}
			}
		}
	}

	private bool CheckValidMatch(ElementGroup match) {
		for (int y = (int)match.MinIndex.y; y <= match.MaxIndex.y; y++) {
			for (int x = (int)match.MinIndex.x; x <= match.MaxIndex.x; x++) {
				// if this GridSlot is currently in a match make sure that it is bigger
				if (BoardManager.gameBoard[x, y].IsInMatch) {
					if (matches.ContainsKey(BoardManager.gameBoard[x, y].MatchGuid)) {
						if (match.Width <= matches[BoardManager.gameBoard[x, y].MatchGuid].Data.Group.Width) {
							return false;
						}
					}
				}
			}
		}

		ClearMatchForGroup(match);
		return true;
	}

	private void ClearMatchForGroup(ElementGroup group) {
		for (int y = (int)group.MinIndex.y; y <= group.MaxIndex.y; y++) {
			for (int x = (int)group.MinIndex.x; x <= group.MaxIndex.x; x++) {
				if (BoardManager.gameBoard[x, y].IsInMatch && matches.ContainsKey(BoardManager.gameBoard[x, y].MatchGuid)) {
					matches[BoardManager.gameBoard[x, y].MatchGuid].Clear();
				}
			}
		}
	}
	
	private ElementGroup GetBestSquare(int startX, int startY) {
		string patternMatchType = BoardManager.gameBoard[startX, startY].GetContentType();
		if (String.IsNullOrEmpty(patternMatchType)) {
			return null;
		}

		int tempX = startX;
		int tempY = startY;
		int potentailSize = 1;

		do { //for each element check its diagonal
			if (tempX + 1 < BoardManager.width && tempY + 1 < BoardManager.height) {
				if (BoardManager.gameBoard[tempX, tempY].GetContentType() == (BoardManager.gameBoard[tempX + 1, tempY + 1]).GetContentType()) {
					tempX++;
					tempY++;
					potentailSize++;
				} else {
					break;
				}
			} else {
				break;
			}
		} while(true);

		int currentSquareSize = 1;
		int patternMatchSize = 1;

		ElementGroup match = new ElementGroup(new Vector2(startX, startY));
		while (potentailSize >= currentSquareSize && currentSquareSize > 0) {
			//check corners
			if (patternMatchType == (BoardManager.gameBoard[startX + currentSquareSize - 1, startY].GetContentType()) &&
			    patternMatchType == (BoardManager.gameBoard[startX, startY + currentSquareSize - 1].GetContentType())) {

				if (currentSquareSize == 2) { // checked enough for 2x2
					patternMatchSize = 2;
					currentSquareSize++;
				} else { //looking bigger than 2x2 check y and x inbetweens
					GridSlot tempGridSlot;
					//walk down y
					for (int i = 1; i < currentSquareSize-1; i++) {
						tempGridSlot = BoardManager.gameBoard[startX + i, startY + currentSquareSize -1];
						if (patternMatchType != tempGridSlot.GetContentType()) {
							currentSquareSize = -1;
							break;
						}
					}

					//walk across x
					for (int i = 1; i < currentSquareSize-1; i++) {
						tempGridSlot = BoardManager.gameBoard[startX + currentSquareSize - 1, startY + i];
						if (patternMatchType != tempGridSlot.GetContentType()) {
							currentSquareSize = -1;
							break;
						}
					}

					//if we passed the test -- put it up and anty
					if (currentSquareSize > 0) {
						patternMatchSize = currentSquareSize;
						currentSquareSize++;
					}
				}							
			} else {
				//fail
				currentSquareSize = -1;
			}
		}
		match.MaxIndex = new Vector2(startX + (patternMatchSize) - 1, startY + (patternMatchSize) - 1);
		return match;
	}

	public void SetMatch(int startX, int startY, ElementGroup match) {
		string matchType = BoardManager.gameBoard[startX, startY].GetContentType();
		MatchData matchData = new MatchData(match, matchType);

		Vector2 bottomLeftIndex = new Vector2(startX, (startY + match.Height - 1));
		Vector2 topRightIndex = new Vector2(startX + match.Width - 1, startY);

		Vector3 bottomLeftPosition = BoardManager.GetGridSlotAtIndex(bottomLeftIndex).Boundary.min;
		Vector3 topRightPosition = BoardManager.GetGridSlotAtIndex(topRightIndex).Boundary.max;
		
		matches.Add(matchData.MatchGuid, new MatchGroup(matchData, bottomLeftPosition, topRightPosition));
	}
	
	public void RemoveMatch(string guid) {
		if (matches.ContainsKey(guid)) {
			matches[guid].Clear();
			matches.Remove(guid);
		}
	}

	public void AbsorbMatch(string guid) {
		if (matches.ContainsKey(guid)) {
			matches[guid].Absorb();
			RemoveMatch(guid);
		}
	}
}
