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
	
	private bool roundReady = false;
	
	private GameUI ui = null;

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
		mainCamera = GetNode<MainCamera>("/root/World/MainCamera");
		mainCamera.Connect("PlayerClicked", this, nameof(OnPlayerClicked));
		mainCamera.Connect("FollowingFinished", this, nameof(OnCameraFollowingFinished));
		
		var detector = GetNode<FallingPlayerDetector>("/root/World/FallingPlayerDetector");
		detector.Connect("body_entered", this, nameof(OnFallingPlayerDetectorDetected));
		
		focusCurrentPlayer();
		roundReady = true;
		
		UpdateGameUI();
	}
	
	private void UpdateGameUI()
	{
		var playerLabel = ui.GetPlayerLabel();
		playerLabel.Text = "Player " + (currentPlayerIndex + 1).ToString();
	}

	// Initialize the game.
	public override void _Ready()
	{
		base._Ready();
		var uiScene = GD.Load<PackedScene>("res://Scenes/GameUI.tscn");
		ui = uiScene.Instance() as GameUI;
		var backButton = ui.GetBackButton();
		backButton.Connect("button_up", this, nameof(OnBackButtonUp));
		AddChild(ui);
		ResetGame();
	}

	private void OnPlayerClicked(Player player, Vector3 forcePosition, Vector3 forceDirection)
	{
		if (player == players[currentPlayerIndex] as Player && roundReady)
		{
			player.ReceiveImpulse(forcePosition, forceDirection);
			mainCamera.FollowPlayer(player);
			currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
			roundReady = false;
		}
	}

	private void OnCameraFollowingFinished()
	{
		focusCurrentPlayer();
		roundReady = true;
		UpdateGameUI();
	}
	
	private void OnFallingPlayerDetectorDetected(Node node)
	{
		var player = node as Player;
		if (!(player is null))
		{
			GD.Print("The player has lost:");
			GD.Print(player);
		}
	}
	
	private void OnBackButtonUp()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
}
