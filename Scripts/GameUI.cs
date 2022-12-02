using Godot;
using System;

public class GameUI : Control
{
	[Export]
	private NodePath PlayerLabelNode = null;
	
	[Export]
	private NodePath BackButton = null;
	
	[Export]
	private NodePath WinMessageLabel = null;
	
	public Label GetPlayerLabel()
	{
		return GetNode<Label>(PlayerLabelNode);
	}
	
	public Button GetBackButton()
	{
		return GetNode<Button>(BackButton);
	}
	
	public Label GetWinMessageLabel()
	{
		return GetNode<Label>(WinMessageLabel);
	}
}
