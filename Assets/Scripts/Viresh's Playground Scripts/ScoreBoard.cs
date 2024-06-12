using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the score display and position of the HUD
/// </summary>

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text score;
    // Start is called before the first frame update
    public static int scoreBoardValue;
    public enum Corner { TopLeft, TopRight, BottomLeft, BottomRight }
    public Corner corner = Corner.TopRight; // Default corner

    //public Vector2 offset = new Vector2(10, 10); // Offset from the corner

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Calculate screen corners
        Vector2 screenCorner = GetScreenCorner(corner);

        // Apply the offset to the screen corner
        Vector2 targetPosition = screenCorner;// + offset;

        // Set the position
        rectTransform.position = targetPosition;
    }

    Vector2 GetScreenCorner(Corner corner)
    {
        switch (corner)
        {
            case Corner.TopLeft:
                return new Vector2(Screen.width/12, Screen.height - Screen.height/8);
            case Corner.TopRight:
                return new Vector2(Screen.width, Screen.height);
            case Corner.BottomLeft:
                return new Vector2(0, 0);
            case Corner.BottomRight:
                return new Vector2(Screen.width, 0);
            default:
                return Vector2.zero;
        }
    }

    void ResetScore()
    {
        scoreBoardValue = 0;
    }

    void IncrementScore(int incVal)
    {
        scoreBoardValue += incVal;
        score.text = scoreBoardValue.ToString();
    }

    void DecrementScore(int decVal)
    {
        scoreBoardValue -= decVal;
        score.text = scoreBoardValue.ToString();
    }

}
