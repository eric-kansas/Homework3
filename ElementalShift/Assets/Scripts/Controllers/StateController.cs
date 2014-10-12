using UnityEngine;
using System.Collections;


/**
 * Input Controller for handling all mouse, touch, and keyboard events
 * @author  Eric Heaney, Dan Sternfeld
 * @version 1.0, June 29, 2013
 */
public class StateController : MonoBehaviour, IController {
	
	[SerializeField]
	public enum GameStates { 
		MAIN_MENU,
		PLAYING,
		PAUSED,
		LEVEL_TRANSITION_MENU,
		NUM_STATES
	};
	
	private GameState[] _gameStateRefs;	
	private GameState _currentState;
	
	private Stack _gameStateStack = new Stack();
	
	/** Build states */
	public void Init() {
		_gameStateRefs = new GameState[(int)GameStates.NUM_STATES];
		_gameStateRefs[(int)GameStates.MAIN_MENU] = new MainMenu();
		_gameStateRefs[(int)GameStates.PLAYING] = new Playing();
		_gameStateRefs[(int)GameStates.PAUSED] = new Paused();
		_gameStateRefs[(int)GameStates.LEVEL_TRANSITION_MENU] = new LevelTransitionMenu();
		
		_currentState = _gameStateRefs[(int)GameStates.PLAYING];
		_gameStateStack.Push(_gameStateRefs[(int)GameStates.PLAYING]);
		_currentState.OnEnter();
	}
	
	/**
	 * Push gameState onto the gamestate stack
	 * @params: GameStates state 	the delegate that should be removed from the event type T's handler list
	 */
	public void pushState(GameStates state) {
		_currentState.OnExit();
		_currentState = _gameStateRefs[(int)state];
		_gameStateStack.Push(_gameStateRefs[(int)state]);
		_currentState.OnEnter();
	}
	
	/** Push Pop gameState off the gamestate stack */
	public void popState() {
		_currentState.OnExit();
		_gameStateStack.Pop();
		_currentState = (GameState) _gameStateStack.Peek();
		_currentState.OnEnter();
	}
	
}
