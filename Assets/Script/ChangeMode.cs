using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{

    public GameObject shield;
    public GameObject canon;

    public static event Action<bool> onModeStatus;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //toggle game object shiled
            shield.SetActive(!shield.activeInHierarchy);
            onModeStatus.Invoke(shield.activeInHierarchy);
            //toggle game object canon
            canon.SetActive(!shield.activeInHierarchy);
        }
    }
}
