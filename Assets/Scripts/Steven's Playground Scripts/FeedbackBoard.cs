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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    public enum Feedback {PERFECT, GREAT, OKAY, MISS}

    [SerializeField]
    [Tooltip("The amount of time the feedback is visible.")]
    private float displayTime = 0.5f;

    [SerializeField]
    [Tooltip("The sprite for the \"Perfect\" feedback.")]
    private Sprite perfect;

    [SerializeField]
    [Tooltip("The sprite for the \"Great\" feedback.")]
    private Sprite great;

    [SerializeField]
    [Tooltip("The sprite for the \"Okay\" feedback.")]
    private Sprite okay;

    [SerializeField]
    [Tooltip("The sprite for the \"Miss\" feedback.")]
    private Sprite miss;

    // Timer for being visible
    private float timer = 0.0f;

    #endregion

    #region Feedback

    /// <summary>
    /// Changes the text of the feedback board.
    /// </summary>
    public void setFeedback(Feedback feedback)
    {
        GameObject fb = transform.Find("message").gameObject;
        switch (feedback)
        {
            case Feedback.PERFECT:
                fb.GetComponent<Image>().sprite = perfect;
                break;
            case Feedback.GREAT:
                fb.GetComponent<Image>().sprite = great;
                break;
            case Feedback.OKAY:
                fb.GetComponent<Image>().sprite = okay;
                break;
            case Feedback.MISS:
                fb.GetComponent<Image>().sprite = miss;
                break;
        }
    }

    /// <summary>
    /// Make the feedback board visible for some time.
    /// </summary>
    public void show()
    {
        timer = displayTime;
        transform.Find("message").gameObject.SetActive(true);
        transform.Find("background").gameObject.SetActive(true);
    }

    #endregion

    #region Update

    private void Update()
    {
        if (timer > 0.0f)
            timer -= Time.deltaTime;
        else
        {
            transform.Find("message").gameObject.SetActive(false);
            transform.Find("background").gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        transform.Find("message").gameObject.GetComponent<Image>().SetNativeSize();
        RectTransform rect = transform.Find("message").gameObject.GetComponent<RectTransform>();
        transform.Find("message").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.sizeDelta.x / 8.5f, rect.sizeDelta.y / 8.5f);
    }

    #endregion
}
