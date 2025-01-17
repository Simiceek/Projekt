using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximální poèet životù portálu
    private int currentHealth; // Aktuální poèet životù portálu
    public TextMeshProUGUI healthText; // Reference na Text UI (TextMeshProUGUI)
    public Sprite fullHeart; // Sprite pro vyplnìné srdce
    public Sprite emptyHeart; // Sprite pro prázdné srdce
    public int healthPerHeart = 10;

    public List<GameObject> hearts; // Seznam UI Image objektù (každé srdce je jeden Image)
    void Start()
    {
        currentHealth = maxHealth; // Inicializace životù
        //UpdateHealthText();        // Aktualizace textu na zaèátku
        UpdateHearts(); // Aktualizace srdíèek na zaèátku hry
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Zabráníme tomu, aby životy byly pod 0
        UpdateHearts(); // Aktualizace UI

        //UpdateHealthText(); // Aktualizace textu po zmìnì životù

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Die();
        }
    }

    /*void UpdateHealthText()
    {
        // Aktualizace textu podle aktuálních životù
        if (healthText != null)
        {
            healthText.text = "Player: " + currentHealth;
        }
    }*/

    void Die()
    {
        Destroy(gameObject);
    }
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            SpriteRenderer renderer = hearts[i].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                if (i < currentHealth / healthPerHeart)
                {
                    renderer.sprite = fullHeart; // Vyplnìné srdíèko
                }
                else
                {
                    renderer.sprite = emptyHeart; // Prázdné srdíèko
                }
            }
        }
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        UpdateHearts();
    }
}
