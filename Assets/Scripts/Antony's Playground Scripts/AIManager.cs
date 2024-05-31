using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy AI behaviors, controlling their actions and interactions.
/// </summary>
public class AIManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static AIManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of AIManager.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Fields

    // List of enemy controllers
    private List<EnemyController> enemyControllers = new List<EnemyController>();

    #endregion

    #region Initialization

    /// <summary>
    /// Called when the game starts.
    /// Initializes the AI manager.
    /// </summary>
    private void Start()
    {
        InitializeAI();
    }

    /// <summary>
    /// Initializes the AI by finding all enemy controllers in the scene.
    /// </summary>
    private void InitializeAI()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        enemyControllers.AddRange(enemies);
    }

    #endregion

    #region AI Management

    /// <summary>
    /// Controls the actions of all enemies.
    /// </summary>
    public void ControlEnemies()
    {
        foreach (EnemyController enemy in enemyControllers)
        {
            enemy.PerformAction();
        }
    }

    /// <summary>
    /// Adds an enemy controller to the manager.
    /// </summary>
    /// <param name="enemyController">The enemy controller to add.</param>
    public void AddEnemyController(EnemyController enemyController)
    {
        if (!enemyControllers.Contains(enemyController))
        {
            enemyControllers.Add(enemyController);
        }
    }

    /// <summary>
    /// Removes an enemy controller from the manager.
    /// </summary>
    /// <param name="enemyController">The enemy controller to remove.</param>
    public void RemoveEnemyController(EnemyController enemyController)
    {
        if (enemyControllers.Contains(enemyController))
        {
            enemyControllers.Remove(enemyController);
        }
    }

    #endregion
}
