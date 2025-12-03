using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Tooltip("Reference to the player's health script")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Sprites representing each possible health state")]
    [SerializeField] private Sprite[] healthSprites;

    [Tooltip("UI Image displaying the health bar")]
    [SerializeField] private Image healthImage;

    private void Update()
    {
        // Normalize the health to a sprite index
        int index = Mathf.Clamp(Mathf.RoundToInt(playerHealth.GetCurrentHealth()), 0, healthSprites.Length - 1);

        healthImage.sprite = healthSprites[index];
    }
}
