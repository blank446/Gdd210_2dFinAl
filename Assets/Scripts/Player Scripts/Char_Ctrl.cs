using UnityEngine;

public class Char_Ctrl : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Change for speed")]
    public float moveSpeed = 6f;

    [Tooltip("How fast sprint increases")]
    public float sprintMultiplier = 1.5f;

    [Header("Dash")]
    [Tooltip("dash burst")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.12f; // actual dash burst time (not used here but kept)
    public float dashEndDelay = 2f;    // dash lasts 2 seconds

    [Header("Player Bounds")]
    public bool usePlayerBounds = true;
    public float playerMinX = -8.29f;
    public float playerMaxX = 8.33f;
    public float playerMinY = -4.45f;
    public float playerMaxY = 4.41f;

    [Header("Camera Bounds")]
    public bool useCameraBounds = true;
    [Tooltip("Assign your MainCamera child here")]
    public Transform mainCamera;
    public float cameraMinX = -8.29f;
    public float cameraMaxX = 8.33f;
    public float cameraMinY = -4.45f;
    public float cameraMaxY = 4.41f;

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

        // --- player movement (original style using GetKeyDown) ---
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = Vector2.up * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.linearVelocity = Vector2.down * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.linearVelocity = Vector2.left * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.linearVelocity = Vector2.right * moveSpeed;
        }

        // stop movement when no keys are pressed
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = Vector2.zero;
        }

        // sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.linearVelocity *= sprintMultiplier;
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
        //anim.SetFloat("MoveX", moveInput.x);
       // anim.SetFloat("MoveY", moveInput.y);
       // anim.SetBool("IsMoving", moveInput.sqrMagnitude > 0.001f);
    }
}
