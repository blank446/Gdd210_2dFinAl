using UnityEngine;

[CreateAssetMenu(menuName = "Disasters/Disaster Type")]
public class DisasterType : ScriptableObject
{
    [Tooltip("Name of disaster.")]
    public string disasterName;
    [Tooltip("Disaster prefab.")]
    public GameObject prefab;

    [Header("Director Cost")]
    [Tooltip("The amount of credits the disaster costs for the director to spawn it.")]
    public float cost = 1f;

    [Header("Spawn Timing")]
    [Tooltip("Minimum delay before the disaster can be spawned again.")]
    public float minSpawnDelay = 1f;
    [Tooltip("Maximum delay before thedisaster can be spawned again.")]
    public float maxSpawnDelay = 3f;

    [Header("Spawn Distance from Player")]
    [Tooltip("Minimum distance the disaster can be spawned from the player.")]
    public float minSpawnDistance = 8f;
    [Tooltip("Maximum distance the disaster can be spawned from the player.")]
    public float maxSpawnDistance = 14f;

    /*Possibly implement minimum wave needed to spawn*/
}
