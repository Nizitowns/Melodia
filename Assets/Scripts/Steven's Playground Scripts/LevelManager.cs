using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public CommandManager.Command newTribeCommand;
        public CommandManager.Command simonCommand;
        public GameObject cutscene;
        public List<GameObject> objectsToDestroy;
    }

    [SerializeField]
    [Tooltip("The list of level events for this level.")]
    private List<LevelEvent> levelEvents;

    // Index of current event
    private int eventIndex;

    #endregion

    #region Events

    private void nextEvent()
    {
        eventIndex++;
    }

    private void doEvent()
    {
        LevelEvent currentEvent = levelEvents[eventIndex];

        switch (currentEvent.type) 
        {
            case LevelEventType.CUTSCENE:
                if (currentEvent.cutscene)
                    cutscene(currentEvent.cutscene.GetComponent<Cutscene>());
                else
                    nextEvent();
                break;
            case LevelEventType.SIMON_SAYS:
                break;
            case LevelEventType.FREE_AREA:
                break;
            case LevelEventType.NEW_TRIBE:
                break;
        }
    }

    private void cutscene(Cutscene cs)
    {
        cs.enabled = true;
    }

    #endregion

    #region Update

    private void Update()
    {
        doEvent();
    }

    #endregion
}
