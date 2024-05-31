using UnityEngine;

/// <summary>
/// Manages loading and instantiating resources such as prefabs, textures, and audio clips.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    #region Singleton

    // Singleton instance
    public static ResourceManager Instance { get; private set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures there is only one instance of ResourceManager.
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

    #region Resource Loading

    /// <summary>
    /// Loads a resource of type T from the specified path.
    /// </summary>
    /// <typeparam name="T">The type of resource to load.</typeparam>
    /// <param name="path">The path to the resource.</param>
    /// <returns>The loaded resource of type T.</returns>
    public T LoadResource<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);
        if (resource == null)
        {
            Debug.LogError($"Resource at path {path} could not be loaded.");
        }
        return resource;
    }

    /// <summary>
    /// Instantiates a prefab at the specified position and rotation.
    /// </summary>
    /// <param name="path">The path to the prefab.</param>
    /// <param name="position">The position to instantiate the prefab at.</param>
    /// <param name="rotation">The rotation to instantiate the prefab with.</param>
    /// <returns>The instantiated GameObject.</returns>
    public GameObject InstantiatePrefab(string path, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = LoadResource<GameObject>(path);
        if (prefab != null)
        {
            return Instantiate(prefab, position, rotation);
        }
        Debug.LogError($"Failed to instantiate prefab from path {path}");
        return null;
    }

    /// <summary>
    /// Instantiates a prefab at the specified position with default rotation.
    /// </summary>
    /// <param name="path">The path to the prefab.</param>
    /// <param name="position">The position to instantiate the prefab at.</param>
    /// <returns>The instantiated GameObject.</returns>
    public GameObject InstantiatePrefab(string path, Vector3 position)
    {
        return InstantiatePrefab(path, position, Quaternion.identity);
    }

    /// <summary>
    /// Instantiates a prefab with default position and rotation.
    /// </summary>
    /// <param name="path">The path to the prefab.</param>
    /// <returns>The instantiated GameObject.</returns>
    public GameObject InstantiatePrefab(string path)
    {
        return InstantiatePrefab(path, Vector3.zero, Quaternion.identity);
    }

    #endregion

    #region Resource Unloading

    /// <summary>
    /// Unloads an asset to free up memory.
    /// </summary>
    /// <typeparam name="T">The type of asset to unload.</typeparam>
    /// <param name="asset">The asset to unload.</param>
    public void UnloadAsset<T>(T asset) where T : Object
    {
        Resources.UnloadAsset(asset);
    }

    /// <summary>
    /// Unloads unused assets to free up memory.
    /// </summary>
    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }

    #endregion
}
