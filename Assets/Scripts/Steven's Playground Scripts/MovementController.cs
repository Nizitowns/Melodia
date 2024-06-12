using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static MovementController Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of InputReceiver.
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

    [SerializeField]
    [Tooltip("The amount the tribe moves forward.")]
    private float moveDistance = 1.0f;

    [SerializeField]
    [Tooltip("The amount the tribe moves forward in fever mode.")]
    private float feverMoveDistance = 2.0f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward when failing.")]
    private float regressDistance = 0.5f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward (each beat) after a lack of progress.")]
    private float driftDistance = 0.1f;

    // Flag for whether the tribe is drifting
    private bool drifting = false;

    #endregion

    #region Tribe Movement

    /// <summary>
    /// Start drifting the tribe.
    /// </summary>
    public void startDrift()
    {
        drifting = true;
    }

    /// <summary>
    /// If the tribe is drifting, move them backward.
    /// </summary>
    public void drift()
    {
        
    }

    /// <summary>
    ///  Move the tribe forward.
    /// </summary>
    public void moveForward()
    {
        
    }

    /// <summary>
    ///  Move the tribe backward.
    /// </summary>
    public void moveBackward()
    {
        
    }

    #endregion
}
