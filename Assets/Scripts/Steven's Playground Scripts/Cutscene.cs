using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Tooltip("Ensure cutscenes are disabled in the scene!")]
public abstract class Cutscene : MonoBehaviour
{
    #region Fields

    // Flag for when the cutscene is over
    [HideInInspector]
    public bool over = false;

    // Instances
    [HideInInspector]
    public RhythmManager rhythmManager;
    [HideInInspector]
    public MovementController movementController;
    [HideInInspector]
    public CommandManager commandManager;
    [HideInInspector]
    public InputReceiver inputReceiver;
    [HideInInspector]
    public UIEffects uiEffects;
    [HideInInspector]
    public SFXManager sfxManager;

    #endregion

    private void Start()
    {
        rhythmManager = RhythmManager.Instance;
        movementController = MovementController.Instance;
        commandManager = CommandManager.Instance;
        inputReceiver = InputReceiver.Instance;
        uiEffects = UIEffects.Instance;
        sfxManager = SFXManager.Instance;

        inputReceiver.DisableGameplayInput();

        rhythmManager.enabled = false;
        movementController.enabled = false;
        commandManager.enabled = false;
    }

    private void Update()
    {
        doCutscene();

        if (over)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        inputReceiver.EnableGameplayInput();
        rhythmManager.enabled = true;
        movementController.enabled = true;
        commandManager.enabled = true;
    }

    // Abstract class to fill in cutscene
    public abstract void doCutscene();
}
