using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgBarVisualizer : MonoBehaviour
{
    [Header("Visualized Value")]
    [Range(0,100f)]
    public float LevelProgress;

    [Header("Level Parameters")]
    public float Bar1StartsAt = 0;
    public float Bar2StartsAt = 33;
    public float Bar3StartsAt = 66;

    [Header("References")]
    public Slider Slider1;
    public Slider Slider2;
    public Slider Slider3;

    public Image Star1;
    public Image Star2;
    public Image Star3;
    private void FixedUpdate()
    {
        UpdateValue(LevelProgress);
    }
    public void UpdateValue(float newVal)
    {
        LevelProgress = newVal;

        Slider1.value = Mathf.Max(0, (LevelProgress - Bar1StartsAt) / (Bar2StartsAt - Bar1StartsAt));
        Slider2.value = Mathf.Max(0, (LevelProgress - Bar2StartsAt) / (Bar3StartsAt - Bar2StartsAt));
        Slider3.value = Mathf.Max(0, (LevelProgress - Bar3StartsAt) / (100 - Bar3StartsAt));

        Star1.enabled = Slider1.value >= 1;
        Star2.enabled = Slider2.value >= 1;
        Star3.enabled = Slider3.value >= 1;
    }
}
