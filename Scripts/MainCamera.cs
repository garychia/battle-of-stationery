using Godot;
using Godot.Collections;

// Displays the battle field and players in the game.
public class MainCamera : Camera
{
	[Signal]
	public delegate void PlayerClicked(Player player, Vector3 forcePosition, Vector3 forceDirection);
	
	// Length of ray used to detect a player.
	private const float RayLength = 1000;

	// Handle the input events.
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == 1)
		{
			// Calculate the ray vector.
			var mousePos = ProjectRayOrigin(eventMouseButton.Position);
			var rayEnd = mousePos + ProjectRayNormal(eventMouseButton.Position) * RayLength;
			var spaceState = GetWorld().DirectSpaceState;
			// Apply the ray to the game world.
			var result = spaceState.IntersectRay(mousePos, rayEnd, new Array { this });
			// Check the result of the ray cast.
			if (result.Contains("collider"))
			{
				var player = result["collider"] as Player;
				var forceDir = rayEnd - mousePos;
				forceDir.y = 0;
				forceDir = forceDir.Normalized();
				var forcePos = (Vector3)result["position"];
				if (player != null)
				{
					EmitSignal(nameof(PlayerClicked), player, forcePos, forceDir);
					//playerBody.ApplyImpulse(forcePos - playerBody.GlobalTransform.origin, forceDir * GameSettings.Force);
				}
			}
		}
	}
}
