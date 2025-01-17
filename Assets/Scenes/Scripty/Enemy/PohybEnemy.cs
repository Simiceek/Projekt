using UnityEngine;

public class PohybEnemy : MonoBehaviour
{
    public Transform portalTransform;   // Transform port�lu
    public Transform playerTransform;   // Transform hr��e
    public float speed = 2f;            // Rychlost nep��tele
    public float detectionRadius = 5f;  // Polom�r detekce hr��e
    public int portalDamage = 10;
    public int playerDamage = 10;       // Po�kozen�, kter� nep��tel zp�sob� port�lu
    private bool isTouchingPortal = false; // Flag pro kontrolu, zda se nep��tel dot�k� port�lu
    private float damageCooldown = 1f;  // Cooldown mezi �toky na port�l
    private float lastDamageTime = 0f;  // �as posledn�ho �toku na port�l
    private PortalHealth portalHealth;  // Reference na skript �ivot� port�lu
    private PlayerHealth playerHealth;
    private bool isTouchingPlayer = false;

    void Start()
    {
        // Najde port�l podle tagu
        GameObject portal = GameObject.FindGameObjectWithTag("Portal");
        if (portal != null)
        {
            portalTransform = portal.transform;
            portalHealth = portal.GetComponent<PortalHealth>();
        }
        else
        {
            Debug.LogWarning("Port�l nebyl nalezen! Zkontrolujte, zda je spr�vn� ozna�en tagem 'Portal'.");
        }

        // Najde hr��e podle tagu
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Hr�� nebyl nalezen! Zkontrolujte, zda je spr�vn� ozna�en tagem 'Player'.");
        }
    }

    void Update()
    {
        // Pokud se dot�k� port�lu, ka�dou sekundu ub�r� jeho �ivoty
        if (isTouchingPortal && portalHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                portalHealth.TakeDamage(portalDamage);
                lastDamageTime = Time.time;
            }
            return; // Nep��tel se neh�be, kdy� �to�� na port�l
        }
        if (isTouchingPlayer && playerHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(playerDamage);
                lastDamageTime = Time.time;
            }
            return; // Nep��tel se neh�be, kdy� �to�� na port�l
        }

        // Pokud se nep��tel nedot�k� port�lu, pokra�uje v pohybu
        if (portalTransform == null || playerTransform == null) return;

        // Ur�� c�lov� sm�r
        Transform targetTransform = portalTransform;

        // Zjist�, jestli je hr�� v bl�zkosti
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            targetTransform = playerTransform;  // Pokud je hr�� v bl�zkosti, nastav� c�l na hr��e
        }

        // Pohyb sm�rem k c�li (bu� hr��, nebo port�l)
        //Vector2 direction = ((Vector2)targetTransform.position - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Pokud nep��tel za�ne kolidovat s port�lem, spust� �tok
        if (collision.CompareTag("Portal"))
        {
            Debug.Log("ddd");
            isTouchingPortal = true;
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("ddd");
            isTouchingPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Pokud nep��tel opust� kolizi s port�lem, zastav� �tok
        if (collision.CompareTag("Portal"))
        {
            isTouchingPortal = false;
        }
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
    public void SetDamage(int damage)
    {
        portalDamage = damage;
    }
}
