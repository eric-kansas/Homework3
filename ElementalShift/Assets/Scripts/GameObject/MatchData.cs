using UnityEngine;
using System;
using System.Collections;

public class MatchData {
	private string _guid;
	public string MatchGuid {
		get { return _guid; }
		set { _guid = value; }
	}

	private string _type;
	public string Type {
		get { return _type; }
		set { _type = value; }
	}

	private ElementGroup _group;
	public ElementGroup Group {
		get { return _group; }
		set { _group = value; }
	}

	public MatchData(ElementGroup group, string matchType) {
		_group = group;
		_type = matchType;

		_guid = Guid.NewGuid().ToString();

		// Tell the elements in my group that im their match owner
		for (int y = (int)group.MinIndex.y; y <= group.MaxIndex.y; y++) {
			for (int x = (int)group.MinIndex.x; x <= group.MaxIndex.x; x++) {
				BoardManager.gameBoard[x, y].MatchGuid = _guid;
			}
		}
	}

}
