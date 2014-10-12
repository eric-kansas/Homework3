using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * The EventDispatcherBehaviour MonoBehaviour allows objects to fire events and have other classes wire to them. 
 * @author  Dan Sternfeld, Eric Heaney
 * @version 1.0, June 29, 2013
 */
public abstract class EventDispatcherBehaviour : MonoBehaviour {
	
	private Dictionary<Type, object> _handlerDictionary = new Dictionary<Type, object>();
	
	/**
	 * Wire Action<T> to event type T
	 * @params: Action<T> handler 	the delegate that should be added to the event type T's handler list
	 */
	public void WireEvent<T>(Action<T> handler) where T : BaseEvent {
		Type actionType = typeof(T);
		object eventHandlerObject;
		if (_handlerDictionary.TryGetValue(actionType, out eventHandlerObject)) {
			Action<T> eventHandler = (Action<T>) eventHandlerObject + handler;
			_handlerDictionary[typeof(T)] = (object) eventHandler;	
		} else {
			_handlerDictionary[typeof(T)] = (object) handler;	
		}
	}
	
	/**
	 * Unwire Action<T> from event type T
	 * @params: Action<T> handler 	the delegate that should be removed from the event type T's handler list
	 */
	public void UnwireEvent<T>(Action<T> handler) where T : BaseEvent {
		Type actionType = typeof(T);
		object eventHandlerObject;
		if (_handlerDictionary.TryGetValue(actionType, out eventHandlerObject)) {
			Action<T> eventHandler = (Action<T>) eventHandlerObject - handler;
			_handlerDictionary[typeof(T)] = (object) eventHandler;	
		}
	}
	
	/**
	 * Fires an event
	 * Invokes all Action<T> that are currenetly wired to type T
	 * @params: T evt 	the event to fire
	 */
	protected void FireEvent<T>(T evt) where T : BaseEvent {
		Type actionType = typeof(T);
		object eventHandlerObject;
		if (_handlerDictionary.TryGetValue(actionType, out eventHandlerObject)) {
			((Action<T>) eventHandlerObject)(evt);
		}
	}
}
