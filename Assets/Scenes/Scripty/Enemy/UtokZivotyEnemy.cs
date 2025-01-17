using System;
using UnityEngine;
using UnityEngine.UI; // Pro Slider komponentu
using static UnityEngine.EventSystems.EventTrigger;

public class UtokZivotyEnemy : MonoBehaviour
{
    private int maxHealth;             // Maxim�ln� po�et �ivot� nep��tele
    private int currentHealth;         // Aktu�ln� po�et �ivot� nep��tele
    private Rigidbody2D rb;            // Rigidbody nep��tele
    private bool isKnockedBack = false; // Indikuje, zda nep��tel pr�v� proch�z� knockbackem
    private Action onDeathCallback;    // Callback pro informov�n� o smrti
    public int penize;

    public Slider healthBar;           // Odkaz na Slider (Health Bar)


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth; // Inicializace max hodnoty health baru
            healthBar.value = currentHealth; // Nastav� aktu�ln� hodnotu
        }
    }

    public void SetStats(int health, Action onDeath)
    {
        maxHealth = health;
        currentHealth = health;
        onDeathCallback = onDeath;

        // Aktualizace health baru
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Zajist�, �e �ivoty neklesnou pod 0

        // Aktualizace health baru
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Manager druhyScript = FindObjectOfType<Manager>();
            druhyScript.AddMoney(1);
            Die();
        }
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (isKnockedBack) return;

        isKnockedBack = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        Invoke(nameof(ResetKnockback), 0.3f);
    }

    void ResetKnockback()
    {
        isKnockedBack = false;
        rb.linearVelocity = Vector2.zero; // Zastav� pohyb po knockbacku
    }

    void Die()
    {
        onDeathCallback?.Invoke();
        Destroy(gameObject);
    }
}
