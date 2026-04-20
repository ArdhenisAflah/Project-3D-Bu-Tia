using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceStation : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("Enemy"))
        {
            WinLoseManager.Instance.TriggerLose();
        }
    }
}
