using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The player's maximum health")]
    [SerializeField] private float maxHealth;
    [Tooltip("Connect to Game Over UI")]
    [SerializeField] private UnityEvent gameOver;

    private float currentHealth;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Player Starting Health: " + currentHealth);
    }
    
    public void TakeDamage() // The player takes 1 damage
    {
        currentHealth--;
        Debug.Log("Damage Taken. Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(float damage) // The player looses 'damage' amount of health
    {
        currentHealth -= damage;
        Debug.Log("Damage Taken. Current Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void HealDamage() // Heals a player to their max health,
    {
        currentHealth = maxHealth;
        Debug.Log("Player healed to max");
    }

    public void HealDamage(float health) // Heals a player 'health' amount of health
    {
        currentHealth += health;
        Debug.Log("Player healed by " + health);
    }

    public void Death()
    {
        Debug.Log("Player dies");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Loads current scene. Will likely implement game over screen
        gameOver.Invoke();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
