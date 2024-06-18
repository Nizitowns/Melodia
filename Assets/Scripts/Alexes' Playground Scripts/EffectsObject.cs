using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the lifetime of prefab clones.
/// </summary>
public class EffectsObject : MonoBehaviour
{

    public float lifetime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //destroy the clone after 1 second
        Destroy(gameObject, lifetime);
        
    }
}