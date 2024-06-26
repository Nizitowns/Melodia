using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Level2Cutscene : Cutscene
{
    [SerializeField]
    private Border border;

    [SerializeField]
    private CanvasGroup background;

    [SerializeField]
    private CanvasGroup background1;

    [SerializeField]
    private CanvasGroup background2;

    // Count for cutscene events
    private int count = 0;
    // Timer for screen timing
    private float timer = 0f;
    // Alpha for background
    private float alpha = 0f;

    override public void doCutscene()
    {
        if (count == 0)
        {
            border.setRestingAlpha(0f);
            background1.alpha = alpha;
            background1.gameObject.SetActive(true);
            if (alpha < 1f)
            {
                alpha += 0.5f * Time.deltaTime;
                background1.alpha = alpha;
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 1)
        {
            if (timer < 3)
                timer += Time.deltaTime;
            else
                count++;
        }

        if (count == 2)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                background1.alpha = alpha;
            }
            else
            {
                count++;
                alpha = 0f;
            }
        }

        if (count == 3)
        {
            background2.gameObject.SetActive(true);
            if (alpha < 1f)
            {
                alpha += 0.5f * Time.deltaTime;
                background2.alpha = alpha;
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 4)
        {
            if (timer < 3)
                timer += Time.deltaTime;
            else
            {
                timer = 0f;
                count++;
            }
        }

        if (count == 5)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                background2.alpha = alpha;
            }
            else
            {
                count++;
                alpha = 1f;
            }
        }

        if (count == 6)
        {
            if (alpha > 0f)
            {
                alpha -= 0.5f * Time.deltaTime;
                background.alpha = alpha;
            }
            else
            {
                count++;
            }
        }

        if (count == 7)
        {
            over = true;
        }

    }
}