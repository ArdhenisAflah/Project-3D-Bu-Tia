using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform[] firePoint;
    public float fireRate = 0.5f;

    private float nextFireTime;

    void Update()
    {
        // Mengecek input (Default: Klik Kiri atau Ctrl)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        EnergyManager.Instance.UseEnergy(25);
        Quaternion rotationOffset = Quaternion.Euler(90, 0, 0);


        // Membuat instance peluru pada posisi dan rotasi firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint[0].position, firePoint[0].rotation * rotationOffset);
        GameObject bullet2 = Instantiate(bulletPrefab, firePoint[1].position, firePoint[1].rotation * rotationOffset);

        // Opsional: Logika tambahan seperti sound effect atau muzzle flash bisa ditaruh di sini
    }
}
