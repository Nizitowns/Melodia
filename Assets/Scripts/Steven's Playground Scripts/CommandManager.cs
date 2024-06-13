using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static CommandManager Instance { get; private set; }

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

    [System.Serializable]
    public class Command
    {
        public string name;
        public string actionName;
        public List<int> keys;
    }

    [SerializeField]
    [Tooltip("List of commands used in the game.")]
    private List<Command> commandList;

    // Instances
    private MovementController movementController;
    private RhythmManager rhythmManager;
    private InputReceiver inputReceiver;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes input actions and sets up event listeners.
    /// </summary>
    private void Start()
    {
        // Instances
        movementController = MovementController.Instance;
        rhythmManager = RhythmManager.Instance;
        inputReceiver = InputReceiver.Instance;
    }

    #endregion

    #region Simon Says

    /// <summary>
    /// Checks if a command matches the current Simon Says pattern.
    /// </summary>
    public bool checkSimon(int button)
    {
        return button == rhythmManager.getSimonPattern()[rhythmManager.getBeatsPlayed() - 1];
    }

    /// <summary>
    /// Chooses a command for Simon Says at random.
    /// </summary>
    public List<int> chooseCommand()
    {
        return commandList[Random.Range(0, commandList.Count)].keys;
    }

    #endregion

    #region Freeplay Commands

    /// <summary>
    /// Checks if a button goes toward a command in the current string, adding it to the string if it does.
    /// </summary>
    public void checkForCommand(int button)
    {
        List<int> commandString = rhythmManager.getCommandString();
        if (commandString.Count == 0)
            rhythmManager.addToCommandString(button);
        else
        {
            int patternLength = commandString.Count;
            foreach (Command c in commandList)
            {
                bool following = true;
                for (int i = 0; i < commandString.Count; i++)
                {
                    if (c.keys[i] != commandString[i])
                    {
                        following = false;
                        break;
                    }
                }
                if (following && button == c.keys[commandString.Count])
                {
                    rhythmManager.addToCommandString(button);
                }
            }
            if (patternLength == commandString.Count)
            {
                inputReceiver.missedNote();
            }
        }
    }

    #endregion

    #region Performing Commands

    /// <summary>
    /// Perform a command based on the command string.
    /// </summary>
    public void doCommand(List<int> commandString)
    {
        int playedCommand = -1;
        foreach (Command c in commandList)
        {
            playedCommand = commandList.IndexOf(c);
            for (int i = 0; i < commandString.Count; i++)
                if (c.keys[i] != commandString[i])
                    playedCommand = -1;
            if (playedCommand != -1)
                break;
        }
        if (playedCommand != -1)
        {
            print("Played Command: " + commandList[playedCommand].name);
            switch (commandList[playedCommand].actionName)
            {
                case "move":
                    move();
                    break;
            }
        }
        else
            print("Command not found!");
    }

    /// <summary>
    /// Perform the "move" command
    /// </summary>
    private void move()
    {
        rhythmManager.resetStaticBeats();
        movementController.stopDrift();
        movementController.moveForward();
    }

    #endregion
}
