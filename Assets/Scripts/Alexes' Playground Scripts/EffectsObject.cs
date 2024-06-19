using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the lifetime of prefab clones.
/// </summary>
public class EffectsObject : MonoBehaviour
{

    public enum DestroyType { Both,AfterLifetime, OnDisable};
    [Tooltip("How the effects object should be destroyed.")]
    public DestroyType destroyType;
    public float lifetime = 1f;

    private float timeToDestroyAt;
    // Start is called before the first frame update
    void Start()
    {
        timeToDestroyAt = Time.timeSinceLevelLoad + lifetime;

    }

    private void OnEnable()
    {
        if (timeToDestroyAt > 0)//Dont run on the first enable, but only on subsequent enables mid-game
        {
            if (destroyType == DestroyType.Both || destroyType == DestroyType.AfterLifetime)
            {
                //destroy the clone after liftime elapses
                if (Time.timeSinceLevelLoad >= timeToDestroyAt)
                {
                    //We have to do this method because the EffectsObjects may be getting disabled/enabled at this moment.
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnDisable()
    {
        if (destroyType == DestroyType.Both || destroyType == DestroyType.OnDisable)
        {
            //Destroy the gameobject on disable
            Destroy(gameObject);
        }
    }

    private void CheckDestroyStatus()
    {
        if (destroyType == DestroyType.Both || destroyType == DestroyType.AfterLifetime)
        {
            //destroy the clone after liftime elapses
            if (Time.timeSinceLevelLoad >= timeToDestroyAt)
            {
                //We have to do this method because the EffectsObjects may be getting disabled/enabled at this moment.
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        CheckDestroyStatus();

    }
}