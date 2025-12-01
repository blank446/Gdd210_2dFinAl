using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float flashDuration = 0.1f;

    public int CurrentHealth { get; private set; }

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isFlashing;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;

        if (sr != null)
            StartCoroutine(Flash());

        if (CurrentHealth <= 0)
            Die();
    }

    private IEnumerator Flash()
    {
        if (isFlashing) yield break;
        isFlashing = true;

        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;

        isFlashing = false;
    }

    private void Die()
    {
        Debug.Log("Player died");
        // Optional: disable movement, show game over, etc.
        // GetComponent<Char_Ctrl>().enabled = false;
    }
}
