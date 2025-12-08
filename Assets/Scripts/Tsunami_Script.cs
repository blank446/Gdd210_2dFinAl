using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Tsunami_Script : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [Tooltip("How far outside the camera to spawn/destroy")]
    [SerializeField] private float horizontalPadding = 1f;

    [Header("Hit Visual")]
    [SerializeField] private float flashDuration = 0.1f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float leftEdge;
    private float rightEdge;
    private float bottomEdge;
    private float topEdge;
    private bool movingRight;
    private const float Z_DEPTH = -1f;
    private SpriteRenderer spriteRenderer; // Sprite Renderer to flip the sprite

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get Sprite renderer
    }

    private void Start()
    {
        // --- Camera bounds ---
        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        leftEdge = camPos.x - halfWidth;
        rightEdge = camPos.x + halfWidth;
        bottomEdge = camPos.y - halfHeight;
        topEdge = camPos.y + halfHeight;

        // Random Y within the visible vertical area
        float spawnY = Random.Range(bottomEdge, topEdge);

        // Random side
        movingRight = Random.value < 0.5f;

        float spawnX;
        if (movingRight)
        {
            // Spawn just off the left, move right
            spawnX = leftEdge - horizontalPadding;
            moveDirection = Vector2.right;
        }
        else
        {
            // Spawn just off the right, move left
            spawnX = rightEdge + horizontalPadding;
            moveDirection = Vector2.left;
            spriteRenderer.flipX = true; // Flips the sprite horizontally
        }

        // Position the rigidbody (physics) and transform (render)
        Vector2 start2D = new Vector2(spawnX, spawnY);
        rb.position = start2D;
        transform.position = new Vector3(start2D.x, start2D.y, Z_DEPTH);

        // Rigidbody safety setup
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void FixedUpdate()
    {
        // Move via Rigidbody so triggers are precise
        Vector2 newPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // Keep Z locked for rendering
        transform.position = new Vector3(newPos.x, newPos.y, Z_DEPTH);

        // Despawn once it passes the far edge
        if (movingRight && newPos.x > rightEdge + horizontalPadding)
        {
            Destroy(gameObject);
        }
        else if (!movingRight && newPos.x < leftEdge - horizontalPadding)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(1);
            }
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
