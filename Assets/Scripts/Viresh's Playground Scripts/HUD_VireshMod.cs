using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the HUD (Heads-Up Display) elements in the game, including health and score displays.
/// Responds to changes in player stats and updates the HUD accordingly.
/// </summary>
public class HUD_VireshMod : MonoBehaviour
{
    #region UI References

    [SerializeField] private Text healthText;
    [SerializeField] private Text scoreText;

    private int score;
    /// <summary>
    /// Initialize the Corners and positions
    /// </summary>
    public enum Corner { TopLeft, TopCentre, TopRight, BottomLeft, BottomCentre, BottomRight }
    [SerializeField] private GameObject ScoreBoard;

    public Corner ScoreBoardPosition = Corner.TopLeft;

    [SerializeField] private GameObject ProgressBar;

    public Corner ProgressBarPosition = Corner.TopCentre;
    [SerializeField] private GameObject _bar;
    [SerializeField] private GameObject _check;
    [SerializeField] private GameObject _star;

    private float ProgressBarField;

    public int levels = 3;
    public int curLevel = 0;
    private int curProg;

    [SerializeField] private GameObject status;
    private string statusMessage;

    public float animationDuration = 1.0f;
    public float stayDuration = 1.5f;
    #endregion

    #region Initialization
    /// <summary>
    /// Called just before Start function
    /// Called regardless of whether the GameObject is active or not.
    /// </summary>
    private void Awake()
    {
        ProgressBarField = (3 * Screen.width / 5) - ScoreBoard.GetComponent<RectTransform>().sizeDelta.x - (2 * _check.GetComponent<RectTransform>().sizeDelta.x);
    }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Subscribes to player stats change events.
    /// </summary>
    private void Start()
    {
        PlayerStats.OnHealthChanged += UpdateHealth;
        PlayerStats.OnScoreChanged += UpdateScore;

        // Update Position
        // ScoreBoard
        Reposition(ScoreBoard, ScoreBoardPosition);
        // Progress Bar
        Reposition(ProgressBar, ProgressBarPosition);

        //Set Progress Bar
        SetProgressBar(levels);

        //Set current level at start
        curLevel = 0;
        curProg = curLevel;

        //Set  start score
        score = 0;
    }

