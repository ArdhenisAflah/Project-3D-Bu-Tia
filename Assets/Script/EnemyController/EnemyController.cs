using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public string poolTag = "Enemy"; // Pastikan sama dengan di ObjectPool

    void Update()
    {
        // Bergerak maju searah rotasi yang sudah diberikan oleh Spawner
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Logika tabrakan (Ganti Tag sesuai kebutuhan)
        if (other.CompareTag("Bullet") || other.CompareTag("MainStation") || other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(20);

            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // Kembali ke pool menggunakan tag yang ditentukan
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
}