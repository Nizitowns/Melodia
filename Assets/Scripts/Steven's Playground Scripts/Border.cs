using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Events;

public class Border : MonoBehaviour
{

    #region Fields

    [SerializeField]
    [Tooltip("How long the border flicker lasts.")]
    private float flickerTime = 0.25f;

    // Variable to track the current alpha
    private float alpha = 0.0f;
    // Timer to track the flicker
    private float timer = 0.0f;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the alpha of the border.
    /// </summary>
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
    }

    #endregion

    #region Graphics

    /// <summary>
    /// Initiates the border flicker on the beat
    /// </summary>
    public void flicker()
    {
        timer = flickerTime;
    }

    #endregion

    #region Update

    /// <summary>
    /// Updates the flicker timer and flickers the border.
    /// </summary>
    private void Update()
    {
        if (timer > flickerTime / 2.0f)
        {
            timer -= Time.deltaTime;
            alpha += Time.deltaTime / (flickerTime / 2.0f);
        }
        else if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            alpha -= Time.deltaTime / (flickerTime / 2.0f);
        }
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
    }

    #endregion
}
