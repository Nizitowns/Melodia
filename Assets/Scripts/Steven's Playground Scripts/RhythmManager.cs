using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static RhythmManager Instance { get; private set; }

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
    }

    #endregion

    #region Events

    // Event triggered on each beat
    public event System.Action OnBeat;

    #endregion

    #region Fields

    [SerializeField]
    [Tooltip("Beats per minute (BPM) for the rhythm.")]
    private float bpm = 120f;

    [SerializeField]
    [Tooltip("The number of beats without progress before the tribe starts drifting backward.")]
    private int staticBeatLimit = 8;

    // Instances
    private InputReceiver inputReceiver;
    private UIEffects uiEffects;
    private CommandManager commandManager;
    private MovementController movementController;

    // Possible gamestates
    public enum State { SIMONTEACH, SIMONPLAY, FREEPLAY };
    // Current gamestate
    private State gameState = State.SIMONTEACH;
    // List of keys for the current Simon Says pattern
    private List<int> pattern = new List<int>();
    // List of keys for the current command string
    private List<int> commandString = new List<int>();
    // Length of the commands
    private int patternLength = 4;
    // Number of beats played toward a command
    private int beatsPlayed = 0;
    // Flag for whether the player has played a note on this beat
    private bool beatUsed = false;
    // Flag for whether the next beat has been used (by a slightly early note)
    private bool nextBeatUsed = false;
    // Time interval between beats
    private float beatInterval;
    // The number of beats since the tribe last progressed
    private int staticBeats = 0;
    // Timer to track time since the last beat
    private float timer;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes the rhythm system and starts the beat routine.
    /// </summary>
    private void Start()
    {
        // Instances
        inputReceiver = InputReceiver.Instance;
        uiEffects = UIEffects.Instance;
        commandManager = CommandManager.Instance;
        movementController = MovementController.Instance;

        InitializeRhythm();
        InitializeSimon();
        StartCoroutine(BeatRoutine());
        uiEffects.flickerBorder();

        OnBeat += freeBeat;
    }

    /// <summary>
    /// Initializes the Simon Says pattern.
    /// </summary>
    private void InitializeSimon()
    {
        pattern = commandManager.chooseCommand();
    }

    /// <summary>
    /// Initializes the rhythm system by calculating the beat interval.
    /// </summary>
    private void InitializeRhythm()
    {
        beatInterval = 60f / bpm;
    }

    #endregion

    #region Beat Management

    /// <summary>
    /// Coroutine that generates beats at regular intervals and triggers the OnBeat event.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator BeatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatInterval);
            timer = 0f;
            OnBeat?.Invoke();

            uiEffects.flickerBorder();
        }
    }

    /// <summary>
    /// Gets the progress of the current beat as a value between 0 and 1.
    /// </summary>
    /// <returns>Float value representing the progress of the current beat.</returns>
    public float GetCurrentBeatProgress()
    {
        return timer / beatInterval;
    }

    /// <summary>
    /// Free the beat to be used, if it hasn't already been used (happens every beat, no matter what).
    /// </summary>
    private void freeBeat()
    {
        if (!nextBeatUsed)
            beatUsed = false;
        else
            nextBeatUsed = false;
    }

    /// <summary>
    /// Shows the next Simon Says key.
    /// </summary>
    private void chooseKey()
    {
        if (!beatUsed)
        {
            uiEffects.flashButton(pattern[beatsPlayed]);
            beatsPlayed++;
        }
    }

    /// <summary>
    /// Checks if the player dropped a command by missing a beat.
    /// </summary>
    private void checkDroppedCommand()
    {
        if (pattern.Count > 0 && !beatUsed)
            inputReceiver.missedNote(gameState);
    }

    /// <summary>
    /// Counts the number of beats since the tribe last progressed.
    /// </summary>
    private void incrementStaticBeats()
    {
        staticBeats++;
        if (staticBeats >= staticBeatLimit)
            movementController.startDrift();
    }

    /// <summary>
    /// Sets the beat mode based on the game state.
    /// </summary>
    private void configureBeat()
    {
        OnBeat -= chooseKey;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
        switch (gameState)
        {
            case State.SIMONTEACH:
                OnBeat += chooseKey;
                break;
            case State.SIMONPLAY:
                break;
            case State.FREEPLAY:
                OnBeat += checkDroppedCommand;
                OnBeat += incrementStaticBeats;
                break;
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Returns the game state.
    /// </summary>
    public State getGameState()
    {
        return gameState;
    }

    /// <summary>
    /// Sets the game state.
    /// </summary>
    public void setGameState(State state)
    {
        gameState = state;
    }

    /// <summary>
    /// Returns whether the current beat has been used.
    /// </summary>
    public bool isBeatUsed()
    {
        return beatUsed;
    }

    /// <summary>
    /// Use the current beat.
    /// </summary>
    public void useBeat()
    {
        beatsPlayed++;
        beatUsed = true;
    }

    /// <summary>
    /// Use the next beat.
    /// </summary>
    public void useNextBeat()
    {
        nextBeatUsed = true;
    }

    /// <summary>
    /// Returns how many beats are played.
    /// </summary>
    public int getBeatsPlayed()
    {
        return beatsPlayed;
    }

    /// <summary>
    /// Returns the current Simon Says pattern.
    /// </summary>
    public List<int> getSimonPattern()
    {
        return pattern;
    }

    /// <summary>
    /// Returns the current command string.
    /// </summary>
    public List<int> getCommandString()
    {
        return commandString;
    }

    /// <summary>
    /// Adds a button to the command string.
    /// </summary>
    public void addToCommandString(int button)
    {
        commandString.Add(button);
    }

    /// <summary>
    /// Empties the command string.
    /// </summary>
    public void emptyCommandString()
    {
        commandString = new List<int>();
    }

    /// <summary>
    /// Updates the beat timer and game state.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;

        configureBeat();

        switch (gameState)
        {
            case State.SIMONTEACH:
                inputReceiver.DisableGameplayInput();
                if (beatsPlayed >= patternLength)
                {
                    beatsPlayed = 0;
                    gameState = State.SIMONPLAY;
                }
                break;
            case State.SIMONPLAY:
                inputReceiver.EnableGameplayInput();
                if (beatsPlayed >= patternLength)
                {
                    beatsPlayed = 0;
                    pattern = commandManager.chooseCommand();
                    gameState = State.SIMONTEACH;
                }
                break;
            case State.FREEPLAY:
                if (commandString.Count >= patternLength)
                {
                    commandManager.doCommand(commandString);
                    commandString = new List<int>();
                }
                break;
        }
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Unsubscribes from beat events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        OnBeat -= freeBeat;
        OnBeat -= chooseKey;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
    }

    #endregion
}
