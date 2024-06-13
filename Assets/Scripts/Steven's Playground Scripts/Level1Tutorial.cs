using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Level1Tutorial : Cutscene
{
    [SerializeField]
    private InputActionAsset inputActions;

    [SerializeField]
    private GameObject background;

    // The bpm
    private float bpm = 120f;

    // Count for pattern
    private int count = 0;
    // Count for inputs
    private int inputs = 0;
    // Alpha for background
    private float alpha = 1f;

    override public void doCutscene()
    {
        if (count == 0)
        {
            count = 1;
            StartCoroutine(showPattern());
        }

        if (count == 5)
        {
            inputReceiver.EnableGameplayInput();
            inputReceiver.disableButton(1);
            inputReceiver.disableButton(2);
            inputReceiver.disableButton(3);

            InputActionMap gameplayActionMap = inputActions.FindActionMap("Gameplay");
            gameplayActionMap.FindAction("Button 4").performed += countInputs;
            count++;
        }

        if (inputs == 4)
        {

            rhythmManager.enabled = true;

            GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "Great job! There are four notes in one measure. By maintaining the beat for four counts (one measure), you will execute a \"command.\"\n\n(Do-Re → to continue)";
            inputs++;
        }

        if (inputs == 6)
        {
            InputActionMap gameplayActionMap = inputActions.FindActionMap("Gameplay");
            gameplayActionMap.FindAction("Button 4").performed -= countInputs;
            inputs++;
            
        }

        if (inputs == 7)
        {
            GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = "";
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                Color c = background.GetComponent<SpriteRenderer>().color;
                background.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
                inputs++;
            
        }

        if (inputs == 8)
        {
            over = true;
            inputReceiver.enableButton(1);
            inputReceiver.enableButton(2);
            inputReceiver.enableButton(3);
        }
            

    }

    private void countInputs(InputAction.CallbackContext context)
    {
        inputs++;
    }

    private IEnumerator showPattern()
    {
        while (count <= 4)
        {
            yield return new WaitForSeconds(60f / bpm);
            uiEffects.flashButton(4);
            sfxManager.playButtonSound(4);
            count++;
        }
    }
}
