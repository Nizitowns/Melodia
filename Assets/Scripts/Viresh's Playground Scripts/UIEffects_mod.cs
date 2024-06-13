using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIEffects_mod : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static UIEffects_mod Instance { get; private set; }

    private HUD_VireshMod hud;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of RhythmSystem.
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
        GameObject hudObject = GameObject.Find("HUD");

        if (hudObject != null)
        {
            hud = hudObject.GetComponent<HUD_VireshMod>();

            if (hud != null)
            {
                Debug.Log("HUD_VireshMod component found.");
                // Your code to work with the HUD_VireshMod component
            }
            else
            {
                Debug.LogError("HUD_VireshMod component not found on the HUD object.");
            }
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

    #endregion

    #region Initialization

    private void Start()
    {
        //feedbackBoard = FeedbackBoard.Instance;
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

    public void giveFeedback(string feedback)
    {
        hud.UpdateMessage(feedback);
        hud.DisplayStatus();
        //feedbackBoard.setText(feedback);
        //feedbackBoard.show();
    }

    #endregion
}
