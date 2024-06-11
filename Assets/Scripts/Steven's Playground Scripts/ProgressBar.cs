using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    // The Slider game object
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Add progress to the bar
    public void addProgress(float progress)
    {
        slider.value += progress;
    }

    // Remove progress from the bar
    public void removeProgress(float regress)
    {
        slider.value -= regress;
    }
}
