using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinLoseManager : MonoBehaviour
{
    public static WinLoseManager Instance;

    [Header("Player")]
    public Movement playerController; // drag player movement script

    [Header("Lose UI")]
    public GameObject losePanel;           // drag "Lose" object
    public TextMeshProUGUI loseTxt;        // drag "LoseTxt"
    public TextMeshProUGUI scoreText;      // drag Score (under ScoreTitle)
    public TextMeshProUGUI waveText;       // drag Wave (under WaveTitle)

    private bool isGameOver = false;

    void Awake() => Instance = this;

    public void TriggerLose()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop wave
        WaveManager.Instance.SetGameOver();

        // Stop player input
        if (playerController != null)
            playerController.enabled = false;

        // Isi UI
        if (scoreText != null)
            scoreText.text = GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text;
        if (waveText != null)
            waveText.text = WaveManager.Instance.currentWave.ToString();

        // Tampilkan lose panel
        losePanel.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (scoreText != null && isGameOver)
            scoreText.text = GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}