using UnityEngine;
using System.Collections;

/**
 * PlayerEvent is used to pass events related to the player
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class PlayerEvent : BaseEvent {
	public Vector3 Position { get; set; }
	
	public PlayerEvent(GameObject source) : base(source) {
		Name = "PlayerEvent";
		Position = source.transform.position;
	}
}