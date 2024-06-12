using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmInputtest : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event musicEvent;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Mire", gameObject);
        musicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, CallbackFunction);


        //StartCoroutine(LaunchSFXTest());


    }

    void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info) {

        
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LaunchSFXTest() {
        
        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Do", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "La", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Dore", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Mire", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(2.0f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Do", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "La", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Dore", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");

        yield return new WaitForSeconds(0.5f);

        AkSoundEngine.SetSwitch("RhythmInputSwitchGroup", "Mire", gameObject);
        AkSoundEngine.PostEvent("RhythmInputPlay", gameObject);
        Debug.Log("Launched");
    }
}
