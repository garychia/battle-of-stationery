using Godot;
using Godot.Collections;

// The default game setup.
public class DefaultBattle : BattleSetup
{
	public static Array PlayersPositions = new Array { new Vector3(0, 3, 5), new Vector3(0, 3, -5) };

	public DefaultBattle() : base(PlayersPositions)
	{
	}
}
