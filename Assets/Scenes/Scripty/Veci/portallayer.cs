using UnityEngine;

public class portallayer : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    private SpriteRenderer objectRenderer;

    void Start()
    {
        // Z�sk�n� render�
        playerRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Porovn�n� pozice na ose Y
        if (transform.position.y + 0.7 > playerRenderer.transform.position.y)
        {
            objectRenderer.sortingOrder = playerRenderer.sortingOrder - 1; // Za hr��em
        }
        else
        {
            objectRenderer.sortingOrder = playerRenderer.sortingOrder + 1; // P�ed hr��em
        }
    }
}
