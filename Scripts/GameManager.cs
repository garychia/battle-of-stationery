using Godot;
using Godot.Collections;
using System.Collections.Generic;

// Controls the game flow.
public class GameManager : Node
{
	// Setup of the game.
	private BattleSetup setup = new DefaultBattle();

	// Current player.
	private int currentPlayerIndex = 0;

	// Players in the game.
	private Array players;

	// Players that has lost the game.
	private HashSet<int> losingPlayerIndices;

	// Main camera that watches the current player.
	private MainCamera mainCamera = null;

	// indicates if the round is ready.
	private bool roundReady = false;

	// UI that displays the information of game.
	private GameUI ui = null;

	// Allow the camera to move to and look at the current player.
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
		losingPlayerIndices = new HashSet<int>();
		// Spawn the players.
		var playerScene = GD.Load<PackedScene>("res://Scenes/Pencil.tscn");
		foreach (Vector3 position in setup.PlayerPositions)
		{
			var player = playerScene.Instance() as Player;
			player.SetID(players.Count);
			player.GlobalTranslation = position;
			players.Add(player);
			AddChild(player);
		}
		// Set up the camera.
		mainCamera = GetNode<MainCamera>("/root/World/MainCamera");
		mainCamera.Connect("PlayerClicked", this, nameof(OnPlayerClicked));
		mainCamera.Connect("FollowingFinished", this, nameof(OnCameraFollowingFinished));

		// Set up the detector.
		var detector = GetNode<FallingPlayerDetector>("/root/World/FallingPlayerDetector");
		detector.Connect("body_entered", this, nameof(OnFallingPlayerDetectorDetected));

		// Configure the camera and start the first round.
		focusCurrentPlayer();
		roundReady = true;

		// Update the UI.
		UpdateGameUI();
	}

	// Display the current player.
	private void UpdateGameUI()
	{
		var playerLabel = ui.GetPlayerLabel();
		var currentPlayer = (Player)players[currentPlayerIndex];
		playerLabel.Text = "Player " + (currentPlayer.GetID() + 1);
	}

	// Initialize the game.
	public override void _Ready()
	{
		base._Ready();
		// Set up the UI.
		var uiScene = GD.Load<PackedScene>("res://Scenes/GameUI.tscn");
		ui = uiScene.Instance() as GameUI;
		var backButton = ui.GetBackButton();
		backButton.Connect("button_up", this, nameof(OnBackButtonUp));
		AddChild(ui);
		ResetGame();
	}

	// Called when the player is double-clicked.
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

	// Called when the camera has finished following the player.
	private void OnCameraFollowingFinished()
	{
		focusCurrentPlayer();
		roundReady = true;
		UpdateGameUI();
	}

	// Called when a falling player has been detected.
	private void OnFallingPlayerDetectorDetected(Node node)
	{
		var player = node as Player;
		if (!(player is null))
		{
			losingPlayerIndices.Add(player.GetID());
			while (losingPlayerIndices.Contains(currentPlayerIndex))
			{
				currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
			}
			// Check if there is any winner.
			if (players.Count == losingPlayerIndices.Count + 1)
			{
				GetTree().Paused = true;
				var winMsgLabel = ui.GetWinMessageLabel();
				winMsgLabel.Visible = true;
				winMsgLabel.Text = "Player " + (currentPlayerIndex + 1) + " Won!";
			}
		}
	}

	// Called when the "back" button has been pressed.
	private void OnBackButtonUp()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		GetTree().Paused = false;
	}
}
