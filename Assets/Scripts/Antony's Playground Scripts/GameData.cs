using System;

/// <summary>
/// Data structure for storing game data, including player stats and game progress.
/// </summary>
[Serializable]
public class GameData
{
    #region Player Stats

    public int playerHealth;
    public int playerScore;

    #endregion

    #region Game Progress

    public int levelReached;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes a new instance of the GameData class with default values.
    /// </summary>
    public GameData()
    {
        playerHealth = 100;
        playerScore = 0;
        levelReached = 1;
    }

    #endregion
}
