using Godot;
using Godot.Collections;

// Controls the game flow.
public class GameManager : Node
{
	// Setup of the game.
	private BattleSetup setup = new DefaultBattle();

	// Current player.
	private int currentPlayerIndex = 0;

	// Players in the game.
	private Array players;

	private MainCamera mainCamera = null;

	private void focusCurrentPlayer()
	{
		var currentPlayer = (Player)players[currentPlayerIndex];
		mainCamera.FocusPlayer(currentPlayer.GlobalTranslation);
	}

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
			player.GlobalTranslation = position;
			players.Add(player);
			AddChild(player);
		}
	}

	// Initialize the game.
	public override void _Ready()
	{
		ResetGame();
		mainCamera = GetNode<MainCamera>("/root/World/MainCamera");
		mainCamera.Connect("PlayerClicked", this, nameof(OnPlayerClicked));
		mainCamera.Connect("FollowingFinished", this, nameof(OnCameraFollowingFinished));
		focusCurrentPlayer();
	}

	private void OnPlayerClicked(Player player, Vector3 forcePosition, Vector3 forceDirection)
	{
		if (player == players[currentPlayerIndex] as Player)
		{
			player.ReceiveImpulse(forcePosition, forceDirection);
			mainCamera.FollowPlayer(player);
			currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
		}
	}

	private void OnCameraFollowingFinished()
	{
		focusCurrentPlayer();
	}
}
