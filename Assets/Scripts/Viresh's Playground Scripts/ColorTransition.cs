using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Color startColor = Color.blue; 
    public Color endColor = Color.cyan;   
    public float duration = 5f;           

    private float timeElapsed = 0f;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Set the initial color
        spriteRenderer.color = startColor;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        float t = timeElapsed / duration;

        spriteRenderer.color = Color.Lerp(startColor, endColor, t);

        if (t >= 1f && t < 2f)
        {
            timeElapsed = 0f;

            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
        if (t >= 1f && t < 2f)
        {
            timeElapsed = 0f;

            Color temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
    }
}
