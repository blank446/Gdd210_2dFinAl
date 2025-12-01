using UnityEngine;

public class Tornado_Script : MonoBehaviour
{
    [Header("Movement Setting")]
    [Tooltip("How fast the tornado moves")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Lifetime")]
    [Tooltip("How many seconds the tornado exists before disappearing")]
    [SerializeField] private float lifeTime = 5f;
    [Header("Bounds")]
    [Tooltip("How far left it can go")]
    [SerializeField] private float minX = -8.29f;
    [Tooltip("How far right it can go")]
    [SerializeField] private float maxX = 8.33f;
    [Tooltip("How far down it can go")]
    [SerializeField] private float minY = -4.45f;
    [Tooltip("How far up it can go")]
    [SerializeField] private float maxY = 4.41f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;      // no gravity in 2D top-down
        rb.freezeRotation = true;  // keep upright
        Destroy(gameObject, lifeTime);
        // start moving in a random direction
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void FixedUpdate()
    {
        MoveAndBounce();
    }

    private void MoveAndBounce()
    {
        Vector2 pos = rb.position;
        pos += moveDirection * moveSpeed * Time.fixedDeltaTime;

        // bounce off left/right
        if (pos.x < minX)
        {
            pos.x = minX;
            moveDirection.x *= -1f;
        }
        else if (pos.x > maxX)
        {
            pos.x = maxX;
            moveDirection.x *= -1f;
        }

        // bounce off top/bottom
        if (pos.y < minY)
        {
            pos.y = minY;
            moveDirection.y *= -1f;
        }
        else if (pos.y > maxY)
        {
            pos.y = maxY;
            moveDirection.y *= -1f;
        }

        rb.MovePosition(pos);
    }

    // Collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Tornado hit the player!");

            SpriteRenderer sr = other.gameObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                // Make player flash red
                sr.color = Color.red;

                // Return to normal after a tiny delay
                Invoke(nameof(ResetPlayerColor), 0.1f);
            }
        }
    }
    private void ResetPlayerColor()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }
    }
}
