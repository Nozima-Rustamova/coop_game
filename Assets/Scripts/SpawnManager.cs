using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A serializable struct to group a prefab tag and its specific spawn point.
/// The [System.Serializable] attribute makes this struct visible in the Unity Inspector.
/// </summary>
[System.Serializable]
public struct SpawnData
{
    public string poolTag; // Reference to the pool tag instead of direct prefab
    public Transform spawnPoint;
}

public class SpawnManager : MonoBehaviour
{
    [Header("Pool Reference")]
    [Tooltip("Reference to the ObjectPooler script.")]
    public ObjectPooler objectPooler;

    [Header("Spawn Configuration")]
    [Tooltip("List of pool tags and their corresponding spawn points.")]
    public List<SpawnData> spawnList;

    [Tooltip("If checked, objects will spawn automatically on Start.")]
    public bool spawnOnStart = true;

    [Tooltip("If checked, objects will be spawned in waves.")]
    public bool spawnInWaves = false;

    [Tooltip("Time in seconds to wait between waves.")]
    public float waveDelay = 5.0f;

    [Tooltip("If checked, spawn at random positions within defined bounds instead of fixed points.")]
    public bool randomSpawn = false;

    [Tooltip("Minimum bounds for random spawn (x, y, z).")]
    public Vector3 spawnMin = new Vector3(-5f, 0.5f, -5f);

    [Tooltip("Maximum bounds for random spawn (x, y, z).")]
    public Vector3 spawnMax = new Vector3(5f, 0.5f, 5f);

    [Header("Containers (Optional)")]
    [Tooltip("An empty GameObject to hold all spawned objects for a clean Hierarchy.")]
    public Transform container;

    private void Start()
    {
        if (spawnOnStart)
        {
            if (spawnInWaves)
            {
                StartCoroutine(SpawnWavesRoutine());
            }
            else
            {
                SpawnAllObjects();
            }
        }
    }

    /// <summary>
    /// Spawns every object in the spawnList at its specified spawn point immediately.
    /// </summary>
    public void SpawnAllObjects()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            Vector3 position = GetSpawnPosition(spawnList[i].spawnPoint);
            Spawn(spawnList[i].poolTag, position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns objects in waves with a delay between each wave.
    /// </summary>
    private IEnumerator SpawnWavesRoutine()
    {
        int totalObjects = spawnList.Count;
        int objectsPerWave = 1; // You can make this configurable

        for (int i = 0; i < totalObjects; i += objectsPerWave)
        {
            for (int j = 0; j < objectsPerWave && (i + j) < totalObjects; j++)
            {
                Vector3 position = GetSpawnPosition(spawnList[i + j].spawnPoint);
                Spawn(spawnList[i + j].poolTag, position, Quaternion.identity);
            }
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private Vector3 GetSpawnPosition(Transform spawnPoint)
    {
        if (randomSpawn)
        {
            return new Vector3(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y),
                Random.Range(spawnMin.z, spawnMax.z)
            );
        }
        else if (spawnPoint != null)
        {
            return spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("SpawnManager: No spawn point defined and random spawn disabled. Using default position.");
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Spawns a single object from the pool at a specified position and rotation.
    /// </summary>
    /// <param name="poolTag">The tag of the pool to spawn from.</param>
    /// <param name="position">The position where the object will spawn.</param>
    /// <param name="rotation">The rotation for the spawned object.</param>
    public void Spawn(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (objectPooler == null)
        {
            Debug.LogWarning("SpawnManager: ObjectPooler reference is missing. Cannot spawn.");
            return;
        }

        GameObject newObject = objectPooler.SpawnFromPool(poolTag, position, rotation);

        if (newObject != null && container != null)
        {
            newObject.transform.SetParent(container);
        }
    }

    /// <summary>
    /// Public method to trigger the spawning of a specific object from another script.
    /// Useful for event-based spawning.
    /// </summary>
    /// <param name="poolTag">The pool tag to spawn from.</param>
    /// <param name="position">The position to spawn at.</param>
    /// <param name="rotation">The rotation to apply.</param>
    public void SpawnSpecificObject(string poolTag, Vector3 position, Quaternion rotation)
    {
        Spawn(poolTag, position, rotation);
    }

    /// <summary>
    /// Spawns a random object from a list of pool tags.
    /// </summary>
    /// <param name="poolTags">List of pool tags to choose from.</param>
    /// <param name="spawnLocation">The location to spawn at.</param>
    public void SpawnRandomObject(List<string> poolTags, Transform spawnLocation)
    {
        if (poolTags.Count == 0 || spawnLocation == null)
        {
            Debug.LogWarning("SpawnManager: Pool tags list or spawn location is empty. Cannot spawn.");
            return;
        }

        int randomIndex = Random.Range(0, poolTags.Count);
        string randomTag = poolTags[randomIndex];
        Vector3 position = GetSpawnPosition(spawnLocation);
        Spawn(randomTag, position, spawnLocation.rotation);
    }
}