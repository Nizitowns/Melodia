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
    private int staticBeatLimit = 10;

    // Instances
    private InputReceiver inputReceiver;
    private UIEffects uiEffects;
    private CommandManager commandManager;
    private MovementController movementController;
    private SFXManager sfx;
    private MusicManager musicManager;

    // Possible gamestates
    public enum State { SIMONTEACH, SIMONPLAY, FREEPLAY };
    // Current gamestate
    private State gameState = State.FREEPLAY;
    // List of keys for the current Simon Says pattern
    private List<int> pattern = new List<int>();
    // List of keys for the current command string
    private List<int> commandString = new List<int>();
    // Length of the commands
    private int patternLength = 4;
    // Number of beats played toward a command
    private int beatsPlayed = 0;
    // Flag for whether the player played a note on the last beat
    private bool lastBeatUsed = false;
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
        sfx = SFXManager.Instance;
        musicManager = MusicManager.Instance;

        InitializeRhythm();
        InitializeSimon();
        InitializeBeat();

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

    /// <summary>
    /// Initializes the beat system, syncing it with the music playback (and starting the music, consequentially).
    /// </summary>
    private void InitializeBeat()
    {
        musicManager.syncBeat();
    }

    #endregion

    #region Beat Management

    /// <summary>
    /// Gets the progress of the current beat as a value between 0 and 1.
    /// </summary>
    /// <returns>Float value representing the progress of the current beat.</returns>
    public float GetCurrentBeatProgress()
    {
        return timer / beatInterval;
    }

    /// <summary>
    /// Invokes the beat functions.
    /// </summary>
    public void beat()
    {
        timer = 0f;
        OnBeat?.Invoke();

        uiEffects.flickerBorder();
    }

    /// <summary>
    /// Free the beat to be used, if it hasn't already been used (happens every beat, no matter what).
    /// </summary>
    private void freeBeat()
    {
        if (!beatUsed && !nextBeatUsed)
            lastBeatUsed = false;
        else
            lastBeatUsed = true;

        if (!nextBeatUsed)
            beatUsed = false;
        else
        {
            beatUsed = true;
            nextBeatUsed = false;
        } 
    }

    /// <summary>
    /// Shows the next Simon Says key.
    /// </summary>
    private void chooseKey()
    {
        if (!beatUsed)
        {
            uiEffects.flashButton(pattern[beatsPlayed]);
            sfx.playButtonSound(pattern[beatsPlayed]);
            beatsPlayed++;
        }
    }

    /// <summary>
    /// Checks if the player dropped a command by missing a beat (during Simon Says).
    /// </summary>
    private void checkDroppedSimon()
    {
        if (beatsPlayed > 0 && !lastBeatUsed)
            inputReceiver.missedNote(gameState);
    }

    /// <summary>
    /// Checks if the player dropped a command by missing a beat (during Freeplay).
    /// </summary>
    private void checkDroppedCommand()
    {
        if (commandString.Count > 0 && !lastBeatUsed)
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
        movementController.drift();
    }

    /// <summary>
    /// Sets the beat mode based on the game state.
    /// </summary>
    private void configureBeat()
    {
        OnBeat -= chooseKey;
        OnBeat -= checkDroppedSimon;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
        switch (gameState)
        {
            case State.SIMONTEACH:
                OnBeat += chooseKey;
                break;
            case State.SIMONPLAY:
                OnBeat += checkDroppedSimon;
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
        beatsPlayed = 0;
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
        beatsPlayed++;
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
    /// Resets the static beat counter.
    /// </summary>
    public void resetStaticBeats()
    {
        staticBeats = 0;
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
        OnBeat -= checkDroppedSimon;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
    }

    #endregion
}
