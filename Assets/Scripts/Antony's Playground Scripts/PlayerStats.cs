using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Fields

    [SerializeField]
    [Tooltip("Health of the player unit.")]
    private int health = 100;

    [SerializeField]
    [Tooltip("Score of the player unit.")]
    private int score = 0;

    [SerializeField]
    [Tooltip("Attack power of the player unit.")]
    private int attackPower = 10;

    [SerializeField]
    [Tooltip("Cooldown time between attacks.")]
    private float attackCooldown = 1.0f;

    private float lastAttackTime;

    #endregion

    #region Events

    /// <summary>
    /// Event triggered when the player's health changes.
    /// </summary>
    public static event Action<int> OnHealthChanged;

    /// <summary>
    /// Event triggered when the player's score changes.
    /// </summary>
    public static event Action<int> OnScoreChanged;

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Initializes the player stats.
    /// </summary>
    private void Start()
    {
        lastAttackTime = -attackCooldown; // Allow immediate attack at start
        OnHealthChanged?.Invoke(health);
        OnScoreChanged?.Invoke(score);
    }

    #endregion

    #region Attack Management

    /// <summary>
    /// Checks if the player unit can attack based on the attack cooldown.
    /// </summary>
    /// <returns>True if the player can attack, false otherwise.</returns>
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
            Debug.Log("Player attacking with power: " + attackPower);
        }
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
        OnHealthChanged?.Invoke(health);

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

    #region Score Management

    /// <summary>
    /// Increases the score of the player unit by the specified amount.
    /// </summary>
    /// <param name="amount">The amount to increase the score by.</param>
    public void IncreaseScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    #endregion
}
