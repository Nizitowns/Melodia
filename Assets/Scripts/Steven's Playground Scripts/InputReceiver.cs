using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static RhythmManager;
using static LevelManager;
using static FeedbackBoard;

public class InputReceiver : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static InputReceiver Instance { get; private set; }

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

    #region Events

    // Event triggered when a command input is received
    public event Action<string> CommandInputEvent;

    #endregion

    #region Fields
    // Input actions asset
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField]
    [Tooltip("Margin of error for a Perfect note, as a percentage of the beat interval.")]
    private float perfectMargin = 0.1f;

    [SerializeField]
    [Tooltip("Margin of error for an Excellent note, as a percentage of the beat interval.")]
    private float excellentMargin = 0.15f;

    [SerializeField]
    [Tooltip("Margin of error for a Good note, as a percentage of the beat interval.")]
    private float goodMargin = 0.2f;

    // Instances
    private UIEffects uiEffects;
    private RhythmManager rhythmManager;
    private CommandManager commandManager;
    private MovementController movementController;
    private SFXManager sfx;
    private LevelManager levelManager;
    private ComboHandler comboHandler;

    // Individual input action maps
    private InputActionMap gameplayActionMap;
    private InputAction button1;
    private InputAction button2;
    private InputAction button3;
    private InputAction button4;

    // Disabled button flags
    private bool button1Disabled = false;
    private bool button2Disabled = false;
    private bool button3Disabled = false;
    private bool button4Disabled = false;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes input actions and sets up event listeners.
    /// </summary>
    private void Start()
    {
        // Instances
        uiEffects = UIEffects.Instance;
        rhythmManager = RhythmManager.Instance;
        commandManager = CommandManager.Instance;
        movementController = MovementController.Instance;
        sfx = SFXManager.Instance;
        levelManager = LevelManager.Instance;
        comboHandler = ComboHandler.Instance;

        InitializeInputActions();
        EnableGameplayInput();
    }

    /// <summary>
    /// Initializes input actions and sets up references to individual actions.
    /// </summary>
    private void InitializeInputActions()
    {
        gameplayActionMap = inputActions.FindActionMap("Gameplay");

        button1 = gameplayActionMap.FindAction("Button 1");
        button2 = gameplayActionMap.FindAction("Button 2");
        button3 = gameplayActionMap.FindAction("Button 3");
        button4 = gameplayActionMap.FindAction("Button 4");

        button1.performed += doButton1;
        button2.performed += doButton2;
        button3.performed += doButton3;
        button4.performed += doButton4;
    }

    /// <summary>
    /// Enables gameplay input actions.
    /// </summary>
    public void EnableGameplayInput()
    {
        gameplayActionMap.Enable();
    }

    /// <summary>
    /// Disables gameplay input actions.
    /// </summary>
    public void DisableGameplayInput()
    {
        gameplayActionMap.Disable();
    }

    /// <summary>
    ///  Disables a particular button
    /// </summary>
    public void disableButton(int button)
    {
        switch (button)
        {
            case 1:
                button1Disabled = true;
                break;
            case 2:
                button2Disabled = true;
                break;
            case 3:
                button3Disabled = true;
                break;
            case 4:
                button4Disabled = true;
                break;
        }
    }

    /// <summary>
    ///  Enables a particular button
    /// </summary>
    public void enableButton(int button)
    {
        switch (button)
        {
            case 1:
                button1Disabled = false;
                break;
            case 2:
                button2Disabled = false;
                break;
            case 3:
                button3Disabled = false;
                break;
            case 4:
                button4Disabled = false;
                break;
        }
    }

    #endregion

    #region Input Handlers

    // Helper functions to map button presses to correct parameter pass to pressButton.
    private void doButton1(InputAction.CallbackContext context) { if (!button1Disabled) pressButton(1); }
    private void doButton2(InputAction.CallbackContext context) { if (!button2Disabled) pressButton(2); }
    private void doButton3(InputAction.CallbackContext context) { if (!button3Disabled) pressButton(3); }
    private void doButton4(InputAction.CallbackContext context) { if (!button4Disabled) pressButton(4); }

    /// <summary>
    /// Receives a player input.
    /// </summary>
    public void pressButton(int button)
    {
        uiEffects.flashButton(button);

        if (rhythmManager && levelManager.getCurrentEvent().type != LevelEventType.CUTSCENE)
        {
            // Check the note's timing
            if (checkTiming(rhythmManager.isBeatUsed()))
            {
                // If in Simon Says mode, check the input is the correct button
                if (levelManager.getCurrentEvent().type == LevelEventType.SIMON_SAYS || levelManager.getCurrentEvent().type == LevelEventType.NEW_TRIBE
                    || levelManager.getCurrentEvent().type == LevelEventType.OBSTACLE)
                {
                    if (!commandManager.checkSimon(button))
                    {
                        button = 0;
                        missedNote();
                    } 
                }
                // If in freeplay mode, check if the input goes toward a command
                else if (levelManager.getCurrentEvent().type == LevelEventType.FREE_AREA)
                {
                    commandManager.checkForCommand(button);
                    comboHandler.RegisterHit();
                }
            }
        }

        sfx.playButtonSound(button);
    }

    /// <summary>
    /// Checks the played beat's timing. Returns false on missed notes.
    /// </summary>
    public bool checkTiming(bool beatUsed, float progress = 0f)
    {
        if (levelManager.getCurrentEvent().type != LevelEventType.CUTSCENE)
        {
            if (!beatUsed)
            {
                if (rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f ||
                    1 - rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f)
                    uiEffects.giveFeedback(Feedback.PERFECT);
                else if (rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f ||
                    1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.GREAT);
                else if (rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f ||
                    1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.OKAY);
                else
                {
                    missedNote();
                    rhythmManager.useBeat();
                    return false;
                }

                rhythmManager.useBeat();
            }
            else
            {
                if (1 - rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f)
                    uiEffects.giveFeedback(Feedback.PERFECT);
                else if (1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.GREAT);
                else if (1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.OKAY);
                else
                {
                    missedNote();
                    rhythmManager.useNextBeat();
                    return false;
                }

                rhythmManager.useNextBeat();
            }
            return true;
        }
        else
        {
            if (!beatUsed)
            {
                if (progress < perfectMargin / 2.0f ||
                    1 - progress < perfectMargin / 2.0f)
                    uiEffects.giveFeedback(Feedback.PERFECT);
                else if (progress < (perfectMargin + excellentMargin) / 2.0f ||
                    1 - progress < (perfectMargin + excellentMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.GREAT);
                else if (progress < (perfectMargin + excellentMargin + goodMargin) / 2.0f ||
                    1 - progress < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.OKAY);
                else
                {
                    sfx.playButtonSound(0);
                    uiEffects.giveFeedback(Feedback.MISS);
                    return false;
                }
            }
            else
            {
                if (1 - progress < perfectMargin / 2.0f)
                    uiEffects.giveFeedback(Feedback.PERFECT);
                else if (1 - progress < (perfectMargin + excellentMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.GREAT);
                else if (1 - progress < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                    uiEffects.giveFeedback(Feedback.OKAY);
                else
                {
                    sfx.playButtonSound(0);
                    uiEffects.giveFeedback(Feedback.MISS);
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Resets commands, combos, and fever mode on missed notes.
    /// </summary>
    public void missedNote()
    {
        if (levelManager.getCurrentEvent().type == LevelEventType.SIMON_SAYS || levelManager.getCurrentEvent().type == LevelEventType.NEW_TRIBE
            || levelManager.getCurrentEvent().type == LevelEventType.OBSTACLE)
        {
            uiEffects.giveFeedback(Feedback.MISS);
            rhythmManager.setGameState(State.SIMONTEACH);
        }
        else if (levelManager.getCurrentEvent().type == LevelEventType.FREE_AREA)
        {
            comboHandler.ResetCombo();
            uiEffects.giveFeedback(Feedback.MISS);
            rhythmManager.emptyCommandString();
            //movementController.moveBackward();

        }
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Unsubscribes from input events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        button1.performed -= doButton1;
        button2.performed -= doButton2;
        button3.performed -= doButton3;
        button4.performed -= doButton4;
    }

    #endregion
}
