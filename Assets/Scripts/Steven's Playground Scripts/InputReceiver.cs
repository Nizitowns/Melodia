using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static RhythmManager;

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
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
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
    private float perfectMargin = 0.2f;

    [SerializeField]
    [Tooltip("Margin of error for an Excellent note, as a percentage of the beat interval.")]
    private float excellentMargin = 0.2f;

    [SerializeField]
    [Tooltip("Margin of error for a Good note, as a percentage of the beat interval.")]
    private float goodMargin = 0.2f;

    // Instances
    private UIEffects uiEffects;
    private RhythmManager rhythmManager;
    private CommandManager commandManager;
    private MovementController movementController;
    private CommandAudio commandAudio;

    // Individual input action maps
    private InputActionMap gameplayActionMap;
    private InputAction button1;
    private InputAction button2;
    private InputAction button3;
    private InputAction button4;

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
        commandAudio = CommandAudio.Instance;

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

    #endregion

    #region Input Handlers

    // Helper functions to map button presses to correct parameter pass to pressButton.
    private void doButton1(InputAction.CallbackContext context) { pressButton(1, rhythmManager.getGameState()); }
    private void doButton2(InputAction.CallbackContext context) { pressButton(2, rhythmManager.getGameState()); }
    private void doButton3(InputAction.CallbackContext context) { pressButton(3, rhythmManager.getGameState()); }
    private void doButton4(InputAction.CallbackContext context) { pressButton(4, rhythmManager.getGameState()); }

    /// <summary>
    /// Receives a player input.
    /// </summary>
    public void pressButton(int button, State gameState)
    {
        uiEffects.flashButton(button);

        // Check the note's timing
        if (checkTiming(rhythmManager.isBeatUsed()))
        {
            // If in Simon Says mode, check the input is the correct button
            if (gameState == State.SIMONPLAY)
            {
                if (!commandManager.checkSimon(button))
                    missedNote(gameState);
            }
            // If in freeplay mode, check if the input goes toward a command
            else if (gameState == State.FREEPLAY)
            {
                commandManager.checkForCommand(button);
            }
        }

        commandAudio.playSound(button);
    }

    /// <summary>
    /// Checks the played beat's timing. Returns false on missed notes.
    /// </summary>
    private bool checkTiming(bool beatUsed)
    {
        if (!beatUsed)
        {
            if (rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f || 
                1 - rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f)
                uiEffects.giveFeedback("Perfect");
            else if (rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f || 
                1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f)
                uiEffects.giveFeedback("Excellent");
            else if (rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f || 
                1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                uiEffects.giveFeedback("Good");
            else
            {
                missedNote(rhythmManager.getGameState());
                rhythmManager.useBeat();
                return false;
            }

            rhythmManager.useBeat();
        }
        else
        {
            if (1 - rhythmManager.GetCurrentBeatProgress() < perfectMargin / 2.0f)
                uiEffects.giveFeedback("Perfect");
            else if (1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin) / 2.0f)
                uiEffects.giveFeedback("Excellent");
            else if (1 - rhythmManager.GetCurrentBeatProgress() < (perfectMargin + excellentMargin + goodMargin) / 2.0f)
                uiEffects.giveFeedback("Good");
            else
            {
                missedNote(rhythmManager.getGameState());
                rhythmManager.useNextBeat();
                return false;
            }

            rhythmManager.useNextBeat();
        }
        return true;
    }

    /// <summary>
    /// Resets commands, combos, and fever mode on missed notes.
    /// </summary>
    public void missedNote(State gameState)
    {
        if (gameState == State.SIMONPLAY)
        {
            uiEffects.giveFeedback("Miss");
            rhythmManager.setGameState(State.SIMONTEACH);
        }
        else if (gameState == State.FREEPLAY)
        {
            uiEffects.giveFeedback("Miss");
            rhythmManager.emptyCommandString();
            movementController.moveBackward();
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
