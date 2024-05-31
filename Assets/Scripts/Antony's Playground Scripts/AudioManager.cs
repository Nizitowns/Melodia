using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages audio playback for the game, including background music and sound effects.
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static AudioManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of AudioManager.
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

    [System.Serializable]
    public class AudioClipInfo
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    [Tooltip("List of audio clips used in the game.")]
    private List<AudioClipInfo> audioClips;

    private Dictionary<string, AudioClip> audioClipDictionary;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes the audio manager and sets up audio sources.
    /// </summary>
    private void Start()
    {
        InitializeAudioSources();
        InitializeAudioClipDictionary();
    }

    /// <summary>
    /// Initializes the audio sources for music and sound effects.
    /// </summary>
    private void InitializeAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Initializes the audio clip dictionary for quick lookup.
    /// </summary>
    private void InitializeAudioClipDictionary()
    {
        audioClipDictionary = new Dictionary<string, AudioClip>();
        foreach (var audioClipInfo in audioClips)
        {
            audioClipDictionary[audioClipInfo.name] = audioClipInfo.clip;
        }
    }

    #endregion

    #region Music Playback

    /// <summary>
    /// Plays the specified music clip.
    /// </summary>
    /// <param name="name">The name of the music clip to play.</param>
    /// <param name="loop">Whether the music should loop.</param>
    public void PlayMusic(string name, bool loop = true)
    {
        if (audioClipDictionary.ContainsKey(name))
        {
            musicSource.clip = audioClipDictionary[name];
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music clip not found: " + name);
        }
    }

    /// <summary>
    /// Stops the currently playing music.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    #endregion

    #region Sound Effects Playback

    /// <summary>
    /// Plays the specified sound effect.
    /// </summary>
    /// <param name="name">The name of the sound effect to play.</param>
    public void PlaySFX(string name)
    {
        if (audioClipDictionary.ContainsKey(name))
        {
            sfxSource.PlayOneShot(audioClipDictionary[name]);
        }
        else
        {
            Debug.LogWarning("SFX clip not found: " + name);
        }
    }

    #endregion
}
