using Godot.Collections;

// Represents a game setup. (the position of a player, etc.)
public class BattleSetup
{
    // Positions of the players.
    public readonly Array PlayerPositions;

    // Initialize the BattleSetup with player initial positions.
    public BattleSetup(Array playerPositions)
    {
        PlayerPositions = playerPositions;
    }
}
