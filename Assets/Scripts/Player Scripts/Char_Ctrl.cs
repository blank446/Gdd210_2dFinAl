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
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //player movement
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
        //sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.linearVelocity *= sprintMultiplier;
        }
        //dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 dashDirection = rb.linearVelocity.normalized;
            rb.linearVelocity = dashDirection * dashSpeed;
        }
    }
}