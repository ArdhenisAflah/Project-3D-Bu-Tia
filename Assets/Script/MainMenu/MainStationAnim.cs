using UnityEngine;

/// <summary>
/// Procedural idle animation for a spaceship decoration.
/// Attach this script to the "newspaceship" GameObject.
/// No Animator needed — pure code-driven motion.
/// </summary>
public class MainStationAnim : MonoBehaviour
{
    [Header("Hover (Naik Turun)")]
    [SerializeField] private float hoverAmplitude = 0.4f;   // seberapa jauh naik-turun (unit)
    [SerializeField] private float hoverSpeed = 0.8f;   // kecepatan naik-turun

    [Header("Roll (Manuver Sayap Kiri-Kanan)")]
    [SerializeField] private float rollAmplitude = 12f;    // derajat kemiringan sayap
    [SerializeField] private float rollSpeed = 0.55f;  // kecepatan rolling

    [Header("Pitch (Hidung Naik-Turun)")]
    [SerializeField] private float pitchAmplitude = 6f;     // derajat pitch
    [SerializeField] private float pitchSpeed = 0.65f;

    [Header("Yaw (Belok Kiri-Kanan)")]
    [SerializeField] private float yawAmplitude = 5f;     // derajat yaw
    [SerializeField] private float yawSpeed = 0.42f;

    [Header("Phase Offsets (biar tidak sinkron / lebih natural)")]
    [SerializeField] private float hoverPhase = 0f;
    [SerializeField] private float rollPhase = 1.3f;
    [SerializeField] private float pitchPhase = 2.5f;
    [SerializeField] private float yawPhase = 0.7f;

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.25f;      // semakin besar = semakin lembut

    // State
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Vector3 _currentPos;
    private Vector3 _posVelocity;

    private Vector3 _currentEuler;
    private Vector3 _eulerVelocity;

    private float _time;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;

        _currentPos = _startPosition;
        _currentEuler = _startRotation.eulerAngles;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        // ── Target position (hover) ──────────────────────────────
        float hoverY = Mathf.Sin(_time * hoverSpeed + hoverPhase) * hoverAmplitude;
        Vector3 targetPos = _startPosition + new Vector3(0f, hoverY, 0f);

        // // ── Target rotation ──────────────────────────────────────
        // Roll: sayap miring seperti manuver
        // float roll = Mathf.Sin(_time * rollSpeed + rollPhase) * rollAmplitude;
        float roll = _time * rollSpeed;


        // Gabungkan dengan rotasi awal
        Vector3 startEuler = _startRotation.eulerAngles;
        Vector3 targetEuler = new Vector3(
            startEuler.x,
            startEuler.y,
            startEuler.z + roll
        );

        // ── Smooth damp ──────────────────────────────────────────
        _currentPos = Vector3.SmoothDamp(
            _currentPos, targetPos, ref _posVelocity, smoothTime);

        _currentEuler = new Vector3(
            Mathf.SmoothDampAngle(_currentEuler.x, targetEuler.x, ref _eulerVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(_currentEuler.y, targetEuler.y, ref _eulerVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(_currentEuler.z, targetEuler.z, ref _eulerVelocity.z, smoothTime)
        );

        // ── Apply ────────────────────────────────────────────────
        transform.localPosition = _currentPos;
        transform.localRotation = Quaternion.Euler(_currentEuler);
    }

    // Panggil ini kalau mau reset ke posisi awal (opsional)
    public void ResetToIdle()
    {
        _time = 0f;
        transform.localPosition = _startPosition;
        transform.localRotation = _startRotation;
        _currentPos = _startPosition;
        _currentEuler = _startRotation.eulerAngles;
    }

#if UNITY_EDITOR
    // Gizmo biar keliatan range hover di Scene view
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = Application.isPlaying ? _startPosition : transform.localPosition;
        Gizmos.color = new Color(0.3f, 0.8f, 1f, 0.4f);
        Gizmos.DrawLine(
            transform.parent != null
                ? transform.parent.TransformPoint(pos + Vector3.up * hoverAmplitude)
                : pos + Vector3.up * hoverAmplitude,
            transform.parent != null
                ? transform.parent.TransformPoint(pos - Vector3.up * hoverAmplitude)
                : pos - Vector3.up * hoverAmplitude
        );
    }
#endif
}