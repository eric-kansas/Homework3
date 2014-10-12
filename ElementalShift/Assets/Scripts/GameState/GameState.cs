using UnityEngine;
using System.Collections;

public interface GameState {
	void OnEnter();
	void OnExit();
}

public class MainMenu : GameState {
		
	public MainMenu() {
		
	}
	
	public void OnEnter() {
		
	}
	
	public void OnExit() {
	
	}
	
}

public class Playing : GameState {
		
	public Playing() {
		
	}
	
	public void OnEnter() {
		/*
		//ApplicationController.Controllers["GameController"].Player.OnTurn += OnTurnHandler;
		GameController gameController = (GameController)ApplicationController.Controllers["GameController"];
		Player player = (Player) gameController.Player;
		
		// have the game controller & the camera listen to player events
		player.AddReceiver(gameController);
		player.AddReceiver((CameraController) ApplicationController.Controllers["CameraController"]);
		
		// have player listen to the input controller
		((InputController)ApplicationController.Controllers["InputController"]).AddReceiver(player);
		*/
	}
	
	public void OnExit() {
	
	}
	
}

public class Paused : GameState {
		
	public Paused() {
		
	}
	
	public void OnEnter() {
		
	}
	
	public void OnExit() {
	
	}
	
}

public class LevelTransitionMenu : GameState {
		
	public LevelTransitionMenu() {
		
	}
	
	public void OnEnter() {
		
	}
	
	public void OnExit() {
	
	}
	
}
