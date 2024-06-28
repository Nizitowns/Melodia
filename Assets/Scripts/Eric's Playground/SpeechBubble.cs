using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public GameObject[] Texts;
    public float SequentialDelay = 1.5f;
    //Enables text[id] then waits and enables text[id+1] instead
    public void EnableSequential(int id)
    {
        StopAllCoroutines();
        StartCoroutine(DelayedSpeech(id));
    }
    public IEnumerator DelayedSpeech(int id)
    {
        EnableOne(id );
        yield return new WaitForSeconds(SequentialDelay);
        EnableOne(id + 1);
        DisableOne(id);//This also disables coroutine so we have to place this here.
    }

    public void EnableOne(int id)
    {
        foreach(GameObject t in Texts)
        {
            t.SetActive(false);
        }
        Texts[id].gameObject.SetActive(true);
    }
    public void DisableOne(int id)
    {
        Texts[id].gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void DisableMe()
    {
        gameObject.SetActive(false);
    }
}
