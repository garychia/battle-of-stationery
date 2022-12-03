using Godot;

// Represents an UI displayed when the game is running.
public class GameUI : Control
{
	// Path to the player label.
	[Export]
	private NodePath PlayerLabelNode = null;

	// Path to the "back" button.
	[Export]
	private NodePath BackButton = null;

	// Path to the win message label.
	[Export]
	private NodePath WinMessageLabel = null;

	// Get the player label.
	public Label GetPlayerLabel()
	{
		return GetNode<Label>(PlayerLabelNode);
	}

	// Get the "back" button.
	public Button GetBackButton()
	{
		return GetNode<Button>(BackButton);
	}


	// Get the win message label.
	public Label GetWinMessageLabel()
	{
		return GetNode<Label>(WinMessageLabel);
	}
}
