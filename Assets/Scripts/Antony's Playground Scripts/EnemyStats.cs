using UnityEngine;

/// <summary>
/// Manages the stats of the enemy unit, including health, attack power, and attack cooldown.
/// </summary>
public class EnemyStats : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("Health of the enemy unit.")]
    private int health = 100;

    [SerializeField]
    [Tooltip("Attack power of the enemy unit.")]
    private int attackPower = 10;

    [SerializeField]
    [Tooltip("Cooldown time between attacks.")]
    private float attackCooldown = 1.0f;

    private float lastAttackTime;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the enemy stats.
    /// </summary>
    private void Start()
    {
        lastAttackTime = -attackCooldown; // Allow immediate attack at start
    }

    #endregion

    #region Attack Management

    /// <summary>
    /// Checks if the enemy unit can attack based on the attack cooldown.
    /// </summary>
    /// <returns>True if the enemy can attack, false otherwise.</returns>
    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    /// <summary>
    /// Performs the attack action and updates the last attack time.
    /// </summary>
    public void Attack()
    {
        if (CanAttack())
        {
            lastAttackTime = Time.time;
            // Implement attack logic here
            Debug.Log("Enemy attacking with power: " + attackPower);
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
}
