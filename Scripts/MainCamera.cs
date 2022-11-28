using Godot;
using Godot.Collections;

// Displays the battle field and players in the game.
public class MainCamera : Camera
{
	[Signal]
	public delegate void PlayerClicked(Player player, Vector3 forcePosition, Vector3 forceDirection);

	[Signal]
	public delegate void FollowingFinished();

	// Length of ray used to detect a player.
	private const float RayLength = 1000;

	// whether the MainCamera is moving.
	private bool moving = false;

	private Vector3 destination;

	private Vector3 focusPoint;

	private float distanceToDestination = 0f;

	private bool mouseLButtonPressed = false;

	private bool following = false;

	private Player playerFollowed = null;

	private Vector3 playerToCamera;

	[Export]
	public float MovingSpeed = 0.2f;

	[Export]
	public float RotationSpeed = 0.003f;

	[Export]
	public float FollowingPlayerDuration = 5f;

	private void move()
	{
		var currentDistanceToDestination = Mathf.Abs(GlobalTransform.origin.DistanceTo(destination));
		if (currentDistanceToDestination < 0.05 || currentDistanceToDestination > 15)
		{
			GlobalTranslation = destination;
			moving = false;
		}
		else
		{
			var toDestination = (destination - GlobalTranslation).Normalized();
			var speed = Mathf.Lerp(0.01f, MovingSpeed, currentDistanceToDestination / distanceToDestination);
			GlobalTranslation += toDestination * speed;
		}
		LookAtFromPosition(GlobalTranslation, focusPoint, Vector3.Up);
	}

	void follow()
	{
		if (playerFollowed is null)
		{
			following = false;
			return;
		}
		GlobalTranslation = playerFollowed.GlobalTranslation + playerToCamera;
		LookAtFromPosition(GlobalTranslation, playerFollowed.GlobalTranslation, Vector3.Up);
	}

	void OnFollowingTimeout()
	{
		following = false;
		EmitSignal(nameof(FollowingFinished));
	}

	private void giveImpulseToPlayer(Vector2 mousePosition)
	{
		// Calculate the ray vector.
		var mousePos = ProjectRayOrigin(mousePosition);
		var rayEnd = mousePos + ProjectRayNormal(mousePosition) * RayLength;
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
			}
		}
	}

	public void FocusPlayer(Vector3 playerPosition)
	{
		var originToPlayer = playerPosition.Normalized();
		destination = playerPosition + originToPlayer * 3 + Vector3.Up * 4;
		distanceToDestination = Mathf.Abs(GlobalTranslation.DistanceTo(destination));
		focusPoint = playerPosition;
		moving = true;
	}

	public void FollowPlayer(Player player)
	{
		playerToCamera = GlobalTranslation - player.GlobalTranslation;
		playerFollowed = player;
		following = true;
		var timer = GetTree().CreateTimer(FollowingPlayerDuration);
		timer.Connect("timeout", this, nameof(OnFollowingTimeout));
	}

	public override void _Process(float delta)
	{
		if (moving)
			move();
		else if (following)
			follow();
	}

	// Handle the input events.
	public override void _Input(InputEvent @event)
	{
		var movable = !moving && !following;
		if (@event.IsActionPressed("move_forward") && movable)
		{
			GlobalTranslation += -GlobalTransform.basis.z;
		}
		else if (@event.IsActionPressed("move_backward") && movable)
		{
			GlobalTranslation += GlobalTransform.basis.z;
		}
		else if (@event.IsActionPressed("move_left") && movable)
		{
			GlobalTranslation += -GlobalTransform.basis.x;
		}
		else if (@event.IsActionPressed("move_right") && movable)
		{
			GlobalTranslation += GlobalTransform.basis.x;
		}
		else if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Doubleclick && eventMouseButton.ButtonIndex == 1 && !moving)
				giveImpulseToPlayer(eventMouseButton.Position);
			else
				mouseLButtonPressed = eventMouseButton.Pressed && eventMouseButton.ButtonIndex == 1;
		}
		else if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (mouseLButtonPressed && !moving)
			{
				var delta = eventMouseMotion.Relative;
				var xDelta = delta.x;
				var yDelta = delta.y;
				GlobalRotate(Vector3.Up, xDelta * RotationSpeed);
				GlobalRotate(GlobalTransform.basis.x, yDelta * RotationSpeed);
			}
		}
	}
}
