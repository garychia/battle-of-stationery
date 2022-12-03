using Godot;

// Represents a player in the game.
public class Player : RigidBody
{
	// identifies a Player in the game.
	private int id = -1;

	// Set an ID for the Player.
	public void SetID(int newID)
	{
		if (newID > -1)
			id = newID;
	}

	// Retrieve the ID of Player.
	public int GetID()
	{
		return id;
	}


	// Apply an impulse to the Player.
	public void ReceiveImpulse(Vector3 forcePosition, Vector3 forceDirection)
	{
		ApplyImpulse(forcePosition - GlobalTransform.origin, forceDirection * GameSettings.Force);
	}
}
