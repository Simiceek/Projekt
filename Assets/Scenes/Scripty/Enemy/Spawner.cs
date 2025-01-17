using UnityEngine;
using TMPro; // Pro TextMeshPro
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // Prefab nepøítele
    public Transform portalTransform;    // Transform portálu uprostøed
    public float spawnDistance = 15f;    // Vzdálenost od portálu
    public Camera mainCamera;            // Hlavní kamera

    public int initialEnemyCount = 10;      // Poèet nepøátel v první vlnì
    public float initialSpawnInterval = 3f; // Interval spawnování v první vlnì
    public float wavePause = 10f;           // Pauza mezi vlnami

    // Škálovací parametry
    public int enemyCountIncrease = 5;       // O kolik více nepøátel v další vlnì
    public float spawnIntervalDecrease = 0.2f; // O kolik se sníží spawn interval
    public float healthIncrease = 0.1f;      // Zvýšení životù nepøátel o procenta
    public float damageIncrease = 0.1f;      // Zvýšení poškození nepøátel o procenta

    // UI prvky
    public TextMeshProUGUI waveText;        // TextMeshPro pro zobrazení aktuální vlny
    public TextMeshProUGUI enemiesLeftText; // TextMeshPro pro zobrazení zbývajících nepøátel
    public TextMeshProUGUI countdownText;   // TextMeshPro pro zobrazení odpoètu mezi vlnami

    private int currentWave = 0;         // Aktuální vlna
    private int enemiesSpawned = 0;      // Poèet spawnutých nepøátel
    private int enemiesLeft = 0;         // Poèet zbývajících nepøátel
    private bool isSpawning = false;     // Indikátor probíhajícího spawnování

    private PlayerHealth playerHealth;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Hráè nebyl nalezen! Zkontrolujte, zda je správnì oznaèen tagem 'Player'.");
        }
        countdownText.gameObject.SetActive(false); // Skryje odpoèet na zaèátku
        UpdateUI();
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (isSpawning && enemiesLeft <= 0 && enemiesSpawned >= GetEnemyCount())
        {
            isSpawning = false;
            StartCoroutine(NextWave());
        }
    }

    IEnumerator StartWave()
    {
        countdownText.gameObject.SetActive(false); // Skryje odpoèet pøi startu vlny
        currentWave++;
        enemiesSpawned = 0;
        enemiesLeft = GetEnemyCount(); // Inicializuje zbývající nepøátele na zaèátku vlny
        isSpawning = true;

        UpdateUI(); // Aktualizuje UI na zaèátku vlny

        while (enemiesSpawned < GetEnemyCount())
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(Mathf.Max(0.5f, GetSpawnInterval()));
        }
    }

    IEnumerator NextWave()
    {
        Debug.Log("Wave " + currentWave + " completed. Next wave in " + wavePause + " seconds.");
        countdownText.gameObject.SetActive(true); // Zobrazí odpoèet

        float timer = wavePause;
        while (timer > 0)
        {
            playerHealth.Heal(100);
            countdownText.text = Mathf.Ceil(timer) + "s";
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countdownText.gameObject.SetActive(false); // Skryje odpoèet pøed startem nové vlny
        StartCoroutine(StartWave());
    }

    void SpawnEnemy()
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)portalTransform.position + spawnDirection * spawnDistance;

        if (IsOutsideCameraView(spawnPosition))
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Nastavení životù v UtokZivotyEnemy
            UtokZivotyEnemy healthScript = enemy.GetComponent<UtokZivotyEnemy>();
            if (healthScript != null)
            {
                int health = GetEnemyHealth();
                healthScript.SetStats(health, () =>
                {
                    enemiesLeft--;
                    UpdateUI();
                });
            }

            // Nastavení damage v PohybEnemy
            PohybEnemy movementScript = enemy.GetComponent<PohybEnemy>();
            if (movementScript != null)
            {
                int damage = GetEnemyDamage();
                movementScript.SetDamage(damage);
            }
        }
        else
        {
            SpawnEnemy();
        }
    }

    void UpdateUI()
    {
        // Aktualizace textù na UI
        if (waveText != null)
            waveText.text = "Wave: " + currentWave;

        if (enemiesLeftText != null)
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft;
    }

    bool IsOutsideCameraView(Vector2 position)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    int GetEnemyCount()
    {
        return initialEnemyCount + (enemyCountIncrease * (currentWave - 1));
    }

    float GetSpawnInterval()
    {
        return initialSpawnInterval - (spawnIntervalDecrease * (currentWave - 1));
    }

    int GetEnemyHealth()
    {
        return Mathf.RoundToInt(100 * (1 + healthIncrease * (currentWave - 1)));
    }

    int GetEnemyDamage()
    {
        return Mathf.RoundToInt(10 * (1 + damageIncrease * (currentWave - 1)));
    }
}
