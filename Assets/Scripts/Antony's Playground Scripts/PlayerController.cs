using UnityEngine;

/// <summary>
/// Controls the behavior of player units, including movement and actions based on commands.
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("Movement speed of the player unit.")]
    private float speed = 2f;

    [SerializeField]
    [Tooltip("Health of the player unit.")]
    private int health = 100;

    private Vector3 targetPosition;
    private bool isMoving;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the player unit.
    /// </summary>
    private void Start()
    {
        targetPosition = transform.position;
    }

    #endregion

    #region Update

    /// <summary>
    /// Called once per frame.
    /// Updates the movement of the player unit.
    /// </summary>
    private void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    #endregion

    #region Command Handling

    /// <summary>
    /// Executes a command on the player unit.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void ExecuteCommand(string command)
    {
        switch (command)
        {
            case "Move":
                SetTargetPosition(transform.position + Vector3.right * 5f);
                break;
            case "Attack":
                Attack();
                break;
            case "Defend":
                Defend();
                break;
            default:
                Debug.LogWarning("Unknown command: " + command);
                break;
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Sets the target position for the player unit to move to.
    /// </summary>
    /// <param name="position">The target position.</param>
    private void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        isMoving = true;
    }

    /// <summary>
    /// Moves the player unit towards the target position.
    /// </summary>
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

    #endregion

    #region Actions

    /// <summary>
    /// Performs the attack action.
    /// </summary>
    private void Attack()
    {
        // Implement attack logic here
        Debug.Log("Player attacking");
    }

    /// <summary>
    /// Performs the defend action.
    /// </summary>
    private void Defend()
    {
        // Implement defend logic here
        Debug.Log("Player defending");
    }

    #endregion

    #region Health Management

    /// <summary>
    /// Reduces the health of the player unit by the specified damage amount.
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles the death of the player unit.
    /// </summary>
    private void Die()
    {
        // Implement death logic here
        Debug.Log("Player died");
        Destroy(gameObject);
    }

    #endregion
}
