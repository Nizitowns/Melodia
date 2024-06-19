using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 0.5f;

    private bool fadingIn = false, fadingOut = false;
    private float alpha = 1f;

    #region Singleton

    // Singleton instance
    public static LevelTransition Instance { get; private set; }

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

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
        fadeOut();
    }

    public void fadeIn()
    {
        fadingIn = true;
    }

    public void fadeOut()
    {
        fadingOut = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (fadingIn && alpha < 1f)
        {
            alpha += Time.deltaTime / fadeTime;
            if (alpha >= 1f)
            {
                alpha = 1f;
                fadingIn = false;
            }
        }

        if (fadingOut && alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeTime;
            if (alpha <= 0f)
            {
                alpha = 0f;
                fadingOut = false;
            }
        }

        GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }
}
