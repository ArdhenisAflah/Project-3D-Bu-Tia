using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Enemy";
    public float rotationOffset = 0f; // adjust kalau arah panah tidak pas

    private Transform target;
    public int counterTarget = 0;
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
        if (obj != null)
            target = obj.transform;
    }

    void Update()
    {

        if (target == null)
        {
            // Cari lagi kalau target hilang
            GameObject obj = GameObject.FindGameObjectWithTag(targetTag);
            if (obj != null)
            {

                target = obj.transform;
                counterTarget++;
            }
            return;
        }

        // Hitung arah ke target
        Vector3 dir = target.position - transform.position;

        // Konversi ke sudut Z (2D rotation di world space)
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle + rotationOffset, 0f);
    }
}