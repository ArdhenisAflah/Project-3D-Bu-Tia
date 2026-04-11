using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour, IPoolable
{
    [Header("Movement")]
    public float Speed = 0f;
    public float RotationSpeed = 0f;

    [Header("Size Variants")]
    public float minScale = 0.5f;
    public float maxScale = 2.5f;

    [Header("Sine Wave Movement")]
    public float driftAmplitude = 0.8f;
    public float driftFrequency = 0.8f;
    public float rotationSpeed = 15f;



    [Header("Pop Out Effect")]
    public float popDuration = 0.6f;   // Berapa lama animasi pop
    public AnimationCurve popCurve;

    [Header("Pool")]
    public string poolTag = "Asteroid";

    private Rigidbody rb;
    private float lifetime;
    private float maxLifetime = 30f;


    private Vector3 originPos;
    private Vector2 driftOffset;
    private float timeOffset;
    private float popTimer = 0f;
    private bool isPopping = true;
    private float targetScale;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void OnSpawn()
    {

        targetScale = Random.Range(minScale, maxScale);

        // Mulai dari nol
        transform.localScale = Vector3.zero;
        popTimer = 0f;
        isPopping = true;

        originPos = transform.position;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
        driftOffset = new Vector2(
            Random.Range(0f, Mathf.PI * 2f),
            Random.Range(0f, Mathf.PI * 2f)
        );

        lifetime = 0f;
    }

    public void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        HandlePopOut();
        HandleDrift();
        // Auto return ke pool setelah maxLifetime
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
            ReturnToPool();
    }

    void OnTriggerEnter(Collider other)
    {
        // Kena spaceship atau projectile
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {
            ScoreManager.Instance.AddScore(10);
            ReturnToPool();
        }
    }


    void HandleDrift()
    {
        float t = Time.time * driftFrequency + timeOffset;
        float dx = Mathf.Sin(t + driftOffset.x) * driftAmplitude;
        float dz = Mathf.Cos(t + driftOffset.y) * driftAmplitude;

        transform.position = new Vector3(
            originPos.x + dx,
            originPos.y,
            originPos.z + dz
        );

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }


    void HandlePopOut()
    {
        if (!isPopping) return;

        popTimer += Time.deltaTime;
        float t = Mathf.Clamp01(popTimer / popDuration);

        // Curve langsung handle overshoot, tidak perlu rumus
        transform.localScale = Vector3.one * (popCurve.Evaluate(t) * targetScale);
        if (t >= 1f) isPopping = false;


    }


    public void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }


}