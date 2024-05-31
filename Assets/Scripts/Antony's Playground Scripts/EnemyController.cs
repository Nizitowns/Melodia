using UnityEngine;

/// <summary>
/// Controls the behavior of enemy units, including movement and actions based on AI logic.
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("Movement speed of the enemy unit.")]
    private float speed = 2f;

    [SerializeField]
    [Tooltip("Health of the enemy unit.")]
    private int health = 100;

    [SerializeField]
    [Tooltip("Attack power of the enemy unit.")]
    private int attackPower = 10;

    [SerializeField]
    [Tooltip("Attack range of the enemy unit.")]
    private float attackRange = 1.5f;

    [SerializeField]
    [Tooltip("Cooldown time between attacks.")]
    private float attackCooldown = 1.0f;

    private float lastAttackTime;
    private Transform player;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the enemy unit.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        lastAttackTime = -attackCooldown; // Allow immediate attack at start
    }

    #endregion

    #region Update

    /// <summary>
    /// Called once per frame.
    /// Updates the movement and actions of the enemy unit.
    /// </summary>
    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                if (CanAttack())
                {
                    AttackPlayer();
                }
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Moves the enemy unit towards the player.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    #endregion

    #region Attack Management

    /// <summary>
    /// Checks if the enemy unit can attack based on the attack cooldown.
    /// </summary>
    /// <returns>True if the enemy can attack, false otherwise.</returns>
    private bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    /// <summary>
    /// Attacks the player and updates the last attack time.
    /// </summary>
    private void AttackPlayer()
    {
        if (player != null)
        {
            lastAttackTime = Time.time;
            player.GetComponent<PlayerStats>().TakeDamage(attackPower);
            Debug.Log("Enemy attacking player with power: " + attackPower);
        }
    }

    #endregion

    #region Health Management

    /// <summary>
    /// Reduces the health of the enemy unit by the specified damage amount.
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
    /// Handles the death of the enemy unit.
    /// </summary>
    private void Die()
    {
        // Implement death logic here
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    #endregion

    #region AI Actions

    /// <summary>
    /// Performs the action determined by the AI.
    /// </summary>
    public void PerformAction()
    {
        // Implement AI action logic here
        // This method can be called by AIManager to control enemy actions
    }

    #endregion
}
