using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonIndicator : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("How long the button flash lasts.")]
    private float flashTime = 0.25f;

    // Variable to track the current alpha
    private float alpha = 0.5f;
    // Timer to track the flash
    private float timer = 0.0f;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the alpha of the button.
    /// </summary>
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    #endregion

    #region Graphics

    /// <summary>
    /// Initiates the button flash on the beat
    /// </summary>
    public void flash()
    {
        if (timer <= 0.0f)
            timer = flashTime;
    }

    #endregion

    #region Update

    /// <summary>
    /// Updates the flash timer and flashes the button.
    /// </summary>
    private void Update()
    {
        if (timer == flashTime)
            alpha = 1f;
        else if (timer < (flashTime / 2.0f) && timer > 0.0f)
            alpha -= Time.deltaTime / flashTime;

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);

        if (timer > 0.0f)
            timer -= Time.deltaTime;
    }

    #endregion
}
