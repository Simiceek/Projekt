using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maxim�ln� po�et �ivot� port�lu
    private int currentHealth; // Aktu�ln� po�et �ivot� port�lu
    public TextMeshProUGUI healthText; // Reference na Text UI (TextMeshProUGUI)
    public Sprite fullHeart; // Sprite pro vypln�n� srdce
    public Sprite emptyHeart; // Sprite pro pr�zdn� srdce
    public int healthPerHeart = 10;

    public List<GameObject> hearts; // Seznam UI Image objekt� (ka�d� srdce je jeden Image)
    void Start()
    {
        currentHealth = maxHealth; // Inicializace �ivot�
        //UpdateHealthText();        // Aktualizace textu na za��tku
        UpdateHearts(); // Aktualizace srd��ek na za��tku hry
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Zabr�n�me tomu, aby �ivoty byly pod 0
        UpdateHearts(); // Aktualizace UI

        //UpdateHealthText(); // Aktualizace textu po zm�n� �ivot�

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
            Die();
        }
    }

    /*void UpdateHealthText()
    {
        // Aktualizace textu podle aktu�ln�ch �ivot�
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
                    renderer.sprite = fullHeart; // Vypln�n� srd��ko
                }
                else
                {
                    renderer.sprite = emptyHeart; // Pr�zdn� srd��ko
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
