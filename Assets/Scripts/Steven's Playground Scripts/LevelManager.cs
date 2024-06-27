using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static CommandManager;
using static RhythmManager;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static LevelManager Instance { get; private set; }

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

    #region Fields

    [System.Serializable]
    public enum LevelEventType { CUTSCENE, SIMON_SAYS, FREE_AREA, NEW_TRIBE, OBSTACLE, FINISH }

    [System.Serializable]
    public class LevelEvent
    {
        public string name; // This is here just for notes in the inspector
        public LevelEventType type; // What kind of level event this is
        public float areaLength; // How long the area is (FREE_AREA)
        public string screenText; // What text to display on the screen (placeholder for quick testing)
        public bool doCommand; // Whether the game should execute the command completed during simon says (SIMON_SAYS, NEW_TRIBE)
        public bool doSimon; // Whether the game should perform the teaching portion of simon says (SIMON_SAYS, NEW_TRIBE, OBSTACLE)
        public int repetitions; // How many repetitions must be completed to progress past simon says (SIMON_SAYS, NEW_TRIBE, OBSTACLE)
        public GameObject obstacle; // The GameObject that represents the obstacle or new tribe (NEW_TRIBE, OBSTACLE)
        public Command newTribeCommand; // What command the new tribe is teaching the player (NEW_TRIBE)
        public Command obstacleCommand; // What command must be input to clear the obstacle (OBSTACLE)
        public Command simonCommand; // What command is taught during simon says (SIMON_SAYS)
        public GameObject cutscene; // The GameObject containing the Cutscene script (CUTSCENE)
        public string nextLevel; // The name of the scene that contains the next level (FINISH)
        public UnityEvent onFinish; // Function to execute when the event is finished (animations, etc. before next event)
        public List<GameObject> objectsToDestroy; // Objects to be destroyed when the event is finished (obstacles, cutscene-specific objects, etc.)
    }

    [SerializeField]
    [Tooltip("The UI Canvas")]
    private Canvas canvas;

    [SerializeField]
    [Tooltip("The list of level events for this level.")]
    private List<LevelEvent> levelEvents;

    [SerializeField]
    [Tooltip("The summed lenght of the walking areas for this level.")]
    private float levelLength=10;
    // Instances
    private RhythmManager rhythmManager;
    private LevelTransition levelTransition;
    private InputReceiver inputReceiver;

    // Index of current event
    private int eventIndex;

    #endregion

    #region Initialization

    private void Start()
    {
        // Instances
        rhythmManager = RhythmManager.Instance;
        levelTransition = LevelTransition.Instance;
        inputReceiver = InputReceiver.Instance;

        setRhythmState();
    }

    #endregion

    #region Events

    private void nextEvent()
    {
        levelEvents[eventIndex].onFinish?.Invoke();
        eventIndex++;
        Text text = canvas.transform.Find("Text").gameObject.GetComponent<Text>();
        text.text = levelEvents[eventIndex].screenText.Replace("\\n", "\n");
        setRhythmState();
    }

    private void setRhythmState()
    {
        switch (levelEvents[eventIndex].type)
        {
            case LevelEventType.SIMON_SAYS:
                rhythmManager.setSimonCommand(levelEvents[eventIndex].simonCommand);
                if (levelEvents[eventIndex].doSimon)
                    rhythmManager.setGameState(State.SIMONTEACH);
                else
                    rhythmManager.setGameState(State.SIMONPLAY);
                break;
            case LevelEventType.NEW_TRIBE:
                bringObstacle();
                rhythmManager.setSimonCommand(levelEvents[eventIndex].newTribeCommand);
                if (levelEvents[eventIndex].doSimon)
                    rhythmManager.setGameState(State.SIMONTEACH);
                else
                    rhythmManager.setGameState(State.SIMONPLAY);
                break;
            case LevelEventType.OBSTACLE:
                bringObstacle();
                rhythmManager.setSimonCommand(levelEvents[eventIndex].obstacleCommand);
                if (levelEvents[eventIndex].doSimon)
                    rhythmManager.setGameState(State.SIMONTEACH);
                else
                    rhythmManager.setGameState(State.SIMONPLAY);
                break;
            case LevelEventType.FREE_AREA:
                rhythmManager.setGameState(State.FREEPLAY);
                break;
            default:
                rhythmManager.setGameState(State.DEFAULT);
                break;
        }
    }

    public LevelEvent getCurrentEvent()
    {
        return levelEvents[eventIndex];
    }

    private void bringObstacle()
    {
        if (levelEvents[eventIndex].obstacle)
        {
            GameObject obstacle = levelEvents[eventIndex].obstacle;
            obstacle.GetComponent<Obstacle>().enter();
        }
    }

    private void doEvent()
    {
        LevelEvent currentEvent = levelEvents[eventIndex];

        if (currentEvent.type == LevelEventType.CUTSCENE) 
        {
            if (currentEvent.cutscene)
                cutscene(currentEvent.cutscene.GetComponent<Cutscene>());
            else
                nextEvent();
        }
        else if (currentEvent.type == LevelEventType.FINISH)
        {
            inputReceiver.DisableGameplayInput();
            goToNextLevel();
        }
    }

    private void cutscene(Cutscene cs)
    {
        cs.enabled = true;
    }

    public void finishEvent()
    {
        Text text = canvas.transform.Find("Text").gameObject.GetComponent<Text>();
        text.text = "";
        nextEvent();
    }

    private void goToNextLevel()
    {
        if (levelEvents[eventIndex].nextLevel != "")
        {
            StartCoroutine(loadNextLevel());
        }
    }

    private IEnumerator loadNextLevel()
    {
        yield return new WaitForSeconds(3);

        levelTransition.fadeIn();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelEvents[eventIndex].nextLevel);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        levelTransition.fadeOut();
    }

    #endregion

    #region Update

    
    float summed_progress=0;
    float last_progress=0;
    int lastEventIndex;
    private void Update()
    {
        float cur_progress = summed_progress + MovementController.Instance.progress;
        if(lastEventIndex!=eventIndex)
        {
            summed_progress = last_progress;
            lastEventIndex = eventIndex;
        }
        last_progress = cur_progress;
        Debug.Log(cur_progress + " / " + levelLength);
        ProgBarVisualizer.Instance?.UpdateValue((cur_progress/ levelLength)*100f);
        doEvent();
    }

    #endregion
}
