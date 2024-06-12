using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackBoard : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static FeedbackBoard Instance { get; private set; }

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

    [SerializeField]
    [Tooltip("The amount of time the feedback is visible.")]
    private float displayTime = 0.5f;

    // Timer for being visible
    private float timer = 0.0f;

    #endregion

    #region Feedback

    /// <summary>
    /// Changes the text of the feedback board.
    /// </summary>
    public void setText(string feedback)
    {
        GameObject text = transform.Find("statMsg").gameObject;
        text.GetComponent<Text>().text = feedback;
    }

    /// <summary>
    /// Make the feedback board visible for some time.
    /// </summary>
    public void show()
    {
        timer = displayTime;
        transform.Find("statImg").gameObject.SetActive(true);
        transform.Find("statMsg").gameObject.SetActive(true);
    }

    #endregion

    #region Update

    private void Update()
    {
        if (timer > 0.0f)
            timer -= Time.deltaTime;
        else
        {
            transform.Find("statImg").gameObject.SetActive(false);
            transform.Find("statMsg").gameObject.SetActive(false);
        }
    }

    #endregion
}
