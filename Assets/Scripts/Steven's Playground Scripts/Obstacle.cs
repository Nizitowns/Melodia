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

    public void enter()
    {
        entering = true;
    }

    public void joinTribe()
    {
        joining = true;
    }

    private void Update()
    {
        if (entering && transform.localPosition.x > 6f)
        {
            transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-3f * Time.deltaTime, 0f, 0f), Quaternion.identity);
        }
        else
            entering = false;

        if (joining)
        {
            float leftmost = 0f;
            foreach (Transform t in tribe.GetComponentInChildren<Transform>())
                if (t.tag == "Tribe" && t.localPosition.x < leftmost)
                    leftmost = t.localPosition.x;
            if (transform.localPosition.x > leftmost - 1.5f)
                transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-3f * Time.deltaTime, 0f, 0f), Quaternion.identity);
            else
            {
                tag = "Tribe";
                joining = false;
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
