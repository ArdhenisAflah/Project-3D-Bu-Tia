using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShatter : MonoBehaviour
{
    [SerializeField] private float shatterForce = 5f;
    [SerializeField] private float shatterTorque = 3f;

    [SerializeField] private float shrinkDuration = 1f; // durasi shrink
    [SerializeField] private float fragmentLifetime = 2f; // delay sebelum shrink
    private Collider mainCollider;

    void Awake()
    {

        mainCollider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        // Hide parent
        mainCollider.enabled = false;

        // Activate each fracture child
        foreach (Transform child in transform)
        {
            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (mr != null) mr.enabled = true;

            if (rb != null)
            {
                rb.isKinematic = false; // enable physics
                child.GetComponent<FragmentShrink>().StartShrink(fragmentLifetime, shrinkDuration, this.gameObject.transform);
                // Detach so they fly freely
                child.SetParent(null);

                // Random explosion direction
                Vector3 randomDir = (child.position - transform.position + Random.insideUnitSphere).normalized;
                rb.AddForce(randomDir * shatterForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * shatterTorque, ForceMode.Impulse);


            }
        }
    }
}