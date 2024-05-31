using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages player and enemy units, including their creation, movement, and actions.
/// </summary>
public class UnitManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static UnitManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of UnitManager.
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

    // List of player units
    public List<GameObject> playerUnits = new List<GameObject>();
    // List of enemy units
    public List<GameObject> enemyUnits = new List<GameObject>();

    #endregion

    #region Unit Management

    /// <summary>
    /// Adds a player unit to the manager.
    /// </summary>
    /// <param name="unit">The player unit to add.</param>
    public void AddPlayerUnit(GameObject unit)
    {
        playerUnits.Add(unit);
    }

    /// <summary>
    /// Removes a player unit from the manager.
    /// </summary>
    /// <param name="unit">The player unit to remove.</param>
    public void RemovePlayerUnit(GameObject unit)
    {
        playerUnits.Remove(unit);
    }

    /// <summary>
    /// Adds an enemy unit to the manager.
    /// </summary>
    /// <param name="unit">The enemy unit to add.</param>
    public void AddEnemyUnit(GameObject unit)
    {
        enemyUnits.Add(unit);
    }

    /// <summary>
    /// Removes an enemy unit from the manager.
    /// </summary>
    /// <param name="unit">The enemy unit to remove.</param>
    public void RemoveEnemyUnit(GameObject unit)
    {
        enemyUnits.Remove(unit);
    }

    /// <summary>
    /// Commands all player units to execute a specific action.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void CommandUnits(string command)
    {
        foreach (GameObject unit in playerUnits)
        {
            unit.GetComponent<PlayerController>().ExecuteCommand(command);
        }
    }

    #endregion
}
