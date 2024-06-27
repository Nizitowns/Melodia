using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Net.NetworkInformation;

public class Level5Cutscene_Outro : Cutscene
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

    //Waits till alpha =1 before incrementing count
    private void FadeIn(CanvasGroup group)
    {
        group.gameObject.SetActive(true);
        group.alpha = alpha;
        if (alpha < 1f)
        {
            alpha += 0.5f * Time.deltaTime;
            group.alpha = alpha;
        }
        else
        {
            count++;
            alpha = 1f;
        }
    }
    //Waits till alpha = 0 before incrementing count
    private void FadeOut(CanvasGroup group)
    {
        if (alpha > 0f)
        {
            alpha -= 0.5f * Time.deltaTime;
            group.alpha = alpha;
        }
        else
        {
            count++;
            alpha = 0f;
        }
    }
    //Waits num seconds before incrementing count
    private void WaitTime(float num)
    {
        if (timer < num)
            timer += Time.deltaTime;
        else
        {
            timer = 0f;
            count++;
        }
    }
    //Resets Alpha & Increments Count
    private void ResetState()
    {
        alpha = 1;
        count++;
    }
    override public void doCutscene()
    {
        switch(count)
        {
            case 0:
                border.setRestingAlpha(0f);
                FadeIn(background);
                FadeIn(background1);
                break;
            case 1:
                WaitTime(6);
                break;
            case 2:
                FadeOut(background1);
                break;
            case 3:
                FadeIn(background2);
                break;
            case 4:
                WaitTime(3);
                break;
            case 5:
                FadeOut(background2);
                break;
            case 6:
                ResetState();
                break;
            case 7:
                FadeOut(background);
                break;
            case 8:
                over = true;
                break;

        }

    }
}