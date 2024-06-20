using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;

/// <summary>
/// Stores the number of each notes hit
///Displays the player performance at end of level
/// </summary>
public class NoteResultsDisplay : MonoBehaviour
{

    #region UI Variables

    public Text resultsText;
    public GameObject textBox;
    public GameObject continueButton;
    public Canvas canvas;

    #endregion

    #region Note Variables

    public int perfectNotes = 0;
    public int excellentNotes = 0;
    public int goodNotes = 0;
    public int missedNotes = 0;

    #endregion

    public bool levelEnded = false;

    /// <summary>
    /// Increment # of perfect notes hit
    /// </summary>
    public void LogPerfectNote()
    {
        if (!levelEnded)
        {
            perfectNotes++;
        }
    }

    /// <summary>
    /// Increment # of excellent notes hit
    /// </summary>
    public void LogExcellentNote()
    {
        if (!levelEnded)
        {
            excellentNotes++;
        }
    }

    /// <summary>
    /// Increment # of good notes hit
    /// </summary>
    public void LogGoodNote()
    {
        if (!levelEnded)
        {
            goodNotes++;
        }
    }

    /// <summary>
    /// Increment # of notes missed
    /// </summary>
    public void LogMissedNote()
    {
        if (!levelEnded)
        {
            missedNotes++;
        }
    }

    /// <summary>
    /// Modifies boolean and called display result function
    /// </summary>
    public void EndLevel()
    {
        levelEnded = true;
        DisplayResults();
    }

    /// <summary>
    /// Displays the summary of player results
    /// </summary>
    public void DisplayResults()
    {
        if (resultsText != null)
        {
            resultsText.text = $"      Results:\n \nPerfect Notes: {perfectNotes}\n \nExcellent Notes: {excellentNotes}\n \nGood Notes: {goodNotes}\n \nMissed Notes: {missedNotes}";
            Instantiate(textBox, textBox.transform.position, textBox.transform.rotation);

            //The continue button acts as a transition from one scene to another
            //In this demo, the button is coded to load the level2 scene
            //This can be modified in the button script
            Instantiate(continueButton, canvas.transform);
        }
        else
        {
            Debug.LogError("Results Text is not assigned.");
        }
    }
}