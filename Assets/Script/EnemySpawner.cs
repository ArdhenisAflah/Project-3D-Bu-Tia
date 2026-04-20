using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Target Center")]
    public Transform mainStation; // Drag GameObject pusat (target musuh) ke sini

    [Header("Spawn Settings")]
    public string enemyTag = "Enemy"; // Tag di ObjectPool
    public float spawnRadius = 35f;  // Jarak spawn dari pusat
    public float spawnInterval = 5f;
    public int maxEnemies = 30;
    public int spawnBatchSize = 2;    // Jumlah musuh per interval

    [Header("Wave Settings")]
    public bool enableWaves = true;
    public float timeBetweenWaves = 30f;
    public int enemiesPerWave = 5;

    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private int activeCount = 0;

    void Update()
    {
        if (mainStation == null) return;

        // 1. Hitung musuh yang aktif (untuk membatasi maxEnemies)
        TrackActiveEnemies();
    }
    public void SpawnWaveExternal(int amount)
    {
        int toSpawn = Mathf.Min(amount, maxEnemies - activeCount);
        for (int i = 0; i < toSpawn; i++)
            SpawnEnemy();
    }
    void TrackActiveEnemies()
    {
        // Mencari semua objek dengan script EnemyController (sesuaikan nama script musuhmu)
        // Jika belum ada script di musuh, bisa pakai GameObject.FindGameObjectsWithTag(enemyTag)
        EnemyController[] active = FindObjectsOfType<EnemyController>();
        activeCount = 0;
        foreach (var e in active)
            if (e.gameObject.activeSelf) activeCount++;
    }


    void SpawnEnemy()
    {
        // Menentukan posisi random di lingkaran luar sekitar mainStation
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPos = new Vector3(
            mainStation.position.x + Mathf.Cos(angle) * spawnRadius,
            mainStation.position.y,
            mainStation.position.z + Mathf.Sin(angle) * spawnRadius
        );

        // Menghitung rotasi agar musuh menghadap ke arah MainStation
        Vector3 dirToStation = (mainStation.position - spawnPos).normalized;
        Quaternion spawnRot = Quaternion.LookRotation(dirToStation);

        // Memanggil dari ObjectPool
        ObjectPool.Instance.SpawnFromPool(enemyTag, spawnPos, spawnRot);
    }

    // --- Visualisasi Radius di Editor (Gizmos) ---
    void OnDrawGizmosSelected()
    {
        if (mainStation == null) return;

        Gizmos.color = Color.red; // Warna merah untuk radius musuh
        DrawCircle(mainStation.position, spawnRadius);
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