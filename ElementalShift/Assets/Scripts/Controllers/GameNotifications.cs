using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * The GameNotifications controls communication between gameobjects and controllers. 
 * @author  Dan Sternfeld, Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class GameNotifications : MonoBehaviour, IController {
	
	public event Action OnRestartLevel = delegate { };
	public void RaiseRestartLevel() { OnRestartLevel(); }
	
	public void Init () {
		
	}
}
