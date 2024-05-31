using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the overall game state and transitions between different game states.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of GameManager.
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

    #region Game State

    // Enum for the different game states
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    // Current game state
    public GameState CurrentState { get; private set; }

    // Events for state changes
    public event Action<GameState> OnGameStateChange;

    #endregion

    #region Configuration

    // Initial scene to load (configurable)
    [SerializeField] private string initialScene = "MainMenu";
    [SerializeField] private string firstLevelScene = "Level1";

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes the game state to MainMenu.
    /// </summary>
    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    #endregion

    #region State Management

    /// <summary>
    /// Changes the current game state and performs actions based on the new state.
    /// </summary>
    /// <param name="newState">The new game state to change to.</param>
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChange?.Invoke(newState); // Trigger state change event

        switch (newState)
        {
            case GameState.MainMenu:
                UIManager.Instance.ShowMainMenu();
                LoadScene(initialScene);
                break;
            case GameState.Playing:
                UIManager.Instance.ShowHUD();
                Time.timeScale = 1f; // Ensure the game is running
                break;
            case GameState.Paused:
                UIManager.Instance.ShowPauseMenu();
                Time.timeScale = 0f; // Pause the game
                break;
            case GameState.GameOver:
                UIManager.Instance.ShowGameOverMenu();
                break;
        }
    }

    /// <summary>
    /// Starts the game by changing the state to Playing.
    /// </summary>
    public void StartGame()
    {
        ChangeState(GameState.Playing);
        LoadScene(firstLevelScene);
    }

    /// <summary>
    /// Pauses the game by changing the state to Paused.
    /// </summary>
    public void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    /// <summary>
    /// Resumes the game from a paused state.
    /// </summary>
    public void ResumeGame()
    {
        ChangeState(GameState.Playing);
        Time.timeScale = 1f; // Resume the game
    }

    /// <summary>
    /// Ends the game by changing the state to GameOver.
    /// </summary>
    public void EndGame()
    {
        ChangeState(GameState.GameOver);
    }

    /// <summary>
    /// Returns to the main menu by loading the main menu scene and changing the state to MainMenu.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure the game is running
        ChangeState(GameState.MainMenu);
        LoadScene(initialScene);
    }

    #endregion

    #region Scene Management

    /// <summary>
    /// Loads a scene by name with error handling.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    private void LoadScene(string sceneName)
    {
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load scene {sceneName}: {e.Message}");
        }
    }

    #endregion

    #region Application Management

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        // If we are running in the Unity editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion
}
