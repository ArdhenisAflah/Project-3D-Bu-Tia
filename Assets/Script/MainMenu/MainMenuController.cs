using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string StartPlayDestinationScene = "Gameplay";

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject howToPlayPanel; // drag panel How To Play

    void Start()
    {
        // Pastikan panel tertutup di awal
        howToPlayPanel.SetActive(false);
    }

    public void OnPlayButton()
    {
        AudioManager.Instance.PlaySFX(0);
        SceneManager.LoadScene(StartPlayDestinationScene);
    }

    public void OnHowToPlay()
    {
        AudioManager.Instance.PlaySFX(0);
        howToPlayPanel.SetActive(true); // popup muncul di atas
    }

    public void OnCloseHowToPlay()
    {
        AudioManager.Instance.PlaySFX(0);
        howToPlayPanel.SetActive(false); // tutup panel
    }

    public void OnQuitButton()
    {
        AudioManager.Instance.PlaySFX(0);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}