using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the main menu interactions, handling button clicks for starting the game and quitting the application.
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region UI References

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the main menu buttons.
    /// </summary>
    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    #endregion

    #region Button Handlers

    /// <summary>
    /// Starts the game when the start button is clicked.
    /// </summary>
    private void OnStartButtonClicked()
    {
        UIManager.Instance.OnStartButtonClicked();
    }

    /// <summary>
    /// Quits the game when the quit button is clicked.
    /// </summary>
    private void OnQuitButtonClicked()
    {
        UIManager.Instance.OnQuitButtonClicked();
    }

    #endregion
}
