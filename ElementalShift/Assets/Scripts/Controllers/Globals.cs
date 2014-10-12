using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

/**
 * Globals Singleton
 * @author  Dan Sternfeld, Eric Heaney
 * @version 1.0, June 29, 2013
 */
public class Globals : MonoBehaviour {
	
	public static Globals Instance { get; private set; }
	public Dictionary<System.Type, IController> Controllers { get; private set; }
	public static bool IsInit { get; private set; }
	
	//public event Action OnInit = delegate { };
	
	#region game values
	
	public const float ELEMENT_WIDTH = 1.0f;
		
	#endregion
	
	public T GetController<T>() where T : IController {
		return (T)Controllers[typeof(T)];	
	}
	
	/** Create singleton */
	private void Awake() {
		if (Instance != null) {
			throw new System.Exception("Cannot Instantiate the ApplicationController more than once");	
		}
		Instance = this;
		
		Init();
	}
	
	/** Build dictionary of controllers and call their inits */
	private void Init() {
		Component[] controllersHolder = gameObject.GetComponentsInChildren(typeof(IController));
		Controllers = new Dictionary<Type, IController>();
		
		foreach (Component controller in controllersHolder) {
			Controllers.Add(controller.GetType(), controller as IController);
        }
		
		IsInit = true;
		
		foreach (Component controller in controllersHolder) {
			(controller as IController).Init();
        }
	}
}
