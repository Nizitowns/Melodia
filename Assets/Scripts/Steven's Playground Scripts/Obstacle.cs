using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Tribe container (if this obstacle is a potential new tribe member)")]
    private GameObject tribe;

    // Flag for when obstacle is entering
    private bool entering = false;
    private bool joining = false;

    //Should the Obstacle Join when it spawns?
    [SerializeField]
    private bool joinOnStart=false;

    public void enter()
    {
        entering = true;
    }

    public bool isJoining()
    {
        return joining;
    }

    public void joinTribe()
    {
        joining = true;
    }

    public void clear()
    {
        Destroy(gameObject);
    }

    private void Update()
    {

        if (joinOnStart&&Time.timeSinceLevelLoad>3f)
        {
            joinTribe();
        }

        if (entering && transform.localPosition.x > 6f)
        {
            if (gameObject.GetComponent<Animator>())
                gameObject.GetComponent<Animator>().enabled = true;
            transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-5f * Time.deltaTime, 0f, 0f), Quaternion.identity);
        }
        else
        {
            if (gameObject.GetComponent<Animator>())
                gameObject.GetComponent<Animator>().enabled = false;
            entering = false;
        }

        if (joining)
        {
            if (gameObject.GetComponent<Animator>())
                gameObject.GetComponent<Animator>().enabled = true;
            float leftmost = 0f;
            foreach (Transform t in tribe.GetComponentInChildren<Transform>())
                if (t.tag == "Tribe" && t.localPosition.x < leftmost&&t!=transform)
                    leftmost = t.localPosition.x;
            if (transform.localPosition.x > leftmost - 3f)
                transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-3f * Time.deltaTime, 0f, 0f), Quaternion.identity);
            else
            {
                tag = "Tribe";
                joining = false;
                joinOnStart = false;
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
