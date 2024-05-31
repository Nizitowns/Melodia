using UnityEngine;

/// <summary>
/// Manages the UI elements in the game, including the main menu, HUD, pause menu, and game over menu.
/// Responds to changes in game state by showing or hiding relevant UI elements.
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of UIManager.
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

    #region UI References

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Subscribes to the GameManager's OnGameStateChange event.
    /// </summary>
    private void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStateChange;
        ShowMainMenu();
    }

    /// <summary>
    /// Unsubscribes from the GameManager's OnGameStateChange event when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChange -= HandleGameStateChange;
    }

    #endregion

    #region UI Management

    /// <summary>
    /// Handles changes in the game state and updates the UI accordingly.
    /// </summary>
    /// <param name="newState">The new game state.</param>
    private void HandleGameStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                ShowMainMenu();
                break;
            case GameManager.GameState.Playing:
                ShowHUD();
                break;
            case GameManager.GameState.Paused:
                ShowPauseMenu();
                break;
            case GameManager.GameState.GameOver:
                ShowGameOverMenu();
                break;
        }
    }

    /// <summary>
    /// Shows the main menu UI.
    /// </summary>
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        hud.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    /// <summary>
    /// Shows the HUD (Heads-Up Display) UI.
    /// </summary>
    public void ShowHUD()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    /// <summary>
    /// Shows the pause menu UI.
    /// </summary>
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Hides the pause menu UI.
    /// </summary>
    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Shows the game over menu UI.
    /// </summary>
    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    #endregion

    #region Button Handlers

    /// <summary>
    /// Starts the game when the start button is clicked.
    /// </summary>
    public void OnStartButtonClicked()
    {
        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Resumes the game when the resume button is clicked.
    /// </summary>
    public void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
        HidePauseMenu();
    }

    /// <summary>
    /// Returns to the main menu when the main menu button is clicked.
    /// </summary>
    public void OnMainMenuButtonClicked()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    /// <summary>
    /// Quits the game when the quit button is clicked.
    /// </summary>
    public void OnQuitButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }

    #endregion
}
