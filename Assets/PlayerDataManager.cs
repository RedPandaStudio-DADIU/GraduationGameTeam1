using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public float playerHealth;
    public float maxPlayerHealth;

    public int[] bulletsLeftInMagazine;
    public WeaponIdentification[] inventory;
    public int currentWeaponIndex;

    private void Awake()
    {
        // Singleton pattern for persistent data manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    public void ResetData(int length)
    {
        playerHealth = maxPlayerHealth;
        bulletsLeftInMagazine = new int[length];  
        inventory = new WeaponIdentification[length]; 
        currentWeaponIndex = 0;
    }
}
