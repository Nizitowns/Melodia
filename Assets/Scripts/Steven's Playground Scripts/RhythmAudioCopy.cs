using UnityEngine;

/// <summary>
/// Manages audio playback synchronized with the game's rhythm system.
/// </summary>
public class RhythmAudioCopy : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("Name of the sound effect to play on each beat.")]
    private string beatSFXName = "BeatSound";

    [SerializeField]
    [Tooltip("Name of the music track to play in the background.")]
    private string musicTrackName = "MainTrack";

    private RhythmSystemCopy rhythmSystem;
    private AudioManager audioManager;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the rhythm audio manager.
    /// </summary>
    private void Start()
    {
        rhythmSystem = RhythmSystemCopy.Instance;
        audioManager = AudioManager.Instance;

        if (rhythmSystem != null)
        {
            rhythmSystem.OnBeat += OnBeat;
        }

        PlayBackgroundMusic();
    }

    /// <summary>
    /// Unsubscribes from the rhythm system's beat event when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (rhythmSystem != null)
        {
            rhythmSystem.OnBeat -= OnBeat;
        }
    }

    #endregion

    #region Beat Handling

    /// <summary>
    /// Called on each beat of the rhythm.
    /// Plays the beat sound effect.
    /// </summary>
    private void OnBeat()
    {
        audioManager.PlaySFX(beatSFXName);
    }

    #endregion

    #region Music Playback

    /// <summary>
    /// Plays the background music track.
    /// </summary>
    private void PlayBackgroundMusic()
    {
        //audioManager.PlayMusic(musicTrackName);
    }

    #endregion
}
