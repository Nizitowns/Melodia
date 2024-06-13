using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static CommandManager;
using static RhythmManager;

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
    public enum LevelEventType { CUTSCENE, SIMON_SAYS, FREE_AREA, NEW_TRIBE, FINISH }

    [System.Serializable]
    public class LevelEvent
    {
        public string name; // This is here just for notes in the inspector
        public LevelEventType type;
        public float areaLength;
        public string screenText;
        public bool doCommand;
        public Command newTribeCommand;
        public Command simonCommand;
        public GameObject cutscene;
        public UnityEvent onFinish;
        public List<GameObject> objectsToDestroy;
    }

    [SerializeField]
    [Tooltip("The UI Canvas")]
    private Canvas canvas;

    [SerializeField]
    [Tooltip("The list of level events for this level.")]
    private List<LevelEvent> levelEvents;

    // Instances
    private RhythmManager rhythmManager;

    // Index of current event
    private int eventIndex;

    #endregion

    #region Initialization

    private void Start()
    {
        // Instances
        rhythmManager = RhythmManager.Instance;
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
                rhythmManager.setGameState(State.SIMONTEACH);
                break;
            case LevelEventType.NEW_TRIBE:
                rhythmManager.setSimonCommand(levelEvents[eventIndex].newTribeCommand);
                rhythmManager.setGameState(State.SIMONTEACH);
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

    #endregion

    #region Update

    private void Update()
    {
        doEvent();
    }

    #endregion
}
