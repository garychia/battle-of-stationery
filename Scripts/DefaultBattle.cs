using Godot;
using Godot.Collections;

// The default game setup.
public class DefaultBattle : BattleSetup
{
	// Initialize with two player positions.
	public DefaultBattle() : base(new Array { new Vector3(0, 3, 5), new Vector3(0, 3, -5) })
	{
	}
}
