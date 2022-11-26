using Godot;
using Godot.Collections;

// Controls the game flow.
public class GameManager : Node
{
	// Setup of the game.
	private BattleSetup setup = new DefaultBattle();
	
	// Current player.
	private uint currentPlayerIndex = 0;
	
	// Players in the game.
	private Array players;
	
	// Reset the game to the intial state.
	private void ResetGame()
	{
		// Initialize the variables.
		currentPlayerIndex = 0;
		players = new Array();
		// Spawn the players.
		var playerScene = GD.Load<PackedScene>("res://Scenes/Pencil.tscn");
		foreach (Vector3 position in setup.PlayerPositions)
		{
			var player = playerScene.Instance() as Player;
			player.Translation = position;
			players.Add(player);
			AddChild(player);
		}
	}
	
	// Initialize the game.
	public override void _Ready()
	{
		ResetGame();
	}
}
