using UnityEngine;
using System.Collections.Generic;

public class DisasterDirector : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Transform player; //position of player

    [Header("Disaster Data")]
    [Tooltip("Disasters spawned")]
    [SerializeField] private DisasterType[] disasterTypes; //spawnable disasters

    [Header("Director Credits")]
    [Tooltip("Legacy value. Kept for inspector compatibility but overridden by Max Credits Curve when present.")]
    [SerializeField] private float maxCredits = 10f;
    [Tooltip("Legacy value. Kept for inspector compatibility but overridden by Regen Curve when present.")]
    [SerializeField] private float creditRegenRate = 2f;

    // ---------------- AnimationCurve scaling (YOU WILL EDIT THESE IN THE INSPECTOR) ----------------
    [Header("Per-wave scaling (AnimationCurve)")]
    [Tooltip("Evaluate this curve at the current wave number (x = wave #) to obtain the current max credits.\n" +
             "Example: Add keys at (1,10), (5,20) to make wave1 cap = 10, wave5 cap = 20.")]
    [SerializeField] private AnimationCurve maxCreditsCurve = AnimationCurve.Linear(1f, 10f, 10f, 30f);

    [Tooltip("Evaluate this curve at the current wave number (x = wave #) to obtain the credit regen rate (credits/sec).\n" +
             "Example: Add keys at (1,2), (5,4) to make regen increase over waves.")]
    [SerializeField] private AnimationCurve regenCurve = AnimationCurve.Linear(1f, 2f, 10f, 5f);

    // -----------------------------------------------------------------------------------------------

    private float currentCredits; // current amount of credits
    private float nextSpawnTime = 0f;
    private bool wasRestingLastFrame = false; // prevents ClearAllDisasters() from being called everyframe during rest

    // Track active disasters so we can clear them on rest
    private List<GameObject> activeDisasters = new List<GameObject>();

    private void Start()
    {
        // Initialize credits using the curve at wave 1 (or fall back to legacy maxCredits)
        currentCredits = (maxCreditsCurve != null) ? GetScaledMaxCredits(1) : maxCredits;
        nextSpawnTime = Time.time + 1f;
    }

    private void Update()
    {
        float waveNum = waveManager.GetWaveStatus();

        // REST PERIOD
        if (waveNum == 0)
        {
            if (!wasRestingLastFrame)
            {
                // Entering rest for the first time
                ClearAllDisasters();

                // Reset credits for next wave (use scaled max for wave 1)
                currentCredits = GetScaledMaxCredits(1);

                // Reset spawn timer so wave starts cleanly
                nextSpawnTime = Time.time;
            }

            wasRestingLastFrame = true;
            return;
        }

        // If we reach here, it's an ACTIVE wave
        wasRestingLastFrame = false;

        // Evaluate curves using the current wave number
        int waveInt = Mathf.Max(1, Mathf.FloorToInt(waveNum));
        float scaledMax = GetScaledMaxCredits(waveInt);
        float scaledRegen = GetScaledRegen(waveInt);

        // ACTIVE WAVE: Regenerate credits using scaled regen and scaled max
        currentCredits += scaledRegen * Time.deltaTime;
        currentCredits = Mathf.Min(currentCredits, scaledMax);

        // Wait until next spawn window
        if (Time.time < nextSpawnTime)
            return;

        TrySpawnDisaster(waveNum);
    }

    private void ClearAllDisasters() // Destroys all disasters for rest. Might be swapped out for just preventing disaster spawning
    {
        foreach (GameObject obj in activeDisasters)
        {
            if (obj != null)
                Destroy(obj);
        }

        activeDisasters.Clear();
    }

    private void TrySpawnDisaster(float waveNum) // Checks to see if a disaster can be spawned and spends credits to spawn it
    {
        // We rely on scaled max and regen; don't add ad-hoc wave bonuses here
        float effectiveCredits = currentCredits;

        // Choose disasters we can afford
        DisasterType[] affordable = System.Array.FindAll(disasterTypes,
            d => d.cost <= effectiveCredits);

        if (affordable.Length == 0)
            return;

        // Random disaster from the affordable pool
        DisasterType chosen = affordable[Random.Range(0, affordable.Length)];

        // Spend credits to spawn it
        currentCredits -= chosen.cost;

        // Spawn the chosen disaster
        SpawnDisaster(chosen);

        // Set the next spawn time
        nextSpawnTime = Time.time + Random.Range(chosen.minSpawnDelay, chosen.maxSpawnDelay);
    }

    private void SpawnDisaster(DisasterType type) // Spawns the disaster based on the player's position and spawn distance of the disaster
    {
        // sanity check
        if (type == null || type.prefab == null)
        {
            Debug.LogWarning("DisasterType or its prefab is null. Skipping spawn.");
            return;
        }

        // random direction around player
        Vector2 dir = Random.insideUnitCircle.normalized;

        // each disaster controls how far away it spawns
        float dist = Random.Range(type.minSpawnDistance, type.maxSpawnDistance);

        // final spawn position
        Vector2 spawnPos = (Vector2)player.position + dir * dist;

        // spawn instance
        GameObject spawned = Instantiate(type.prefab, spawnPos, Quaternion.identity);
        Debug.Log(type.disasterName + " spawned");

        // Assigns player using interface
        IDisasterNeedsPlayer needsPlayer = spawned.GetComponent<IDisasterNeedsPlayer>();
        if (needsPlayer != null)
            needsPlayer.SetPlayer(player.gameObject);

        // track it for rest cleanup
        activeDisasters.Add(spawned);
    }

    // ------------------ Curve helpers ------------------
    private float GetScaledMaxCredits(int waveNum)
    {
        // if curve missing, fall back to legacy maxCredits
        if (maxCreditsCurve == null)
            return maxCredits;

        // Evaluate at the wave number. Ensure waveNum >= 1.
        float x = Mathf.Max(1, waveNum);
        return maxCreditsCurve.Evaluate(x);
    }

    private float GetScaledRegen(int waveNum)
    {
        if (regenCurve == null)
            return creditRegenRate;

        float x = Mathf.Max(1, waveNum);
        return regenCurve.Evaluate(x);
    }
}
