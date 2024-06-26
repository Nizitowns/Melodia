using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the parallax scrolls.")]
    private float moveSpeed;

    [SerializeField]
    [Tooltip("Whether the parallax is scrolling left.")]
    private bool movingLeft;

    [SerializeField]
    [Tooltip("The Tribe container.")]
    private GameObject tribe;

    // How far to move
    private float distanceToMove = 0f;
    // Width of one of the tiled textures
    private float singleTextureWidth;

    private void Start()
    {
        SetupTexture();
        if (movingLeft)
            moveSpeed = -moveSpeed;
    }

    private void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    private void Scroll()
    {
        float delta = moveSpeed * Time.deltaTime;
        if (Mathf.Abs(distanceToMove) < Mathf.Abs(Time.deltaTime))
            distanceToMove = 0f;
        else
            distanceToMove -= Time.deltaTime * Mathf.Sign(distanceToMove);
        transform.position += new Vector3(delta, 0f, 0f);
    }

    private void CheckReset()
    {
        if ((Mathf.Abs(transform.position.x) - singleTextureWidth) > 0)
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
    }

    public void addDistance(float distance)
    {
        distanceToMove += distance;
    }

    public float getSpeed()
    {
        return moveSpeed;
    }

    public void setMoveLeft(bool left)
    {
        if (movingLeft != left)
            moveSpeed = -moveSpeed;
        movingLeft = left;
    }

    private void Update()
    {
        if (Mathf.Abs(distanceToMove) > 0f)
        {
            foreach (Transform t in tribe.GetComponentsInChildren<Transform>())
            {
                if (t.gameObject.tag == "Tribe")
                {
                    t.gameObject.GetComponent<Animator>().enabled = true;
                }
            }
            Scroll();
            CheckReset();
        }
        else
        {
            foreach (Transform t in tribe.GetComponentsInChildren<Transform>())
            {
                if (t.gameObject.tag == "Tribe")
                {
                    if (!t.gameObject.GetComponent<Obstacle>() || !t.gameObject.GetComponent<Obstacle>().isJoining())
                        t.gameObject.GetComponent<Animator>().enabled = false;
                }
            }
        }
    }
}
