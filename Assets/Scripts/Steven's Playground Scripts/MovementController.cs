using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    [SerializeField]
    [Tooltip("A reference to the game object containing the parallax backgrounds as children.")]
    private GameObject parallaxContainer;

    [SerializeField]
    [Tooltip("A reference to the game object containing the tribe as children.")]
    private GameObject tribe;

    [SerializeField]
    [Tooltip("The amount the tribe moves forward.")]
    private float moveDistance = 1.0f;

    [SerializeField]
    [Tooltip("The multiplier received to movement when in fever mode.")]
    private float feverMultiplier = 2.0f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward when failing.")]
    private float regressDistance = 0.5f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward (each beat) after a lack of progress.")]
    private float driftDistance = 0.1f;

    // Instances
    LevelManager levelManager;

    // Flag for whether the tribe is drifting
    private bool drifting = false;
    // Flag for whether the tribe has moved forward yet
    private bool moved = false;
    // Flag for whether the tribe has made their first move yet
    private bool firstMove = false;
    // Keeping track of the tribe's progress
    private float progress = 0f;

    #endregion

    #region Initialization

    private void Start()
    {
        // Instances
        levelManager = LevelManager.Instance;
    }

    #endregion

    #region Visuals

    /// <summary>
    /// Moves the parallax backgrounds a certain distance
    /// </summary>
    private void moveBackground(float distance, bool left)
    {
        foreach (Parallax p in parallaxContainer.GetComponentsInChildren<Parallax>())
        {
            p.setMoveLeft(left);
            p.addDistance(distance);
        }
    }

    /// <summary>
    /// Moves the tribe and saturation effect forward (at start of level)
    /// </summary>
    private void moveTribe()
    {
        moved = true;
        foreach (Transform t in tribe.GetComponentInChildren<Transform>())
            if (t.gameObject.tag == "Tribe" && t.localPosition.x < -5f)
            {
                t.gameObject.GetComponent<Transform>().SetLocalPositionAndRotation(t.localPosition + new Vector3(Time.deltaTime * 2f, 0f, 0f), Quaternion.identity);
                moved = false;
            }
    }

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
    /// Stop drifting the tribe.
    /// </summary>
    public void stopDrift()
    {
        drifting = false;
    }

    /// <summary>
    /// If the tribe is drifting, move them backward.
    /// </summary>
    public void drift()
    {
        if (drifting)
        {
            moveBackground(driftDistance, false);
            progress -= driftDistance;
        }

    }

    /// <summary>
    ///  Move the tribe forward.
    /// </summary>
    public void moveForward()
    {
        firstMove = true;
        moveBackground(moveDistance, true);
        progress += moveDistance;

        if (levelManager.getCurrentEvent().type == LevelEventType.FREE_AREA && progress >= levelManager.getCurrentEvent().areaLength)
        {
            progress = 0f;
            levelManager.finishEvent();
        }
            
    }

    /// <summary>
    ///  Move the tribe backward.
    /// </summary>
    public void moveBackward()
    {
        moveBackground(regressDistance, false);
        progress -= regressDistance;
    }

    private void Update()
    {
        if (firstMove && !moved)
            moveTribe();
    }

    #endregion
}
