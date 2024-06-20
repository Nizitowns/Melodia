using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

/// <summary>
/// Handles the scene transition after button is pressed
/// </summary>
public class ButtonTransition : MonoBehaviour
{
    public void LoadNextLevel()
    {
        //Can be modified to load other scenes
        SceneManager.LoadScene("Level2");
    }
}
