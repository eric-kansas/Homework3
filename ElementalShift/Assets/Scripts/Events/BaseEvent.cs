using UnityEngine;
using System.Collections;

/**
 * BaseEvent 
 * @author  Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class BaseEvent {
	public string Name { get; protected set; }
	public GameObject Source { get; set; }
			
	public BaseEvent(GameObject source){
		Name = "BaseEvent";
		Source = source;
	}
}