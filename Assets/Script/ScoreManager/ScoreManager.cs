using UnityEngine;
using TMPro; // Jika pakai TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int currentScore = 0;
    public TextMeshProUGUI scoreText; // Drag UI Text ke sini

    void Awake()
    {
        Instance = this;
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
}