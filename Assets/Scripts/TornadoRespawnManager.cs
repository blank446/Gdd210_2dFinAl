using UnityEngine;

public class TornadoRespawnManager : MonoBehaviour
{
    [Header("Tornado Settings")]
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private float minScoreToStart = 20f;
    [SerializeField] private float spawnInterval = 8f;
    [SerializeField] private int maxActiveTornados = 3;

    [Header("Spawn Bounds")]
    [SerializeField] private float minX = -8.29f;
    [SerializeField] private float maxX = 8.33f;
    [SerializeField] private float minY = -4.45f;
    [SerializeField] private float maxY = 4.41f;

    private float spawnTimer;

    private void Update()
    {
        if (ScoreManager.Instance == null)
            return;

        // Wait until score is high enough
        if (ScoreManager.Instance.Score < minScoreToStart)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            // Only spawn if we are under the active limit
            int activeTornados = GameObject.FindGameObjectsWithTag("Tornado").Length;
            if (activeTornados < maxActiveTornados)
            {
                SpawnTornado();
            }
        }
    }

    private void SpawnTornado()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);
    }
}
