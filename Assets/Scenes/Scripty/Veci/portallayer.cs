using UnityEngine;

public class portallayer : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    private SpriteRenderer objectRenderer;

    void Start()
    {
        // Získání renderù
        playerRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Porovnání pozice na ose Y
        if (transform.position.y + 0.7 > playerRenderer.transform.position.y)
        {
            objectRenderer.sortingOrder = playerRenderer.sortingOrder - 1; // Za hráèem
        }
        else
        {
            objectRenderer.sortingOrder = playerRenderer.sortingOrder + 1; // Pøed hráèem
        }
    }
}
