using System.Collections;
using TMPro;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Target")]
    public Transform player; // Drag spaceship di Inspector

    [Header("Spawn Settings")]
    // public string asteroidTag = "Asteroid";
    // public string asteroidTag1 = "Asteroid1";
    // public string asteroidTag2 = "Asteroid2";
    // public string asteroidTag3 = "Asteroid3";

    public string[] asteroids = new string[] { "Asteroid", "Asteroid1", "Asteroid2", "Asteroid3" };
    public float spawnRadius = 30f;  // Jarak spawn dari player
    public float despawnRadius = 40f;  // Auto-despawn kalau terlalu jauh
    public float spawnInterval = 1.5f;
    public int maxAsteroids = 30;
    public int spawnBatchSize = 2;    // Spawn berapa per interval

    [Header("Wave Settings")]
    public bool enableWaves = true;
    public float timeBetweenWaves = 20f;
    public int asteroidsPerWave = 10;

    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    public int waveCounter = 0;
    private int activeCount = 0;

    public TextMeshProUGUI waveText;

    void Update()
    {
        if (player == null) return;

        TrackActiveAsteroids();

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnBatch();
        }

        if (enableWaves)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= timeBetweenWaves)
            {
                waveTimer = 0f;
                SpawnWave();
            }
        }
    }

    void TrackActiveAsteroids()
    {
        // Count asteroid aktif di scene
        Asteroid[] active = FindObjectsOfType<Asteroid>();
        activeCount = 0;
        foreach (var a in active)
            if (a.gameObject.activeSelf) activeCount++;
    }

    void SpawnBatch()
    {
        if (activeCount >= maxAsteroids) return;

        int toSpawn = Mathf.Min(spawnBatchSize, maxAsteroids - activeCount);
        for (int i = 0; i < toSpawn; i++)
            SpawnAsteroid();
    }

    void SpawnWave()
    {
        waveCounter++;
        Debug.Log("Asteroid Wave!");
        StartCoroutine(WaveShowTextAnim());
        int toSpawn = Mathf.Min(asteroidsPerWave, maxAsteroids - activeCount);
        for (int i = 0; i < toSpawn; i++)
            SpawnAsteroid();
    }

    IEnumerator WaveShowTextAnim()
    {
        waveText.gameObject.SetActive(true);
        waveText.text = $"WAVE {waveCounter}!";
        yield return new WaitForSeconds(2f);
        waveText.gameObject.SetActive(false);
    }

    void SpawnAsteroid()
    {
        // Posisi random di lingkaran sekitar player (XZ plane only)
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float dist = Random.Range(spawnRadius * 0.8f, spawnRadius);


        // 1.57f = Depan, 4.71f = Belakang
        // float[] validAngles = { 0f, 1.57f };
        // float baseAngle = validAngles[Random.Range(0, validAngles.Length)];

        // Tambahkan sedikit random agar tidak kaku di satu titik
        // float finalAngle = baseAngle + Random.Range(-0.4f, 0.4f);
        Vector3 spawnPos = new Vector3(
            player.position.x + Mathf.Cos(angle) * dist,
            player.position.y, // Sama dengan Y spaceship
            player.position.z + Mathf.Sin(angle) * dist
        );

        // Rotasi arah menuju player + sedikit random offset
        Vector3 dirToPlayer = (player.position - spawnPos).normalized;
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.3f, 0.3f), 0f,
            Random.Range(-0.3f, 0.3f)
        );
        Quaternion spawnRot = Quaternion.LookRotation(dirToPlayer + randomOffset);

        ObjectPool.Instance.SpawnFromPool(asteroids[Random.Range(0, 3)], spawnPos, spawnRot);
        // ObjectPool.Instance.SpawnFromPool(asteroidTag, spawnPos, spawnRot);
        // ObjectPool.Instance.SpawnFromPool(asteroidTag1, spawnPos, spawnRot);
        // ObjectPool.Instance.SpawnFromPool(asteroidTag2, spawnPos, spawnRot);
        // ObjectPool.Instance.SpawnFromPool(asteroidTag, spawnPos, spawnRot);
    }

    // Visualisasi radius di Editor
    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        DrawCircle(player.position, spawnRadius);

        Gizmos.color = Color.red;
        DrawCircle(player.position, despawnRadius);
    }

    void DrawCircle(Vector3 center, float radius)
    {
        int segments = 36;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * (i * angleStep);
            Vector3 newPoint = center + new Vector3(
                Mathf.Cos(rad) * radius, 0f,
                Mathf.Sin(rad) * radius
            );
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}