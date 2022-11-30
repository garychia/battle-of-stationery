using Godot;
using System;

public class MainMenu : Control
{
	public override void _Ready()
	{
		// Setup the signals of buttons.
		var playButton = GetNode<Button>("/root/MainMenu/ButtonContainer/PlayButton");
		var quitButton = GetNode<Button>("/root/MainMenu/ButtonContainer/QuitButton");
		playButton.Connect("button_up", this, nameof(onPlayButtonUp));
		quitButton.Connect("button_up", this, nameof(onQuitButtonUp));
	}
	
	// Start the game.
	private void onPlayButtonUp()
	{
		GetTree().ChangeScene("res://Scenes/BattleField.tscn");
	}
	
	// Quit the game.
	private void onQuitButtonUp()
	{
		GetTree().Quit();
	}
}
