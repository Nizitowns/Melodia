using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static SFXManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of InputReceiver.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    [SerializeField]
    [Tooltip("The sound name for button 1.")]
    private string button1Sound = "Do";

    [SerializeField]
    [Tooltip("The sound name for button 2.")]
    private string button2Sound = "La";

    [SerializeField]
    [Tooltip("The sound name for button 3.")]
    private string button3Sound = "Mire";

    [SerializeField]
    [Tooltip("The sound name for button 4.")]
    private string button4Sound = "Dore";

    #endregion

    #region Audio

    /// <summary>
    /// Plays a sound given the button pressed.
    /// </summary>
    public void playButtonSound(int button)
    {
        string name = "";
        switch (button)
        {
            case 1:
                name = button1Sound;
                break;
            case 2:
                name = button2Sound;
                break;
            case 3:
                name = button3Sound;
                break;
            case 4:
                name = button4Sound;
                break;
            default:
                name = "Miss";
                break;
        }

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", name, gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
    }

    #endregion
}
