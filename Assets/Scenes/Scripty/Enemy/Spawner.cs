using UnityEngine;
using TMPro; // Pro TextMeshPro
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // Prefab nep��tele
    public Transform portalTransform;    // Transform port�lu uprost�ed
    public float spawnDistance = 15f;    // Vzd�lenost od port�lu
    public Camera mainCamera;            // Hlavn� kamera

    public int initialEnemyCount = 10;      // Po�et nep��tel v prvn� vln�
    public float initialSpawnInterval = 3f; // Interval spawnov�n� v prvn� vln�
    public float wavePause = 10f;           // Pauza mezi vlnami

    // �k�lovac� parametry
    public int enemyCountIncrease = 5;       // O kolik v�ce nep��tel v dal�� vln�
    public float spawnIntervalDecrease = 0.2f; // O kolik se sn�� spawn interval
    public float healthIncrease = 0.1f;      // Zv��en� �ivot� nep��tel o procenta
    public float damageIncrease = 0.1f;      // Zv��en� po�kozen� nep��tel o procenta

    // UI prvky
    public TextMeshProUGUI waveText;        // TextMeshPro pro zobrazen� aktu�ln� vlny
    public TextMeshProUGUI enemiesLeftText; // TextMeshPro pro zobrazen� zb�vaj�c�ch nep��tel
    public TextMeshProUGUI countdownText;   // TextMeshPro pro zobrazen� odpo�tu mezi vlnami

    private int currentWave = 0;         // Aktu�ln� vlna
    private int enemiesSpawned = 0;      // Po�et spawnut�ch nep��tel
    private int enemiesLeft = 0;         // Po�et zb�vaj�c�ch nep��tel
    private bool isSpawning = false;     // Indik�tor prob�haj�c�ho spawnov�n�

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
            Debug.LogWarning("Hr�� nebyl nalezen! Zkontrolujte, zda je spr�vn� ozna�en tagem 'Player'.");
        }
        countdownText.gameObject.SetActive(false); // Skryje odpo�et na za��tku
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
        countdownText.gameObject.SetActive(false); // Skryje odpo�et p�i startu vlny
        currentWave++;
        enemiesSpawned = 0;
        enemiesLeft = GetEnemyCount(); // Inicializuje zb�vaj�c� nep��tele na za��tku vlny
        isSpawning = true;

        UpdateUI(); // Aktualizuje UI na za��tku vlny

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
        countdownText.gameObject.SetActive(true); // Zobraz� odpo�et

        float timer = wavePause;
        while (timer > 0)
        {
            playerHealth.Heal(100);
            countdownText.text = Mathf.Ceil(timer) + "s";
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countdownText.gameObject.SetActive(false); // Skryje odpo�et p�ed startem nov� vlny
        StartCoroutine(StartWave());
    }

    void SpawnEnemy()
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)portalTransform.position + spawnDirection * spawnDistance;

        if (IsOutsideCameraView(spawnPosition))
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Nastaven� �ivot� v UtokZivotyEnemy
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

            // Nastaven� damage v PohybEnemy
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
        // Aktualizace text� na UI
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
