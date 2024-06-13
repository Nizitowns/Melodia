using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    #region Singleton

    // Singleton instance
    public static MusicManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of RhythmSystem.
    /// </summary>
    private void Awake()
    {
        // Initializes Wwise Soundbank
        AkBankManager.LoadBank("Main", false, false);

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

    // MUSIC MANAGER VARIABLES
    // private AK.Wwise.Event mainMusic = null;
    // private AK.Wwise.RTPC RTPCName = null;

    [SerializeField]
    [Tooltip("The Wwise event to sync the beats to the music.")]
    private AK.Wwise.Event musicEvent;

    // Instances
    RhythmManager rhythmManager;

    private bool musicPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        // Instances
        rhythmManager = RhythmManager.Instance;
    }

    public void syncBeat()
    {
        musicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, CallbackFunction);
    }

    void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        rhythmManager.beat();
    }


}
