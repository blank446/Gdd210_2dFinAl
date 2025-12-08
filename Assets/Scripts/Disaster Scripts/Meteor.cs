using UnityEngine;

public class Meteor : MonoBehaviour, IDisasterNeedsPlayer
{
    // Player references to access playerHealth and rigidbody
    private GameObject player;
    private PlayerHealth playerHealth;
    private Rigidbody2D playerRb;

    [Header("Meteor Attributes")]
    [Tooltip("How long after spawning before the meteor hitbox appears.")]
    [SerializeField] private float landingTime = 1.5f;

    [Tooltip("How long the hitbox stays active after appearing.")]
    [SerializeField] private float hitboxDuration = 0.3f;

    private Animator anim;

    private bool hitboxActive = false;
    private float timer = 0f;

    private CircleCollider2D hitbox; // use collider as hitbox


    void Awake()
    {
        // Get circle collider
        hitbox = GetComponent<CircleCollider2D>();

        if (hitbox == null)
            Debug.LogError("Meteor needs a CircleCollider2D!");

        hitbox.enabled = false; // start off
        hitbox.isTrigger = true;
    }

    void Start()
    {
        // Cache player components for performance and easier access later
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerRb = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("Meteor has no player reference!");
        }

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 1. Wait until landing time elapses → turn hitbox ON
        if (!hitboxActive && timer >= landingTime)
        {
            ActivateHitbox();
            anim.SetTrigger("Explode");
            Debug.Log("Meteor explodes");
        }

        // 2. When hitbox is active → count down until it expires
        if (hitboxActive && timer >= landingTime + hitboxDuration)
        {
            DeactivateHitbox();
        }
    }

    private void ActivateHitbox()
    {
        hitboxActive = true;
        hitbox.enabled = true;
    }

    private void DeactivateHitbox()
    {
        hitboxActive = false;
        hitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collider2D entered in space");
        if (!hitboxActive) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Meteor hits player via collider!");

            if (playerHealth != null)
                playerHealth.TakeDamage();
            // Player flash
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

    public void DestroyMeteor() // Destroy meteor after animation ends
    {
        Destroy(gameObject);
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
        playerHealth = p.GetComponent<PlayerHealth>();
        playerRb = p.GetComponent<Rigidbody2D>();
    }
}
