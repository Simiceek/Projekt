using UnityEngine;
using TMPro; // Pokud pou��v�te TextMeshPro
using UnityEngine.SceneManagement;

public class PortalHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maxim�ln� po�et �ivot� port�lu
    private int currentHealth; // Aktu�ln� po�et �ivot� port�lu
    public TextMeshProUGUI healthText; // Reference na Text UI (TextMeshProUGUI)

    void Start()
    {
        currentHealth = maxHealth; // Inicializace �ivot�
        UpdateHealthText();        // Aktualizace textu na za��tku
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Zabr�n�me tomu, aby �ivoty byly pod 0

        Debug.Log("Portal Health: " + currentHealth);
        UpdateHealthText(); // Aktualizace textu po zm�n� �ivot�

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Die();
        }
    }

    void UpdateHealthText()
    {
        // Aktualizace textu podle aktu�ln�ch �ivot�
        if (healthText != null)
        {
            healthText.text = "Portal Health: " + currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("Portal destroyed!");
        // Logika pro zni�en� port�lu (nap�. ukon�en� hry)
        Destroy(gameObject);
    }
}
