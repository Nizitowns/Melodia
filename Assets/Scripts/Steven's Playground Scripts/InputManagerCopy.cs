using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Manages player input and triggers corresponding actions based on input events.
/// Supports keyboard, mouse, joypad controller, and Steam Deck.
/// </summary>
public class InputManagerCopy : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static InputManagerCopy Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of InputManager.
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

    [SerializeField]
    [Tooltip("The UnityEvent for button 1.")]
    private UnityEvent button1Event;
    [SerializeField]
    [Tooltip("The UnityEvent for button 2.")]
    private UnityEvent button2Event;
    [SerializeField]
    [Tooltip("The UnityEvent for button 3.")]
    private UnityEvent button3Event;
    [SerializeField]
    [Tooltip("The UnityEvent for button 4.")]
    private UnityEvent button4Event;

    // Input actions asset
    [SerializeField] private InputActionAsset inputActions;

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

    /// <summary>
    /// Inputs button 1.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void doButton1(InputAction.CallbackContext context)
    {
        button1Event.Invoke();
    }

    /// <summary>
    /// Inputs button 2.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void doButton2(InputAction.CallbackContext context)
    {
        button2Event.Invoke();
    }

    /// <summary>
    /// Inputs button 3.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void doButton3(InputAction.CallbackContext context)
    {
        button3Event.Invoke();
    }

    /// <summary>
    /// Inputs button 4.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void doButton4(InputAction.CallbackContext context)
    {
        button4Event.Invoke();
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Unsubscribes from input events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        
    }

    #endregion
}
