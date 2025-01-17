using UnityEngine;

public class PohybEnemy : MonoBehaviour
{
    public Transform portalTransform;   // Transform portálu
    public Transform playerTransform;   // Transform hráèe
    public float speed = 2f;            // Rychlost nepøítele
    public float detectionRadius = 5f;  // Polomìr detekce hráèe
    public int portalDamage = 10;
    public int playerDamage = 10;       // Poškození, které nepøítel zpùsobí portálu
    private bool isTouchingPortal = false; // Flag pro kontrolu, zda se nepøítel dotýká portálu
    private float damageCooldown = 1f;  // Cooldown mezi útoky na portál
    private float lastDamageTime = 0f;  // Èas posledního útoku na portál
    private PortalHealth portalHealth;  // Reference na skript životù portálu
    private PlayerHealth playerHealth;
    private bool isTouchingPlayer = false;

    void Start()
    {
        // Najde portál podle tagu
        GameObject portal = GameObject.FindGameObjectWithTag("Portal");
        if (portal != null)
        {
            portalTransform = portal.transform;
            portalHealth = portal.GetComponent<PortalHealth>();
        }
        else
        {
            Debug.LogWarning("Portál nebyl nalezen! Zkontrolujte, zda je správnì oznaèen tagem 'Portal'.");
        }

        // Najde hráèe podle tagu
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Hráè nebyl nalezen! Zkontrolujte, zda je správnì oznaèen tagem 'Player'.");
        }
    }

    void Update()
    {
        // Pokud se dotýká portálu, každou sekundu ubírá jeho životy
        if (isTouchingPortal && portalHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                portalHealth.TakeDamage(portalDamage);
                lastDamageTime = Time.time;
            }
            return; // Nepøítel se nehýbe, když útoèí na portál
        }
        if (isTouchingPlayer && playerHealth != null)
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(playerDamage);
                lastDamageTime = Time.time;
            }
            return; // Nepøítel se nehýbe, když útoèí na portál
        }

        // Pokud se nepøítel nedotýká portálu, pokraèuje v pohybu
        if (portalTransform == null || playerTransform == null) return;

        // Urèí cílový smìr
        Transform targetTransform = portalTransform;

        // Zjistí, jestli je hráè v blízkosti
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            targetTransform = playerTransform;  // Pokud je hráè v blízkosti, nastaví cíl na hráèe
        }

        // Pohyb smìrem k cíli (buï hráè, nebo portál)
        //Vector2 direction = ((Vector2)targetTransform.position - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Pokud nepøítel zaène kolidovat s portálem, spustí útok
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
        // Pokud nepøítel opustí kolizi s portálem, zastaví útok
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
