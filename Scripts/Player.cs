using Godot;

// Represents a player in the game.
public class Player : RigidBody
{
	private int id = -1;
	
	public void SetID(int newID)
	{
		if (newID > -1)
			id = newID;
	}
	
	public int GetID()
	{
		return id;
	}
	
	public void ReceiveImpulse(Vector3 forcePosition, Vector3 forceDirection)
	{
		ApplyImpulse(forcePosition - GlobalTransform.origin, forceDirection * GameSettings.Force);
	}
}
