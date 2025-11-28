using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("Player Reference")]
    [Tooltip("Reference to the player object so we can access its PlayerHealth.")]
    [SerializeField] private GameObject player;
    [Tooltip("Layer used for detecting the player only.")]
    //[SerializeField] private LayerMask playerLayer;
    private PlayerHealth playerHealth;
    private Rigidbody2D playerRb;



    [Header("Meteor Attributes")]
    [Tooltip("Radius of the circular hitbox when the meteor impacts.")]
    [SerializeField] private float hitboxSize = 1f;

    [Tooltip("How long after spawning before the meteor hitbox appears.")]
    [SerializeField] private float landingTime = 1.5f;

    [Tooltip("How long the hitbox stays active after appearing.")]
    [SerializeField] private float hitboxDuration = 0.3f;

    private Animator anim;

    private bool hitboxActive = false;
    private float timer = 0f;


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

        if (playerRb == null)
            return;

        float distance = Vector2.Distance(transform.position, playerRb.position);

        if (distance <= hitboxSize) // Deal damage to the player if within hitbox
        {
            if (playerHealth != null)
                playerHealth.TakeDamage();
        }
    }

    private void DeactivateHitbox()
    {
        hitboxActive = false;
    }

    public void DestroyMeteor() // Destroy meteor after animation ends
    {
        Destroy(gameObject);
    }



    // This draws the hitbox in the Scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitboxSize);
    }
}
