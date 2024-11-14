using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cowsins;

public class ElevatorManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            PlayerStartPosition.playerSpawnPosition = new Vector3(-0.32f,0.8f, -3f);
            // other.gameObject.transform.position = new Vector3(-0.32f, 0.8f, -3f);
            // other.gameObject.GetComponent<WeaponController>().enabled = false;
            PlayerDataManager.Instance.ResetData(other.gameObject.GetComponent<WeaponController>().inventory.Length);
            PlayerDataManager.Instance.playerHealth = other.gameObject.GetComponent<PlayerStats>().health;
            Debug.Log("Health: " + PlayerDataManager.Instance.playerHealth);
            // PlayerDataManager.Instance.inventory = other.gameObject.GetComponent<WeaponController>().inventory;
            PlayerDataManager.Instance.currentWeaponIndex = other.gameObject.GetComponent<WeaponController>().currentWeapon;
            int index = 0;
            foreach (WeaponIdentification id in other.gameObject.GetComponent<WeaponController>().inventory){
                // Debug.Log("ELEVATOR: ""Weapon identification: " + id.name);

                if(id != null){
                    PlayerDataManager.Instance.inventory[index] = id;
                    Debug.Log("Weapon identification: " + id.name);
                    PlayerDataManager.Instance.bulletsLeftInMagazine[index] = id.bulletsLeftInMagazine;
                    index++;
                }

            }

            SceneManager.LoadScene("Level 2.1 Design");

        }
    }
}
