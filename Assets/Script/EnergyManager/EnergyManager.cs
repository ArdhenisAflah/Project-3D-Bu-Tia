using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Header("Energy Settings")]
    public float maxEnergy = 100f;
    public float currentEnergy = 0f;

    [Header("UI References")]
    public Slider energySlider;
    public TextMeshProUGUI energyText;

    [Header("Shield")]
    public Transform shieldTransform; // drag shield object

    [Header("Events")]
    public UnityEngine.Events.UnityEvent onEnergyFull;
    public UnityEngine.Events.UnityEvent onEnergy50;
    public UnityEngine.Events.UnityEvent onEnergy25;
    public UnityEngine.Events.UnityEvent onEnergyEmpty;

    private Coroutine smoothDampSliderC;
    private float currentVelocity = 0f;

    private bool wasFull = false;
    private bool was50 = false;
    private bool was25 = false;
    private bool wasEmpty = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (energySlider != null)
            energySlider.maxValue = maxEnergy;

        UpdateUI();
        CheckEvents();
    }

    // ── Public API ────────────────────────────────────────────

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
        UpdateUI();
        CheckEvents();
    }

    public void UseEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        UpdateUI();
        CheckEvents();
    }

    public bool HasEnergy(float amount)
    {
        return currentEnergy >= amount;
    }

    public void ResetEnergy()
    {
        currentEnergy = 0f;
        wasFull = false;
        was50 = false;
        was25 = false;
        wasEmpty = true;
        UpdateUI();
        CheckEvents();
    }

    // ── UI ────────────────────────────────────────────────────

    void UpdateUI()
    {
        if (smoothDampSliderC != null)
            StopCoroutine(smoothDampSliderC);
        smoothDampSliderC = StartCoroutine(SmoothDampSlider(currentEnergy));

        if (energyText != null)
            energyText.text = $"{Mathf.RoundToInt(currentEnergy)} / {Mathf.RoundToInt(maxEnergy)}";
    }

    IEnumerator SmoothDampSlider(float target)
    {
        if (energySlider == null) yield break;

        float smoothTime = 0.5f;
        while (Mathf.Abs(energySlider.value - target) > 0.001f)
        {
            energySlider.value = Mathf.SmoothDamp(
                energySlider.value, target, ref currentVelocity, smoothTime);
            yield return null;
        }
        energySlider.value = target;
    }

    // ── Events ────────────────────────────────────────────────

    void CheckEvents()
    {
        float ratio = currentEnergy / maxEnergy;

        bool isFull = ratio >= 1f;
        bool is50 = ratio <= 0.5f;
        bool is25 = ratio <= 0.25f;
        bool isEmpty = ratio <= 0f;

        // Shield — aktif kalau ada energy, mati kalau kosong
        if (shieldTransform != null)
            shieldTransform.gameObject.SetActive(!isEmpty);

        // Full
        if (isFull && !wasFull)
        {
            wasFull = true; was50 = false; was25 = false; wasEmpty = false;
            onEnergyFull?.Invoke();
        }
        if (!isFull) wasFull = false;

        // 50%
        if (is50 && !was50 && !is25 && !isEmpty)
        {
            was50 = true; wasFull = false;
            onEnergy50?.Invoke();
        }

        // 25%
        if (is25 && !was25 && !isEmpty)
        {
            was25 = true; was50 = false;
            onEnergy25?.Invoke();
        }

        // Empty
        if (isEmpty && !wasEmpty)
        {
            wasEmpty = true; was25 = false;
            onEnergyEmpty?.Invoke();
        }
        if (!isEmpty) wasEmpty = false;
    }
}