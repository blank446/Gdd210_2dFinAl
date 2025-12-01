using UnityEngine;

public class DisasterDifficulty : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MeteorSpawner meteorSpawner;
    [SerializeField] private Tornado_Script tornado;

    [Header("Meteor Spawn Interval (seconds)")]
    [SerializeField] private float maxInterval = 5f;  // easy
    [SerializeField] private float minInterval = 1f;  // hard

    [Header("Tornado Speed")]
    [SerializeField] private float baseTornadoSpeed = 5f;
    [SerializeField] private float maxTornadoSpeed = 12f;

    [Header("Difficulty Ramp")]
    [Tooltip("Score value at which difficulty is maxed out")]
    [SerializeField] private float maxScoreForDifficulty = 100f;

    private void Update()
    {
        if (ScoreManager.Instance == null) return;

        float score = ScoreManager.Instance.Score;
        float t = Mathf.Clamp01(score / maxScoreForDifficulty); // 0..1

        if (meteorSpawner != null)
        {
            float interval = Mathf.Lerp(maxInterval, minInterval, t);
            meteorSpawner.SetSpawnInterval(interval);
        }

        if (tornado != null)
        {
            float speed = Mathf.Lerp(baseTornadoSpeed, maxTornadoSpeed, t);
            tornado.SetMoveSpeed(speed);
        }
    }
}
