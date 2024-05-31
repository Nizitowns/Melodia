using System.IO;
using UnityEngine;

/// <summary>
/// Manages saving and loading game data to and from disk.
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static SaveLoadManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of SaveLoadManager.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    // Path to save the game data
    private string saveFilePath;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes the save file path.
    /// </summary>
    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_save.json");
    }

    #endregion

    #region Save/Load Operations

    /// <summary>
    /// Saves the game data to disk.
    /// </summary>
    /// <param name="gameData">The game data to save.</param>
    public void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to " + saveFilePath);
    }

    /// <summary>
    /// Loads the game data from disk.
    /// </summary>
    /// <returns>The loaded game data.</returns>
    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded from " + saveFilePath);
            return gameData;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + saveFilePath);
            return new GameData(); // Return a new game data if no save file exists
        }
    }

    /// <summary>
    /// Deletes the save file from disk.
    /// </summary>
    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted from " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No save file to delete at " + saveFilePath);
        }
    }

    #endregion
}
