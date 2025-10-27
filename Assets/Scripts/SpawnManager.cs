using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public Transform spawnPoint;
}

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Configuration")]
    public List<SpawnData> spawnList;

    [Tooltip("If checked, objects will spawn automatically on Start.")]
    public bool spawnOnStart = true;

    [Tooltip("If checked, objects will be spawned in waves.")]
    public bool spawnInWaves = false;

    public float waveDelay = 5.0f;

    [Tooltip("If checked, spawn at random positions instead of at spawn points.")]
    public bool randomSpawn = false;

    public Vector3 spawnMin = new Vector3(-5f, 0.5f, -5f);
    public Vector3 spawnMax = new Vector3(5f, 0.5f, 5f);

    [Header("Optional: Keep Hierarchy Clean")]
    public Transform container;

    private void Start()
    {
        if (spawnOnStart)
        {
            if (spawnInWaves)
                StartCoroutine(SpawnWavesRoutine());
            else
                SpawnAllObjects();
        }
    }

    public void SpawnAllObjects()
    {
        foreach (var data in spawnList)
        {
            Vector3 position = GetSpawnPosition(data.spawnPoint);
            Spawn(data.prefab, position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnWavesRoutine()
    {
        foreach (var data in spawnList)
        {
            Vector3 position = GetSpawnPosition(data.spawnPoint);
            Spawn(data.prefab, position, Quaternion.identity);
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
        return spawnPoint != null ? spawnPoint.position : Vector3.zero;
    }

    public void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("SpawnManager: Missing prefab reference.");
            return;
        }

        GameObject newObject = Instantiate(prefab, position, rotation);

        if (container != null)
            newObject.transform.SetParent(container);
    }

    public void SpawnSpecificObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Spawn(prefab, position, rotation);
    }

    public void SpawnRandomObject(List<GameObject> prefabs, Transform spawnLocation)
    {
        if (prefabs.Count == 0 || spawnLocation == null) return;

        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject selectedPrefab = prefabs[randomIndex];

        Vector3 position = GetSpawnPosition(spawnLocation);
        Spawn(selectedPrefab, position, spawnLocation.rotation);
    }
}
