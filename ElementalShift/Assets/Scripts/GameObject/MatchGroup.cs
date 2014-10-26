using UnityEngine;
using System.Collections;

public class MatchGroup {

	private MatchData _matchData;
	public MatchData Data {
		get { return _matchData; }
		set { _matchData = value; }
	}
	private MatchGroupView _view;

	public MatchGroup(MatchData data, Vector3 bottomLeftPosition, Vector3 topRightPosition) {
		_matchData = data;
		_view = new MatchGroupView(bottomLeftPosition, topRightPosition);
	}

	public void Clear() {
		// Clear GridSlot match data
		for (int y = (int)_matchData.Group.MinIndex.y; y <= _matchData.Group.MaxIndex.y; y++) {
			for (int x = (int)_matchData.Group.MinIndex.x; x <= _matchData.Group.MaxIndex.x; x++) {
				BoardManager.gameBoard[x, y].MatchGuid = null;
			}
		}

		// Destory view
		_view.Destory();
	}

	public void Absorb() {
		BoardManager.GetInstance.RemoveGroup(_matchData.Group);
		// Destory view
		_view.Destory();
	}
}
