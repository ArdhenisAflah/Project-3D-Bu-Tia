using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the player's energy resource.
/// Attach to an empty GameObject "EnergyManager" in the scene.
/// Mirrors the pattern of ScoreManager for consistency.
/// </summary>
public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;

    [Header("UI References")]
    [Tooltip("Drag Image (fill) atau Slider ke sini untuk energy bar")]
    public Slider energySlider;          // Pakai Slider UI
    public Image energyFillImage;        // Atau pakai Image dengan Image Type = Filled
    public TextMeshProUGUI energyText;   // Label opsional, contoh: "45 / 100"

    [Header("Color Feedback")]
    [Tooltip("Warna bar saat energi rendah, sedang, penuh")]
    public Color colorLow = new Color(0.9f, 0.2f, 0.2f); // merah
    public Color colorMid = new Color(0.9f, 0.7f, 0.1f); // kuning
    public Color colorFull = new Color(0.2f, 0.8f, 0.3f); // hijau
    public float lowThreshold = 0.25f;  // di bawah 25% = merah
    public float midThreshold = 0.60f;  // di bawah 60% = kuning

    [Header("Events (opsional)")]
    public UnityEngine.Events.UnityEvent onEnergyFull;    // Panggil event saat penuh
    public UnityEngine.Events.UnityEvent onEnergyEmpty;   // Panggil event saat kosong

    private bool wasFull = false;
    private bool wasEmpty = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Inisialisasi slider max
        if (energySlider != null)
            energySlider.maxValue = maxEnergy;

        UpdateUI();
    }

    // ── Public API ────────────────────────────────────────────

    /// <summary>Tambah energi. Dipanggil dari Asteroid.cs saat kena player/bullet.</summary>
    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
        UpdateUI();
        CheckEvents();
    }

    /// <summary>Kurangi energi. Panggil saat energi dikonsumsi (misalnya aktifkan shield).</summary>
    public void UseEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        UpdateUI();
        CheckEvents();
    }

    /// <summary>Cek apakah energi cukup untuk dipakai.</summary>
    public bool HasEnergy(float amount)
    {
        return currentEnergy >= amount;
    }

    /// <summary>Reset energi ke nol (misalnya saat game over / restart).</summary>
    public void ResetEnergy()
    {
        currentEnergy = 0f;
        wasFull = false;
        wasEmpty = true;
        UpdateUI();
    }

    // ── Internal ──────────────────────────────────────────────

    void UpdateUI()
    {
        float ratio = currentEnergy / maxEnergy;

        // Slider
        if (energySlider != null)
            energySlider.value = currentEnergy;

        // Image fill
        if (energyFillImage != null)
            energyFillImage.fillAmount = ratio;

        // Text label
        if (energyText != null)
            energyText.text = $"{Mathf.RoundToInt(currentEnergy)} / {Mathf.RoundToInt(maxEnergy)}";

        // Color feedback
        Color targetColor = ratio <= lowThreshold ? colorLow
                          : ratio <= midThreshold ? colorMid
                          : colorFull;

        if (energySlider != null)
        {
            // Cari Fill area dari slider
            var fill = energySlider.fillRect?.GetComponent<Image>();
            if (fill != null) fill.color = targetColor;
        }

        if (energyFillImage != null)
            energyFillImage.color = targetColor;
    }

    void CheckEvents()
    {
        bool isFull = currentEnergy >= maxEnergy;
        bool isEmpty = currentEnergy <= 0f;

        if (isFull && !wasFull)
        {
            wasFull = true;
            onEnergyFull?.Invoke();
        }
        if (!isFull) wasFull = false;

        if (isEmpty && !wasEmpty)
        {
            wasEmpty = true;
            onEnergyEmpty?.Invoke();
        }
        if (!isEmpty) wasEmpty = false;
    }
}