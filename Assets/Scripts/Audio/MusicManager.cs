using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    // MUSIC MANAGER VARIABLES
    // private AK.Wwise.Event mainMusic = null;
    // private AK.Wwise.RTPC RTPCName = null;

    private bool musicPlaying = false;


    private void Awake()
    {
        // Initializes Wwise Soundbank
        AkBankManager.LoadBank("Main", false, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Launch Music on Start
        if (!musicPlaying)
        {
            musicPlaying = true;
            AkSoundEngine.PostEvent("MainMusicStart", gameObject);
        }
    }   
    // Update is called once per frame
    void Update()
    {
        
    }
}
