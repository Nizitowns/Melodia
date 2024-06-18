using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Events;
using UnityEngine.UI;

public class Border : MonoBehaviour
{

    #region Fields

    [SerializeField]
    [Tooltip("How long the border flicker lasts.")]
    private float flickerTime = 0.3f;

    // Variable to track the current alpha
    private float alpha = 0.0f;
    // Resting alpha for the border
    private float restAlpha = 1f;
    // Timer to track the flicker
    private float timer = 0.0f;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the alpha of the border.
    /// </summary>
    private void Start()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.0f);
    }

    #endregion

    #region Graphics

    /// <summary>
    /// Sets the resting alpha of the border
    /// </summary>
    public void setRestingAlpha(float a)
    {
        restAlpha = a;
    }

    /// <summary>
    /// Sets the color of the border
    /// </summary>
    public void setColor(Color c)
    {
        GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
    }

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
        if (timer == flickerTime)
            alpha = 1f;
        else if (timer < (flickerTime / 2.0f) && timer > 0.0f)
            alpha -= Time.deltaTime / (flickerTime / 2.0f);
        else if (timer <= 0.0f && Mathf.Abs(alpha - restAlpha) > 0.01f)
            alpha += Mathf.Sign(restAlpha - alpha) * Time.deltaTime;

        Color c = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);

        if (timer > 0.0f)
            timer -= Time.deltaTime;
    }

    #endregion
}
