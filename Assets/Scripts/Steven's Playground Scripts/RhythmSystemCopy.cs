using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Manages the rhythm for the game, generating beats at regular intervals and triggering events on each beat.
/// </summary>
public class RhythmSystemCopy : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static RhythmSystemCopy Instance { get; private set; }

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

    [System.Serializable]
    public class Command
    {
        public string name;
        public string actionName;
        public List<int> keys;
    }

    [SerializeField]
    [Tooltip("Beats per minute (BPM) for the rhythm.")]
    private float bpm = 120f;

    [SerializeField]
    [Tooltip("List of commands used in the game.")]
    private List<Command> commandList;

    [SerializeField]
    [Tooltip("Margin of error for a Perfect note, as a percentage of the beat interval.")]
    private float perfectMargin = 0.2f;

    [SerializeField]
    [Tooltip("Margin of error for an Excellent note, as a percentage of the beat interval.")]
    private float excellentMargin = 0.2f;

    [SerializeField]
    [Tooltip("Margin of error for a Good note, as a percentage of the beat interval.")]
    private float goodMargin = 0.3f;

    [SerializeField]
    [Tooltip("The amount the tribe moves forward.")]
    private float moveDistance = 1.0f;

    [SerializeField]
    [Tooltip("The amount the tribe moves forward in fever mode.")]
    private float feverMoveDistance = 2.0f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward when failing.")]
    private float regressDistance = 0.5f;

    [SerializeField]
    [Tooltip("The amount the tribe moves backward (each beat) after a lack of progress.")]
    private float driftDistance = 0.1f;

    [SerializeField]
    [Tooltip("The number of beats without progress before the tribe starts drifting backward.")]
    private int staticBeatLimit = 8;

    [SerializeField]
    [Tooltip("The Input Manager.")]
    private InputManagerCopy inputManager;

    [SerializeField]
    [Tooltip("The UnityEvent for the border flickering on the beat.")]
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

    [SerializeField]
    [Tooltip("The UnityEvent to progress forward.")]
    private UnityEvent<float> progressEvent;

    [SerializeField]
    [Tooltip("The UnityEvent to regress backward.")]
    private UnityEvent<float> regressEvent;

    // Possible gamestates
    public enum State{SIMONTEACH, SIMONPLAY, FREEPLAY};
    // Current gamestate
    private State gameState = State.SIMONTEACH;
    // The current Simon Says pattern
    private int chosenCommand;
    // List of keys for the current pattern
    private List<int> pattern = new List<int>();
    // Length of the commands
    private int patternLength = 4;
    // Number of beats played toward a command
    private int beatsPlayed = 0;
    // Flag for whether the player has played a note on this beat
    private bool beatUsed = false;
    // Time interval between beats
    private float beatInterval;
    // The number of beats since the tribe last progressed
    private int staticBeats = 0;
    // Tracking whether the tribe is drifting
    private bool drifting = false;
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
        InitializeRhythm();
        StartCoroutine(BeatRoutine());
        borderFlicker.Invoke();
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

            beatUsed = false;

            borderFlicker.Invoke();
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
    /// Shows the next Simon Says key.
    /// </summary>
    private void chooseKey()
    {
        List<int> chosenPattern = commandList[chosenCommand].keys;
        pattern.Add(chosenPattern[pattern.Count]);
        flashButton(pattern[pattern.Count - 1]);
    }

    /// <summary>
    /// Flashes the button symbol.
    /// </summary>
    private void flashButton(int button)
    {
        switch (button)
        {
            case 1:
                button1Event.Invoke();
                break;
            case 2:
                button2Event.Invoke();
                break;
            case 3:
                button3Event.Invoke();
                break;
            case 4:
                button4Event.Invoke();
                break;
        }
    }

    /// <summary>
    /// Checks if the player dropped a command by missing a beat.
    /// </summary>
    private void checkDroppedCommand()
    {
        if (pattern.Count > 0 && !beatUsed)
            missedNote();
    }

    /// <summary>
    /// Counts the number of beats since the tribe last progressed.
    /// </summary>
    private void incrementStaticBeats()
    {
        staticBeats++;
        if (staticBeats >= staticBeatLimit)
            drifting = true;
    }

    /// <summary>
    /// If the tribe is drifting, move them backward.
    /// </summary>
    private void drift()
    {
        if (staticBeats < staticBeatLimit)
            drifting = false;
        if (drifting)
            regressEvent.Invoke(driftDistance);
    }

    /// <summary>
    /// Sets the beat mode based on the game state.
    /// </summary>
    private void configureBeat()
    {
        OnBeat -= chooseKey;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
        OnBeat -= drift;
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
                OnBeat += drift;
                break;
        }
    }

    #endregion

    #region Player Input

    /// <summary>
    /// Receives a player input.
    /// </summary>
    public void pressButton(int button)
    {
        flashButton(button);

        // Check the note's timing
        if (checkTiming())
        {
            // If in Simon Says mode, check the input is the correct button
            if (gameState == State.SIMONPLAY)
            {
                if (button != pattern[beatsPlayed])
                    missedNote();
                else
                    beatsPlayed++;
            }
            // If in freeplay mode, check if the input goes toward a command
            else if (gameState == State.FREEPLAY)
            {
                if (pattern.Count == 0)
                    pattern.Add(button);
                else
                {
                    int patternLength = pattern.Count;
                    foreach (Command c in commandList)
                    {
                        bool following = true;
                        for (int i = 0; i < pattern.Count; i++)
                        {
                            if (c.keys[i] != pattern[i])
                            {
                                following = false;
                                break;
                            }
                        }
                        if (following && button == c.keys[pattern.Count])
                        {
                            pattern.Add(button);
                            break;
                        }
                    }
                    if (patternLength == pattern.Count)
                    {
                        missedNote();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks the played beat's timing. Returns false on missed notes.
    /// </summary>
    private bool checkTiming()
    {
        if (!beatUsed)
        {
            if (GetCurrentBeatProgress() < perfectMargin)
                print("Perfect");
            else if (GetCurrentBeatProgress() < perfectMargin + excellentMargin)
                print("Excellent");
            else if (GetCurrentBeatProgress() < perfectMargin + excellentMargin + goodMargin)
                print("Good");
            else
            {
                print("Miss");
                missedNote();
                beatUsed = true;
                return false;
            }
        }
        else
        {
            if (1 - GetCurrentBeatProgress() < perfectMargin)
                print("Perfect");
            else if (1 - GetCurrentBeatProgress() < perfectMargin + excellentMargin)
                print("Excellent");
            else if (1 - GetCurrentBeatProgress() < perfectMargin + excellentMargin + goodMargin)
                print("Good");
            else
            {
                print("Miss");
                missedNote();
                beatUsed = true;
                return false;
            }
        }

        beatUsed = true;

        return true;
    }

    /// <summary>
    /// Resets commands, combos, and fever mode on missed notes.
    /// </summary>
    private void missedNote()
    {
        if (gameState == State.SIMONPLAY)
        {
            // For demonstration purposes, switch to freeplay mode
            print("Wrong note!");
            beatsPlayed = 0;
            pattern = new List<int>();
            gameState = State.FREEPLAY;
        }
        else if (gameState == State.FREEPLAY)
        {
            print("Wrong note!");
            regressEvent.Invoke(regressDistance);
            pattern = new List<int>();
        }
    }

    #endregion

    #region Commands

    /// <summary>
    /// Performs the command based on the player's input.
    /// </summary>
    private void performCommand(string actionName)
    {
        switch (actionName)
        {
            case "move":
                move();
                break;
        }
    }

    /// <summary>
    /// Perform the "move" command
    /// </summary>
    private void move()
    {
        progressEvent.Invoke(moveDistance);
        staticBeats = 0;
    }

    #endregion

    #region Combos and Fever Mode

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
    /// Updates the beat timer and game state.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;

        switch (gameState)
        {
            case State.SIMONTEACH:
                inputManager.DisableGameplayInput();
                if (pattern.Count == patternLength)
                    gameState = State.SIMONPLAY;
                break;
            case State.SIMONPLAY:
                inputManager.EnableGameplayInput();
                if (beatsPlayed == patternLength)
                {
                    beatsPlayed = 0;
                    pattern = new List<int>();
                    chosenCommand = Random.Range(0, 4);
                    gameState = State.SIMONTEACH;
                }
                break;
            case State.FREEPLAY:
                if (pattern.Count == patternLength)
                {
                    int playedCommand = -1;
                    foreach (Command c in commandList)
                    {
                        playedCommand = commandList.IndexOf(c);
                        for (int i = 0; i < pattern.Count; i++)
                            if (c.keys[i] != pattern[i])
                                playedCommand = -1;
                        if (playedCommand != -1)
                            break;
                    }
                    if (playedCommand != -1)
                    {
                        print("Played Command: " + commandList[playedCommand].name);
                        performCommand(commandList[playedCommand].actionName);
                    }
                    else
                        print("Command not found!");
                    pattern = new List<int>();
                }
                break;
        }

        configureBeat();
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Unsubscribes from beat events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        OnBeat -= chooseKey;
        OnBeat -= checkDroppedCommand;
        OnBeat -= incrementStaticBeats;
        OnBeat -= drift;
    }

    #endregion
}