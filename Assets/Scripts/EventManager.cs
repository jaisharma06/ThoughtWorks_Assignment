using System;

public static class EventManager
{
    public static Action<Tank> OnTankTouched;
    public static Action<int> OnTurnSwitched;
    public static Action<int> OnGameOver;

    /// <summary>
    /// Called whenever a tank is touched
    /// </summary>
    /// <param name="tank">The tank touched by the player</param>
    public static void TankTouched(Tank tank)
    {
        OnTankTouched?.Invoke(tank);
    }

    /// <summary>
    /// Called when the player switches turn.
    /// i.e. One player has taken his turn and now it's other player's turn
    /// </summary>
    /// <param name="turn">1 for player one and 2 for player 2</param>
    public static void TurnSwitched(int turn)
    {
        OnTurnSwitched?.Invoke(turn);
    }

    /// <summary>
    /// Called when the game is over
    /// </summary>
    /// <param name="result">Result of the match</param>
    public static void GameOver(int result)
    {
        OnGameOver?.Invoke(result);
    }
}
