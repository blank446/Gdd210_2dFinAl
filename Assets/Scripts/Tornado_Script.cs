using UnityEngine;

public class Tornado_Script : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Bounds")]
    [SerializeField] private float minX = -8.29f;
    [SerializeField] private float maxX = 8.33f;
    [SerializeField] private float minY = -4.45f;
    [SerializeField] private float maxY = 4.41f;

    private Vector2 moveDirection;

    void Start()
    {
        // start moving in a random direction
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void Update()
    {
        MoveAndBounce();
    }

    private void MoveAndBounce()
    {
        Vector2 pos = transform.position;
        pos += moveDirection * moveSpeed * Time.deltaTime;

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

        transform.position = pos;
    }
}
