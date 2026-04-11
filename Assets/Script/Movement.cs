using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 20f;
    public float moveSpeed = 15f;
    public float deceleration = 8f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;

    [Header("Boost Settings")]
    public float boostMultiplier = 2.5f;
    public float boostDuration = 3f;
    public float boostCooldown = 5f;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float boostTimer = 0f;
    private float boostCooldownTimer = 0f;
    private bool isBoosting = false;
    private float fixedY;
    private Camera mainCam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0.5f;

        rb.constraints = RigidbodyConstraints.FreezePositionY
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;

        fixedY = transform.position.y;

        // Pakai Main Camera — CinemachineBrain otomatis control ini
        mainCam = Camera.main;

        // Auto-assign virtual camera kalau belum di-set di Inspector
        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        // Pastikan Virtual Camera follow spaceship ini
        if (virtualCamera != null)
            virtualCamera.Follow = this.transform;
    }

    void Update()
    {
        HandleBoost();
    }

    void FixedUpdate()
    {
        HandleRotationToMouse();
        MoveForward();
        LockY();
    }

    void HandleRotationToMouse()
    {
        // Gunakan mainCam yang dikendalikan CinemachineBrain
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, fixedY, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);

            Vector3 direction = mouseWorldPos - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion smoothed = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(smoothed);
        }
    }

    void MoveForward()
    {
        float speed = moveSpeed * (isBoosting ? boostMultiplier : 1f);
        Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        rb.velocity = new Vector3(
         flatForward.x * speed,
         0f,
         flatForward.z * speed
        );
    }

    void LockY()
    {
        Vector3 pos = rb.position;
        pos.y = fixedY;
        rb.position = pos;

        Vector3 vel = rb.velocity;
        vel.y = 0f;
        rb.velocity = vel;
    }

    void HandleBoost()
    {
        if (boostCooldownTimer > 0f) boostCooldownTimer -= Time.deltaTime;

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                boostCooldownTimer = boostCooldown;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isBoosting && boostCooldownTimer <= 0f)
        {
            isBoosting = true;
            boostTimer = boostDuration;
        }
    }

    void OnGUI()
    {
        float speed = moveSpeed * (isBoosting ? boostMultiplier : 1f);
        GUI.Label(new Rect(10, 10, 350, 20), $"Speed: {speed:F1}");
        GUI.Label(new Rect(10, 30, 350, 20),
            $"Boost: {(isBoosting ? $"ACTIVE ({boostTimer:F1}s)" : boostCooldownTimer > 0 ? $"Cooldown ({boostCooldownTimer:F1}s)" : "READY")}");
    }
}