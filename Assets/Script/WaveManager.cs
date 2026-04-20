using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine;
using Unity.Mathematics;
using System;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("References")]
    public AsteroidSpawner asteroidSpawner;
    public EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    public float timeBetweenWaves = 50f;

    [Header("Asteroid Per Wave")]
    public int baseAsteroids = 10;
    public int asteroidIncreasePerWave = 2; // tiap wave nambah

    [Header("Enemy Per Wave")]
    public int baseEnemies = 4;
    public int enemyIncreasePerWave = 1;

    [Header("UI")]
    public TextMeshProUGUI waveText;

    public int currentWave = 0;
    private float waveTimer = 0f;
    private bool gameOver = false;

    void Awake() => Instance = this;

    void Start()
    {
        // Matiin wave logic di spawner masing2, biar WaveManager yang kontrol
        asteroidSpawner.enableWaves = false;
        enemySpawner.enableWaves = false;

        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (gameOver) return;

        waveTimer += Time.deltaTime;
        if (waveTimer >= timeBetweenWaves)
        {
            waveTimer = 0f;
            StartCoroutine(StartWave());
        }
    }

    IEnumerator StartWave()
    {
        currentWave++;

        // Hitung jumlah spawn wave ini
        int asteroidsThisWave = baseAsteroids + (asteroidIncreasePerWave * (currentWave - 1));
        int enemiesThisWave = baseEnemies + (UnityEngine.Random.Range(0, 3) * (currentWave - 1));

        // Tampilkan UI
        yield return StartCoroutine(ShowWaveText());

        // Spawn
        asteroidSpawner.SpawnWaveExternal(asteroidsThisWave);
        enemySpawner.SpawnWaveExternal(enemiesThisWave);

        Debug.Log($"Wave {currentWave} — Asteroids: {asteroidsThisWave}, Enemies: {enemiesThisWave}");
    }

    IEnumerator ShowWaveText()
    {
        waveText.gameObject.SetActive(true);
        waveText.text = $"WAVE {currentWave}!";
        yield return new WaitForSeconds(2f);
        waveText.gameObject.SetActive(false);
    }

    public void SetGameOver()
    {
        gameOver = true;
    }
}
