using UnityEngine;

public class MeteorCollision : MonoBehaviour
{
    [Header("Lifetime")]
    [Tooltip("Destroy meteor after this many seconds")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Hit Effect")]
    [Tooltip("How long the player flashes on hit")]
    [SerializeField] private float flashTime = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Meteor hit player!");

            SpriteRenderer sr = collision.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // simple flash
                sr.color = Color.red;
                Invoke(nameof(ResetPlayerColor), flashTime);
            }

            Destroy(gameObject); // meteor disappears after hit
        }
    }

    private void ResetPlayerColor()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.white; // set back to default color
    }
}
