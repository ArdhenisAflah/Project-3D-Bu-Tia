using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;


public enum JenisAsteroid
{
    Diamond,
    Gold,
    Silver,
    Rocky
}

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public int totalAsteroid;
    public int asteroidDiamond;
    public int asteroidGold;
    public int asteroidSilver;
    public int asteroidRocky;

    public TextMeshProUGUI diamond;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI silver;
    public TextMeshProUGUI rocky;
    void Awake()
    {
        Instance = this;
    }
    public void AddAsteroid(int amount, JenisAsteroid tipe)
    {
        switch (tipe)
        {
            case JenisAsteroid.Diamond:
                asteroidDiamond += 1;
                UpdateUI(diamond, asteroidDiamond);
                break;
            case JenisAsteroid.Gold:
                asteroidGold += 1;
                UpdateUI(gold, asteroidGold);
                break;
            case JenisAsteroid.Silver:
                asteroidSilver += 1;
                UpdateUI(silver, asteroidSilver);
                break;
            case JenisAsteroid.Rocky:
                asteroidRocky += 1;
                UpdateUI(rocky, asteroidRocky);
                break;
        }
        totalAsteroid += amount;
    }

    // Update is called once per frame
    void UpdateUI(TextMeshProUGUI textInventory, in int amount)
    {
        textInventory.transform.parent.GetComponent<MMF_Player>().PlayFeedbacks();
        textInventory.text = amount.ToString();
    }
}
