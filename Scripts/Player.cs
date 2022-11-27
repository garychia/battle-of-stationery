using Godot;

// Represents a player in the game.
public class Player : RigidBody
{
	public void ReceiveImpulse(Vector3 forcePosition, Vector3 forceDirection)
	{
		ApplyImpulse(forcePosition - GlobalTransform.origin, forceDirection * GameSettings.Force);
	}
}
