using UnityEngine;
using TMPro; // Pokud používáte TextMeshPro
using UnityEngine.SceneManagement;

public class PortalHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximální poèet životù portálu
    private int currentHealth; // Aktuální poèet životù portálu
    public TextMeshProUGUI healthText; // Reference na Text UI (TextMeshProUGUI)

    void Start()
    {
        currentHealth = maxHealth; // Inicializace životù
        UpdateHealthText();        // Aktualizace textu na zaèátku
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Zabráníme tomu, aby životy byly pod 0

        Debug.Log("Portal Health: " + currentHealth);
        UpdateHealthText(); // Aktualizace textu po zmìnì životù

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Die();
        }
    }

    void UpdateHealthText()
    {
        // Aktualizace textu podle aktuálních životù
        if (healthText != null)
        {
            healthText.text = "Portal Health: " + currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("Portal destroyed!");
        // Logika pro znièení portálu (napø. ukonèení hry)
        Destroy(gameObject);
    }
}
