using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Manages the rhythm for the game, generating beats at regular intervals and triggering events on each beat.
/// </summary>
public class RhythmSystem_Modified : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static RhythmSystem_Modified Instance { get; private set; }

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

    #region Events

    // Event triggered on each beat
    public event System.Action OnBeat;

    #endregion

    #region Fields

    [SerializeField]
    [Tooltip("Beats per minute (BPM) for the rhythm.")]
    private float bpm = 120f;

    [SerializeField]
    [Tooltip("Number of keys for the Simon Says pattern.")]
    private int patternLength = 4;

    [SerializeField]
    [Tooltip("Margin of error for a Perfect note, as a percentage of the beat interval.")]
    private float perfectMargin = 0.1f;

    [SerializeField]
    [Tooltip("Margin of error for an Excellent note, as a percentage of the beat interval.")]
    private float excellentMargin = 0.2f;

    [SerializeField]
    [Tooltip("Margin of error for a Good note, as a percentage of the beat interval.")]
    private float goodMargin = 0.3f;

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

    // Possible gamestates
    private enum State { SIMONTEACH, SIMONPLAY, FREEPLAY };
    // Current gamestate
    private State gameState = State.SIMONTEACH;
    // List of keys for the current pattern
    private List<int> pattern = new List<int>();
    // Number of beats played after the Simon Says pattern is demonstrated
    private int beatsPlayed = 0;
    // Flag for whether the player has played a note on this beat
    private bool beatUsed = false;
    // Time interval between beats
    private float beatInterval;
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
    /// Chooses a key for the current beat.
    /// </summary>
    private void chooseKey()
    {
        int chosenKey = Random.Range(1, 5);
        pattern.Add(chosenKey);
        flashButton(chosenKey);
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
    /// Sets the beat mode based on the game state.
    /// </summary>
    private void configureBeat()
    {
        OnBeat -= chooseKey;
        switch (gameState)
        {
            case State.SIMONTEACH:
                OnBeat += chooseKey;
                break;
            case State.SIMONPLAY:
                break;
            case State.FREEPLAY:
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
        // If in Simon Says mode, check the input is the correct button
        if (gameState == State.SIMONPLAY)
        {
            if (button != pattern[beatsPlayed])
                print("Wrong note!");
            beatsPlayed++;
        }
        // Check the note's timing
        checkTiming();
    }

    /// <summary>
    /// Checks the played beat's timing.
    /// </summary>
    private void checkTiming()
    {
        string tempMsg = "";
        int scr = 0;
        if (!beatUsed)
        {
            if (timer < perfectMargin * beatInterval)
            {
                print("Perfect");
                tempMsg = "Perfect";
                scr = 3;
            }
                
       
            else if (timer - (perfectMargin * beatInterval) < excellentMargin * beatInterval)
            {
                print("Excellent");
                tempMsg = "Excellent";
                scr = 2;
            }
            else if (timer - (perfectMargin * beatInterval) - (excellentMargin * beatInterval) < goodMargin * beatInterval)
            {
                print("Good");
                tempMsg = "Good";
                scr = 1;
            }

            else
            {
                print("Miss");
                tempMsg = "Miss";
            }
                
        }
        else
        {
            if (beatInterval - timer < perfectMargin * beatInterval)
            {
                print("Perfect");
                tempMsg = "Perfect";
                scr = 3;
            }
            else if (beatInterval - timer - (perfectMargin * beatInterval) < excellentMargin * beatInterval)
            {
                tempMsg = "Excellent";
                print("Excellent");
                scr = 2;
            }
                
            else if (beatInterval - timer - (perfectMargin * beatInterval) - (excellentMargin * beatInterval) < goodMargin * beatInterval)
            {
                print("Good");
                tempMsg = "Good";
                scr = 1;
            }
                
            else
            {
                print("Miss");
                tempMsg = "Miss";
            }
        }

        hud.UpdateMessage(tempMsg.ToString());
        hud.DisplayStatus();
        hud.IncDecScore(scr);
        beatUsed = true;
    }

    #endregion

    #region Update

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
                    gameState = State.SIMONTEACH;
                }
                break;
            case State.FREEPLAY:
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
    }

    #endregion
}