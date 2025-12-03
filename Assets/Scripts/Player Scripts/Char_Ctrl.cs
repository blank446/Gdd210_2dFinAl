using UnityEngine;

public class Char_Ctrl : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Change for speed")]
    [SerializeField] private float moveSpeed = 6f;

    [Tooltip("How fast sprint increases")]
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Dash")]
    [Tooltip("how fast the dash boosts the player speed")]
    [SerializeField] private float dashSpeed = 18f;
    [Tooltip("The dash cooldown")]
    [SerializeField] private float dashEndDelay = 2f;

    [Header("Player Bounds")]
    [SerializeField] private bool usePlayerBounds = true;
    [Tooltip("How far left the player can go")]
    [SerializeField] private float playerMinX = -8.29f;
    [Tooltip("How far right the player can go")]
    [SerializeField] private float playerMaxX = 8.33f;
    [Tooltip("How far down the player can go")]
    [SerializeField] private float playerMinY = -4.45f;
    [Tooltip("How far up the player can go")]
    [SerializeField] private float playerMaxY = 4.41f;

    [Header("Camera Bounds")]
    [SerializeField] private bool useCameraBounds = true;
    [Tooltip("Assign your MainCamera child here")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float cameraMinX = -8.29f;
    [SerializeField] private float cameraMaxX = 8.33f;
    [SerializeField] private float cameraMinY = -4.45f;
    [SerializeField] private float cameraMaxY = 4.41f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;
    private bool isDashing;
    private float dashTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- for player animations (MoveX/MoveY) ---
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (!isDashing)
        {
            Vector2 move = Vector2.zero;

            if (Input.GetKey(KeyCode.W)) move += Vector2.up;
            if (Input.GetKey(KeyCode.S)) move += Vector2.down;
            if (Input.GetKey(KeyCode.A)) move += Vector2.left;
            if (Input.GetKey(KeyCode.D)) move += Vector2.right;

            // prevent faster diagonal movement
            if (move.sqrMagnitude > 1f)
                move.Normalize();

            rb.linearVelocity = move * moveSpeed;

            // base speed, boosted while Left Shift is held (and not dashing)
            float speed = moveSpeed;
            if (!isDashing && Input.GetKey(KeyCode.LeftShift))
                speed *= sprintMultiplier;

            // apply velocity (or stop if no input)
            rb.linearVelocity = (move.sqrMagnitude > 0f) ? move * speed : Vector2.zero;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            performDash();
        }

        // end the dash after 2 seconds
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.linearVelocity = Vector2.zero;
            }
        }

        // clamp player + camera inside bounds
        if (usePlayerBounds) EnforcePlayerBounds();
        if (useCameraBounds) EnforceCameraBounds();

        // update the animations
        UpdateAnimator();
    }

    private void performDash()
    {
        // Get the current movement direction from velocity
        Vector2 dashDirection = rb.linearVelocity.normalized;

        // If standing still, use input direction
        if (dashDirection == Vector2.zero && moveInput.sqrMagnitude > 0.001f)
        {
            dashDirection = moveInput.normalized;
        }

        // Apply the dash velocity
        rb.linearVelocity = dashDirection * dashSpeed;

        // Start dash timer (2 seconds)
        isDashing = true;
        dashTimer = dashEndDelay;
    }

    // --- Bounds helpers ---
    private void EnforcePlayerBounds()
    {
        Vector2 p = rb.position;

        float clampedX = Mathf.Clamp(p.x, playerMinX, playerMaxX);
        float clampedY = Mathf.Clamp(p.y, playerMinY, playerMaxY);

        // If we hit vertical wall, stop horizontal motion
        if (!Mathf.Approximately(clampedX, p.x))
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // If we hit horizontal wall, stop vertical motion
        if (!Mathf.Approximately(clampedY, p.y))
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Snap inside bounds
        rb.position = new Vector2(clampedX, clampedY);
    }

    private void EnforceCameraBounds()
    {
        if (mainCamera == null) return;

        Vector3 camPos = mainCamera.position;

        float clampedX = Mathf.Clamp(camPos.x, cameraMinX, cameraMaxX);
        float clampedY = Mathf.Clamp(camPos.y, cameraMinY, cameraMaxY);

        mainCamera.position = new Vector3(clampedX, clampedY, camPos.z);
    }

    /// <summary>
    /// give an input to the blend tree for animations
    /// </summary>
    private void UpdateAnimator()
    {
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
        anim.SetBool("IsMoving", moveInput.sqrMagnitude > 0.001f);
    }
}
