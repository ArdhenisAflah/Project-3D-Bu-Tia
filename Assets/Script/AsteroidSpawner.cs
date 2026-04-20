using System.Collections;
using TMPro;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Spawn Settings")]
    public string[] asteroids = new string[] { "Asteroid", "Asteroid1", "Asteroid2", "Asteroid3" };
    public float spawnRadius = 30f;
    public float despawnRadius = 40f;
    public float spawnInterval = 1.5f;
    public int maxAsteroids = 30;

    // HAPUS semua wave variables — WaveManager yang kontrol sekarang
    [HideInInspector] public bool enableWaves = false; // selalu false

    private float spawnTimer = 0f;
    private int activeCount = 0;

    void Update()
    {
        if (player == null) return;

        TrackActiveAsteroids();


    }

    void TrackActiveAsteroids()
    {
        Asteroid[] active = FindObjectsOfType<Asteroid>();
        activeCount = 0;
        foreach (var a in active)
            if (a.gameObject.activeSelf) activeCount++;
    }


    // Dipanggil HANYA oleh WaveManager
    public void SpawnWaveExternal(int amount)
    {
        int toSpawn = Mathf.Min(amount, maxAsteroids - activeCount);
        for (int i = 0; i < toSpawn; i++)
            SpawnAsteroid();
    }

    void SpawnAsteroid()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float dist = Random.Range(spawnRadius * 0.8f, spawnRadius);

        Vector3 spawnPos = new Vector3(
            player.position.x + Mathf.Cos(angle) * dist,
            player.position.y,
            player.position.z + Mathf.Sin(angle) * dist
        );

        Vector3 dirToPlayer = (player.position - spawnPos).normalized;
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.3f, 0.3f));
        Quaternion spawnRot = Quaternion.LookRotation(dirToPlayer + randomOffset);

        ObjectPool.Instance.SpawnFromPool(asteroids[Random.Range(0, asteroids.Length)], spawnPos, spawnRot);
    }

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
            Vector3 newPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}