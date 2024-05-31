using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the HUD (Heads-Up Display) elements in the game, including health and score displays.
/// Responds to changes in player stats and updates the HUD accordingly.
/// </summary>
public class HUD : MonoBehaviour
{
    #region UI References

    [SerializeField] private Text healthText;
    [SerializeField] private Text scoreText;

    private int score;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Subscribes to player stats change events.
    /// </summary>
    private void Start()
    {
        PlayerStats.OnHealthChanged += UpdateHealth;
        PlayerStats.OnScoreChanged += UpdateScore;
    }

    /// <summary>
    /// Unsubscribes from player stats change events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        PlayerStats.OnHealthChanged -= UpdateHealth;
        PlayerStats.OnScoreChanged -= UpdateScore;
    }

    #endregion

    #region UI Update

    /// <summary>
    /// Updates the health display in the HUD.
    /// </summary>
    /// <param name="health">The current health of the player.</param>
    private void UpdateHealth(int health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    /// <summary>
    /// Updates the score display in the HUD.
    /// </summary>
    /// <param name="newScore">The new score of the player.</param>
    private void UpdateScore(int newScore)
    {
        score = newScore;
        scoreText.text = "Score: " + score.ToString();
    }

    #endregion
}
