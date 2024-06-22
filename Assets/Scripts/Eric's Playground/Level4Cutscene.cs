using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Level4Cutscene : Cutscene
{
    [SerializeField]
    private Border border;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject background1;


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
            Color c = background1.GetComponent<Image>().color;
            background1.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            background1.GetComponent<Image>().enabled = true;
            if (alpha < 1f)
            {
                alpha += 0.5f * Time.deltaTime;
                background1.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
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
                Color c = background1.GetComponent<Image>().color;
                background1.GetComponent<Image>().color = new Color(c.r, c.g, c.b, alpha);
            }
            else
            {
                count++;
                alpha = 0f;
            }
        }

        if (count == 3)
        {
            if (timer < 1f)
                timer += Time.deltaTime;
            else
            {
                alpha = 1f;
                timer = 0f;
                count++;
            }
        }


        if (count == 4)
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


        if (count == 5)
        {
            over = true;
        }

    }
}