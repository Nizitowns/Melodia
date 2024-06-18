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
    private ButtonIndicator buttonDore;

    [SerializeField]
    private ButtonIndicator buttonLa;

    [SerializeField]
    private Border border;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject background1;

    [SerializeField]
    private GameObject background2;

    [SerializeField]
    private GameObject background3;

    // The bpm
    private float bpm = 120f;

    // Count for tutorial events
    private int count = 0;
    // Count for shown patterns
    private int shown = 0;
    // Count for inputs
    private int inputs = 0;
    private int doreCount = 0;
    private int laCount = 0;
    // Timer for beat timing
    private float timer = 0f;
    // Alpha for background
    private float alpha = 1f;

    override public void doCutscene()
    {
        if (count == 0)
        {
            border.setRestingAlpha(0f);
            buttonDore.setRestingAlpha(0.5f);
            buttonLa.setRestingAlpha(0.5f);
            count++;
            background1.GetComponent<Image>().enabled = true;
        }

        if (count == 1)
        {
            count++;
            StartCoroutine(showPattern1());
        }

        if (count == 2 && shown == 2)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                Color c = background1.GetComponent<Image>().color;
                background1.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                shown = 0;
                alpha = 0f;
            }
        }

        if (count == 3)
        {
            Color c = background2.GetComponent<Image>().color;
            background2.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            background2.GetComponent<Image>().enabled = true;
            if (alpha < 1f)
            {
                alpha += 0.5f * Time.deltaTime;
                background2.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 4)
        {
            count++;
            inputReceiver.EnableGameplayInput();
            inputReceiver.disableButton(1);
            inputReceiver.disableButton(3);
            border.setRestingAlpha(1f);
            StartCoroutine(startBeat());
        }

        if (count == 5)
        {
            count++;
            InputActionMap gameplayActionMap = inputActions.FindActionMap("Gameplay");
            gameplayActionMap.FindAction("Button 4").performed += countDoreInputs;
            gameplayActionMap.FindAction("Button 2").performed += countLaInputs;
        }

        if (count == 6)
        {
            if (inputs == 8)
            {
                buttonDore.setRestingAlpha(0f);
                buttonLa.setRestingAlpha(0f);
                inputReceiver.DisableGameplayInput();
                count++;
                timer = 0f;
            }
            else
            {
                if (doreCount == 4)
                {
                    buttonDore.setRestingAlpha(0f);
                    inputReceiver.disableButton(4);
                }
                if (laCount == 4)
                {
                    buttonLa.setRestingAlpha(0f);
                    inputReceiver.disableButton(2);
                }
                timer += Time.deltaTime;
            }
        }

        if (count == 7)
        {
            border.setRestingAlpha(0f);
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                Color c = background2.GetComponent<Image>().color;
                background2.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                alpha = 0f;
            }
        }

        if (count == 8)
        {
            Color c = background3.GetComponent<Image>().color;
            background3.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            background3.GetComponent<Image>().enabled = true;
            if (alpha < 1f)
            {
                alpha += 0.5f * Time.deltaTime;
                background3.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 9)
        {
            if (timer < 10)
                timer += Time.deltaTime;
            else
                count++;
        }

        if (count == 10)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                Color c = background3.GetComponent<Image>().color;
                background3.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 11)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                Color c = background.GetComponent<Image>().color;
                background.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
            }
        }

        if (count == 12)
        {
            over = true;
            border.setRestingAlpha(1f);
            inputReceiver.enableButton(1);
            inputReceiver.enableButton(2);
            inputReceiver.enableButton(3);
            inputReceiver.enableButton(4);
        }

    }

    private void countDoreInputs(InputAction.CallbackContext context)
    {
        if (doreCount < 4)
        {
            if (inputReceiver.checkTiming(false, timer / (60f / bpm)))
            {
                doreCount++;
                inputs++;
            }
        }
    }

    private void countLaInputs(InputAction.CallbackContext context)
    {
        if (laCount < 4)
        {
            if (inputReceiver.checkTiming(false, timer / (60f / bpm)))
            {
                laCount++;
                inputs++;
            }
        }
    }

    private IEnumerator startBeat()
    {
        while (count < 7)
        {
            yield return new WaitForSeconds(60f / bpm);
            timer = 0f;
            uiEffects.flickerBorder();
        }
    }

    private IEnumerator showPattern1()
    {
        while (shown < 2)
        {
            yield return new WaitForSeconds(60f / (bpm / 4));
            if (shown == 0)
            {
                uiEffects.flashButton(4);
                sfxManager.playButtonSound(4);
            }
            else
            {
                uiEffects.flashButton(2);
                sfxManager.playButtonSound(2);
            }
                
            shown++;
        }
    }
}
