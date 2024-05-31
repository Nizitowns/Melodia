using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages player input and triggers corresponding actions based on input events.
/// Supports keyboard, mouse, joypad controller, and Steam Deck.
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static InputManager Instance { get; private set; }

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

    #region Input Actions

    // Input actions asset
    [SerializeField] private InputActionAsset inputActions;

    // Individual input action maps
    private InputActionMap gameplayActionMap;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction pauseAction;
    private InputAction commandPataAction;
    private InputAction commandPonAction;
    private InputAction commandChakaAction;
    private InputAction commandDonAction;

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

        moveAction = gameplayActionMap.FindAction("Move");
        attackAction = gameplayActionMap.FindAction("Attack");
        pauseAction = gameplayActionMap.FindAction("Pause");
        commandPataAction = gameplayActionMap.FindAction("CommandPata");
        commandPonAction = gameplayActionMap.FindAction("CommandPon");
        commandChakaAction = gameplayActionMap.FindAction("CommandChaka");
        commandDonAction = gameplayActionMap.FindAction("CommandDon");

        moveAction.performed += OnMove;
        attackAction.performed += OnAttack;
        pauseAction.performed += OnPause;
        commandPataAction.performed += ctx => OnCommand("Pata");
        commandPonAction.performed += ctx => OnCommand("Pon");
        commandChakaAction.performed += ctx => OnCommand("Chaka");
        commandDonAction.performed += ctx => OnCommand("Don");
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
    /// Handles the move input action.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        Debug.Log($"Move input: {movement}");
        // Implement movement logic here
    }

    /// <summary>
    /// Handles the attack input action.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack input");
        // Implement attack logic here
    }

    /// <summary>
    /// Handles the pause input action.
    /// </summary>
    /// <param name="context">The context of the input action.</param>
    private void OnPause(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.PauseGame();
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    /// <summary>
    /// Handles the command input action.
    /// </summary>
    /// <param name="command">The command input.</param>
    private void OnCommand(string command)
    {
        Debug.Log($"Command input: {command}");
        CommandInputEvent?.Invoke(command);
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Unsubscribes from input events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        moveAction.performed -= OnMove;
        attackAction.performed -= OnAttack;
        pauseAction.performed -= OnPause;
        commandPataAction.performed -= ctx => OnCommand("Pata");
        commandPonAction.performed -= ctx => OnCommand("Pon");
        commandChakaAction.performed -= ctx => OnCommand("Chaka");
        commandDonAction.performed -= ctx => OnCommand("Don");
    }

    #endregion
}
