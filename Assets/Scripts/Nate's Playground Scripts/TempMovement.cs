using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the character
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input for horizontal movement only
        movement.x = Input.GetAxis("Horizontal");
        movement.y = 0; // Ignore vertical input
    }

    void FixedUpdate()
    {
        // Apply horizontal movement to the Rigidbody
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}