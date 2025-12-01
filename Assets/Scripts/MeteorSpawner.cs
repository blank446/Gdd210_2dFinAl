using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Meteor prefab to spawn")]
    [SerializeField] private GameObject meteorPrefab;

    [Tooltip("Warning prefab that appears before the meteor")]
    [SerializeField] private GameObject warningPrefab;

    [Header("Spawn Settings")]
    [Tooltip("Seconds between meteor spawns")]
    [SerializeField] private float spawnInterval = 5f;

    [Tooltip("How long the warning stays before the meteor appears")]
    [SerializeField] private float warningTime = 1f;

    [Header("Bounds")]
    [Tooltip("How far left the meteor can spawn")]
    [SerializeField] private float minX = -8.29f;
    [Tooltip("How far right the meteor can spawn")]
    [SerializeField] private float maxX = 8.33f;
    [Tooltip("How far down the meteor can spawn")]
    [SerializeField] private float minY = -4.45f;
    [Tooltip("How far up the meteor can spawn")]
    [SerializeField] private float maxY = 4.41f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // pick random position inside bounds
            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            // spawn warning
            GameObject warning = null;
            if (warningPrefab != null)
            {
                warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);
            }

            // wait, then spawn meteor
            yield return new WaitForSeconds(warningTime);

            Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

            if (warning != null)
            {
                Destroy(warning);
            }
        }
    }
    public void SetSpawnInterval(float newInterval)
    {
        spawnInterval = newInterval;
    }
}
