using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BulletProjectile : MonoBehaviour
{
    public float speed = 100f;
    public float lifetime = 3f;

    private Rigidbody rbdObject;


    // Start is called before the first frame update
    void Start()
    {
        rbdObject = GetComponent<Rigidbody>();
        rbdObject.velocity = transform.up * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(this.gameObject);
        }
    }
}
