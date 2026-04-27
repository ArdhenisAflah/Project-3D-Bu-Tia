using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public string poolTag = "Enemy"; // Pastikan sama dengan di ObjectPool

    public GameObject ExplosionEffect;

    void Update()
    {
        // Bergerak maju searah rotasi yang sudah diberikan oleh Spawner
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Logika tabrakan (Ganti Tag sesuai kebutuhan)
        if (other.CompareTag("Bullet") || other.CompareTag("MainStation") || other.CompareTag("Player") || other.CompareTag("Shield"))
        {



            if (other.CompareTag("MainStation"))
            {
                WinLoseManager.Instance.TriggerLose();
            }
            if (other.CompareTag("Player"))
            {
                WinLoseManager.Instance.TriggerLose();
            }

            if (other.CompareTag("Shield"))
            {

                Destroy(GameObject.FindWithTag("arrowenem"));
                EnergyManager.Instance.UseEnergy(20);
                // Animasi Bagoyang kena tabrak spaceship.
                other.gameObject.GetComponent<MMF_Player>().PlayFeedbacks();
                ScoreManager.Instance.AddScore(20);
            }
            //Add Explosion Effect
            if (ExplosionEffect != null)
            {
                AudioManager.Instance.PlaySFX(1);
                Destroy(Instantiate(ExplosionEffect, transform.position, Quaternion.identity), 1);
            }

            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // Kembali ke pool menggunakan tag yang ditentukan
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
}