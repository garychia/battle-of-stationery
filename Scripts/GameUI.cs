using Godot;
using System;

public class GameUI : Control
{
	[Export]
	private NodePath PlayerLabelNode = null;
	
	[Export]
	private NodePath BackButton = null;
	
	public Label GetPlayerLabel()
	{
		return GetNode<Label>(PlayerLabelNode);
	}
	
	public Button GetBackButton()
	{
		return GetNode<Button>(BackButton);
	}
}
