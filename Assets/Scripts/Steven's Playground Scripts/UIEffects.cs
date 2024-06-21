using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static FeedbackBoard;

public class UIEffects : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static UIEffects Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of RhythmSystem.
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
    [Tooltip("The UnityEvent for the border flicker.")]
    private UnityEvent borderFlicker;

    [SerializeField]
    [Tooltip("The UnityEvent for button 1 flashing.")]
    private UnityEvent button1Event;

    [SerializeField]
    [Tooltip("The UnityEvent for button 2 flashing.")]
    private UnityEvent button2Event;

    [SerializeField]
    [Tooltip("The UnityEvent for button 3 flashing.")]
    private UnityEvent button3Event;

    [SerializeField]
    [Tooltip("The UnityEvent for button 4 flashing.")]
    private UnityEvent button4Event;

    // Instances
    private FeedbackBoard feedbackBoard;
    private ScoreBoard scoreBoard;

    #endregion

    #region Initialization

    private void Start()
    {
        feedbackBoard = FeedbackBoard.Instance;
        scoreBoard = ScoreBoard.Instance;
    }

    #endregion

    #region Effects

    public void flickerBorder()
    {
        borderFlicker?.Invoke();
    }

    public void flashButton(int button)
    {
        switch (button)
        {
            case 1:
                button1Event?.Invoke();
                break;
            case 2:
                button2Event?.Invoke();
                break;
            case 3:
                button3Event?.Invoke();
                break;
            case 4:
                button4Event?.Invoke();
                break;
        }
    }

    public void giveFeedback(Feedback fb)
    {
        feedbackBoard.setFeedback(fb);
        feedbackBoard.show();
    }

    public void addScore(int score)
    {
        scoreBoard.IncrementScore(score);
    }

    #endregion
}