    /// <summary>
    /// Unsubscribes from player stats change events when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        PlayerStats.OnHealthChanged -= UpdateHealth;
        PlayerStats.OnScoreChanged -= UpdateScore;
    }

    #endregion

    #region UI Update

    /// <summary>
    /// Updates the health display in the HUD.
    /// </summary>
    /// <param name="health">The current health of the player.</param>
    private void UpdateHealth(int health)
    {
        healthText.text = "Health: " + health.ToString();
    }

    /// <summary>
    /// Updates the score display in the HUD.
    /// </summary>
    /// <param name="newScore">The new score of the player.</param>
    private void UpdateScore(int newScore)
    {
        score = newScore;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Updates the position of display.
    /// </summary>
    /// <param name="obj">The HUD to reposition.</param>
    /// <param name="corner">The corner to place the object.</param>

    private void Reposition(GameObject obj, Corner corner)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();

        // Calculate screen corners
        Vector2 screenCorner = GetScreenCorner(corner);

        // Apply the offset to the screen corner
        Vector2 targetPosition = screenCorner;

        // Set the position
        rectTransform.position = targetPosition;
    }
    #endregion
    #region UI Boundary functions

    /// <summary>
    /// Select the corner of the display and set the positions value.
    /// </summary>
    /// <param name="obj">The HUD to reposition.</param>
    /// <param name="corner">The corner to place the object.</param>
    Vector2 GetScreenCorner(Corner corner)
    {
        switch (corner)
        {
            case Corner.TopLeft:
                return new Vector2((2 * ScoreBoard.GetComponent<RectTransform>().sizeDelta.x / 3), Screen.height - Screen.height / 8);
            case Corner.TopRight:
                return new Vector2(Screen.width - (9 * status.GetComponent<RectTransform>().sizeDelta.x / 3), Screen.height - (3 * Screen.height / 8));
            case Corner.BottomLeft:
                return new Vector2(Screen.width / 20, Screen.height / 8);
            case Corner.BottomRight:
                return new Vector2(Screen.width, 0);
            case Corner.TopCentre:
                return new Vector2((Screen.width / 2) + (ScoreBoard.GetComponent<RectTransform>().sizeDelta.x / 2), Screen.height - Screen.height / 8);
            case Corner.BottomCentre:
                return new Vector2(Screen.width / 2, 0);
            default:
                return Vector2.zero;
        }
    }

    /// <summary>
    /// Select the corner of the display and set the positions value.
    /// </summary>
    /// <param name="obj">The HUD to reposition.</param>
    /// <param name="corner">The corner to place the object.</param>
    [System.Obsolete]
    private void SetProgressBar(int levels)
    {
        GameObject pan = ProgressBar.transform.GetChild(0).gameObject;
        DeleteAllChildren(pan);
        for (int i = 0; i < levels; i++)
        {
            GameObject newBar = Instantiate(_bar);
            newBar.GetComponent<RectTransform>().sizeDelta = new Vector2(ProgressBarField / (levels + 1), 100);
            ChangeColor(newBar, Color.grey);
            newBar.transform.SetParent(pan.transform);
            if (i != levels - 1)
            {
                GameObject newCircle = Instantiate(_check);
                newCircle.transform.SetParent(pan.transform);
            }
            else
            {
                GameObject newStar = Instantiate(_star);
                newStar.transform.SetParent(pan.transform);
            }

        }
    }

    /// <summary>
    /// Clear the UI object
    /// </summary>
    /// <param name="parent">The element which is intended to clear of all child objects.</param>
    private void DeleteAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region UI Value Update
    /// <summary>
    /// Set Level for progressBar
    /// </summary>
    /// <param name="levels_num">The element which is intended to clear of all child objects.</param>
    public void SetLevels(int levels_num)
    {
        levels = levels_num;
    }

    /// <summary>
    /// Set Level for progressBar
    /// </summary>
    /// <param name="obj">The element which has image component</param>
    /// <param name="color">The updated color of progress bar</param>
    public void ChangeColor(GameObject obj, Color color)
    {
        Image img = obj.GetComponent<Image>();
        img.color = color;
    }
    /// <summary>
    /// Diplay progression on the progress bar and checkpoint
    /// </summary>
    public void ProgressLevelBar()
    {
        GameObject pan = ProgressBar.transform.GetChild(0).gameObject;
        GameObject levelProg = pan.transform.GetChild(curProg).gameObject;
        Color _complete = Color.white;
        ChangeColor(levelProg, _complete);
        curProg++;
        if(curProg % 2 == 1)
        {
            curLevel++;
        }
    }

    public void IncDecScore(int val)
    {
        score += val;
        UpdateScore(score);
    }

    public void UpdateMessage(string msg)
    {
        statusMessage = msg;
    }

    public void DisplayStatus()
    {
        GameObject newStatus = Instantiate(status);
        newStatus.transform.SetParent(ScoreBoard.transform);
        Reposition(newStatus, Corner.TopRight);
        Text st = newStatus.transform.GetChild(1).gameObject.GetComponent<Text>();
        st.text = statusMessage;
        StartCoroutine(ScaleOverTime(newStatus, animationDuration));

    }

    private IEnumerator ScaleOverTime(GameObject target, float duration)
    {
        Vector3 originalScale = target.transform.localScale;
        target.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            target.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = originalScale;
        yield return new WaitForSeconds(stayDuration);
        Destroy(target);
    }
    #endregion
    
    [System.Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DisplayStatus();
            //ProgressLevelBar();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            IncDecScore(1);
            ProgressLevelBar();
        }
    }
    

}
