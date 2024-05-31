using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the input command sequences and executes corresponding actions based on the detected commands.
/// </summary>
public class CommandSystem : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static CommandSystem Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of CommandSystem.
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

    // List to store the command sequence input by the player
    private List<string> commandSequence = new List<string>();
    // Time window to complete a command
    private const float commandInputTime = 2.0f;
    // Timer to track the duration since the last input
    private float commandTimer;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Subscribes to the InputManager's command input event.
    /// </summary>
    private void Start()
    {
        InputManager.Instance.CommandInputEvent += OnCommandInput;
    }

    /// <summary>
    /// Unsubscribes from the InputManager's command input event when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        InputManager.Instance.CommandInputEvent -= OnCommandInput;
    }

    #endregion

    #region Command Handling

    /// <summary>
    /// Handles the command input from the player.
    /// </summary>
    /// <param name="input">The input command.</param>
    private void OnCommandInput(string input)
    {
        commandSequence.Add(input);
        commandTimer = 0f; // Reset the timer each time a new input is received

        if (IsCommandComplete())
        {
            ExecuteCommand();
            commandSequence.Clear();
        }
    }

    /// <summary>
    /// Checks if the current command sequence forms a complete command.
    /// </summary>
    /// <returns>True if the command sequence is complete, false otherwise.</returns>
    private bool IsCommandComplete()
    {
        // Example: "Pata-Pata-Pata-Pon" for move forward
        if (commandSequence.Count == 4 &&
            commandSequence[0] == "Pata" &&
            commandSequence[1] == "Pata" &&
            commandSequence[2] == "Pata" &&
            commandSequence[3] == "Pon")
        {
            return true;
        }
        // Add more command checks here

        return false;
    }

    /// <summary>
    /// Executes the command based on the current command sequence.
    /// </summary>
    private void ExecuteCommand()
    {
        string command = string.Join("-", commandSequence.ToArray());
        Debug.Log("Executing command: " + command);

        // Implement command execution logic here, for example:
        // UnitManager.Instance.MoveUnits(command);
    }

    #endregion

    #region Update

    /// <summary>
    /// Updates the command timer and checks if the input time window has elapsed.
    /// </summary>
    private void Update()
    {
        if (commandSequence.Count > 0)
        {
            commandTimer += Time.deltaTime;
            if (commandTimer >= commandInputTime)
            {
                commandSequence.Clear();
                commandTimer = 0f;
            }
        }
    }

    #endregion
}
