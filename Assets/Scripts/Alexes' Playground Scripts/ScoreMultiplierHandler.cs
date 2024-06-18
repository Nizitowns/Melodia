using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the score multiplier for fever mode.
/// </summary>
public class ScoreMultiplierHandler : MonoBehaviour
{
    // Instances
    private UIEffects uiEffects;

    // Singleton instance
    public static ScoreMultiplierHandler Instance { get; private set; }

    // Score multiplier
    private int scoreMultiplier = 1;

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

    private void Start()
    {
        // Instances
        uiEffects = UIEffects.Instance;
    }

    /// <summary>
    /// Applies score multiplier to the score per note.
    /// </summary>
    /// <param name="multiplier">Multiplier value to apply.</param>
    public void ApplyScoreMultiplier(int multiplier, int noteScore)
    {

        //get fever multiplier and current combo score
        scoreMultiplier = multiplier;
        // int currentComboScore = ComboHandler.Instance.GetCurrentComboCount();
        int currentPlayerScore = ComboHandler.Instance.GetCurrentPlayerScore();

        //apply multiplier on the score for each note
        int modifiedScorePerNote = noteScore * scoreMultiplier;

        //apply the modified values to the combo score
        //int modifiedScore = currentComboScore - 1 + modifiedScorePerNote;
        int modifiedScore = currentPlayerScore + modifiedScorePerNote;

        uiEffects.addScore(modifiedScorePerNote);

        //set the final modified combo score
        //ComboHandler.Instance.SetCurrentComboCount(modifiedScore);
        ComboHandler.Instance.SetCurrentPlayerScore(modifiedScore);
    }

    /// <summary>
    /// Reset score multiplier to base value.
    /// </summary>
    public void ResetScoreMultiplier()
    {
        scoreMultiplier = 1;
    }

    /// <summary>
    /// Get current score multiplier.
    /// </summary>
    /// <returns>Current score multiplier.</returns>
    public int GetScoreMultiplier()
    {
        return scoreMultiplier;
    }
}
