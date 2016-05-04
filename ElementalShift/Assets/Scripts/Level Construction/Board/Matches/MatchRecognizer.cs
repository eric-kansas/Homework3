using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MatchRecognizer : MonoBehaviour {

	public static Dictionary<string, MatchGroup> matches = new Dictionary<string, MatchGroup>();

	// A delegate type for hooking up match removed notifications.
	public delegate void OnMatchRemovedHandler(MatchData matchData);
	public event OnMatchRemovedHandler OnMatchRemoved;
	
	public void CheckMatches() {
		// Loop through board checking for square matches
		for (int y = 0; y < GameController.Board.Height; y++) {
			for (int x = 0; x < GameController.Board.Width; x++) {
				GridSlotGroup match = GetBestSquare(x,y);
				if (match != null && match.Width >= 2) {
					if (CheckValidMatch(match)) {
						SetMatch(x, y, match);
					}
				}
			}
		}
	}

	private bool CheckValidMatch(GridSlotGroup match) {
		for (int y = (int)match.MinIndex.y; y <= match.MaxIndex.y; y++) {
			for (int x = (int)match.MinIndex.x; x <= match.MaxIndex.x; x++) {
				// if this GridSlot is currently in a match make sure that this match is bigger
				if (GameController.Board.gameBoard[x, y].IsInMatch) {
					if (matches.ContainsKey(GameController.Board.gameBoard[x, y].MatchGuid)) {
						if (match.Width <= matches[GameController.Board.gameBoard[x, y].MatchGuid].Data.Group.Width) {
							return false;
						}
					}
				}
			}
		}

		ClearMatchForGroup(match);
		return true;
	}

	private void ClearMatchForGroup(GridSlotGroup group) {
		for (int y = (int)group.MinIndex.y; y <= group.MaxIndex.y; y++) {
			for (int x = (int)group.MinIndex.x; x <= group.MaxIndex.x; x++) {
				if (GameController.Board.gameBoard[x, y].IsInMatch && matches.ContainsKey(GameController.Board.gameBoard[x, y].MatchGuid)) {
					matches[GameController.Board.gameBoard[x, y].MatchGuid].Clear();
				}
			}
		}
	}

	/*
	 * Finds best sqaure of similar type from starting point
	 */
	private GridSlotGroup GetBestSquare(int startX, int startY) {
		// Get pattern match type for this match
		GridSlotContent.ContentType patternMatchType = GameController.Board.gameBoard[startX, startY].GetContentType();

		int tempX = startX;
		int tempY = startY;
		int potentailSize = 1;

		// Loop diagonally down and right from starting index
		do {
			if (tempX + 1 < GameController.Board.Width && tempY + 1 < GameController.Board.Height) {
				// The diagonal is the same type as our starting type
				if (GameController.Board.gameBoard[tempX, tempY].GetContentType() != null &&
				    GameController.Board.gameBoard[tempX, tempY].GetContentType() == (GameController.Board.gameBoard[tempX + 1, tempY + 1]).GetContentType()) {
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

		GridSlotGroup match = new GridSlotGroup(new Vector2(startX, startY));
		while (potentailSize >= currentSquareSize && currentSquareSize > 0) {
			// check that corners of this square selection against match type
			if (patternMatchType == (GameController.Board.gameBoard[startX + currentSquareSize - 1, startY].GetContentType()) &&
			    patternMatchType == (GameController.Board.gameBoard[startX, startY + currentSquareSize - 1].GetContentType())) {

				if (currentSquareSize != 2) { 
					// Bigger than 2x2 check y and x inbetweens
					GridSlot tempGridSlot;

					// Walk down y and check against match type
					for (int i = 1; i < currentSquareSize-1; i++) {
						tempGridSlot = GameController.Board.gameBoard[startX + i, startY + currentSquareSize - 1];
						if (patternMatchType != tempGridSlot.GetContentType()) {
							currentSquareSize = -1;
							break;
						}
					}

					// Walk across x and check against match type
					for (int i = 1; i < currentSquareSize - 1; i++) {
						tempGridSlot = GameController.Board.gameBoard[startX + currentSquareSize - 1, startY + i];
						if (patternMatchType != tempGridSlot.GetContentType()) {
							currentSquareSize = -1;
							break;
						}
					}
				}

				// If we passed the test -- put it up and anty
				if (currentSquareSize > 0) {
					patternMatchSize = currentSquareSize;
					currentSquareSize++;
				}
						
			} else {
				//fail
				currentSquareSize = -1;
			}
		}
		match.MaxIndex = new Vector2(startX + (patternMatchSize) - 1, startY + (patternMatchSize) - 1);
		return match;
	}

	public void SetMatch(int startX, int startY, GridSlotGroup match) {
		GridSlotContent.ContentType matchType = GameController.Board.gameBoard[startX, startY].GetContentType();
		MatchData matchData = new MatchData(match, matchType);

		//TODO: wtf 
		Vector2 bottomLeftIndex = new Vector2(startX, (startY + match.Height - 1));
		Vector2 topRightIndex = new Vector2(startX + match.Width - 1, startY);

		Vector3 bottomLeftPosition = GameController.Board.GetGridSlotAtIndex(bottomLeftIndex).Boundary.min;
		Vector3 topRightPosition = GameController.Board.GetGridSlotAtIndex(topRightIndex).Boundary.max;
		
		matches.Add(matchData.MatchGuid, new MatchGroup(matchData, bottomLeftPosition, topRightPosition, this.gameObject));
	}
	
	public void RemoveMatch(string guid) {
		if (matches.ContainsKey(guid)) {
			matches[guid].Clear();
			matches.Remove(guid);
		}
	}

	public void AbsorbMatch(string guid) {
		if (matches.ContainsKey(guid)) {
			// Stats track
			string key = matches[guid].Data.Type + " " + matches[guid].Data.Group.Width + " by " + matches[guid].Data.Group.Width;
			if (Stats.matches.ContainsKey(key)) {
				Stats.matches[key] += 1;
			} else {
				Stats.matches.Add(key, 1);
			}

			if (OnMatchRemoved != null) {
				OnMatchRemoved(matches[guid].Data);
			}

			matches[guid].Absorb();
			RemoveMatch(guid);
		}
	}
}
