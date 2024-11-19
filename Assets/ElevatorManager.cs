using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using cowsins;
using UnityEngine.UI;


public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private  Image blackScreenImage;
    [SerializeField] private  GameObject blackScreen;
    [SerializeField] private float fadeDuration = 1.5f; 
    // private bool isFading = false;
    
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            PlayerStartPosition.playerSpawnPosition = new Vector3(-0.32f,0.8f, -3f);
            PlayerDataManager.Instance.ResetData(other.gameObject.GetComponent<WeaponController>().inventory.Length);
            PlayerDataManager.Instance.playerHealth = other.gameObject.GetComponent<PlayerStats>().health;
            Debug.Log("Health: " + PlayerDataManager.Instance.playerHealth);
            PlayerDataManager.Instance.currentWeaponIndex = other.gameObject.GetComponent<WeaponController>().currentWeapon;
            int index = 0;
            foreach (WeaponIdentification id in other.gameObject.GetComponent<WeaponController>().inventory){
                // Debug.Log("ELEVATOR: ""Weapon identification: " + id.name);

                // if(id != null){
                //     PlayerDataManager.Instance.inventory[index] = id;
                //     PlayerDataManager.Instance.weapons[index] = id.weapon;

                //     Debug.Log("Weapon identification: " + id.name);
                //     PlayerDataManager.Instance.bulletsLeftInMagazine[index] = id.bulletsLeftInMagazine;
                //     index++;
                // }

                PlayerDataManager.Instance.inventory[index] = id;
                PlayerDataManager.Instance.weapons[index] = id.weapon;

                Debug.Log("Weapon identification: " + id.name);
                PlayerDataManager.Instance.bulletsLeftInMagazine[index] = id.bulletsLeftInMagazine;
                index++;
            


            }
            StartCoroutine(FadeOutAndLoadScene());

            //SceneManager.LoadScene("Level 2.1 Design");
            //Time.timeScale = 1;

        }


    }
     public IEnumerator FadeOutAndLoadScene()
        {
            // isFading = true;
             Debug.Log("call IEnumerator FadeOutAndLoadScene()");
             blackScreen.SetActive(true);
             GameObject player = GameObject.FindGameObjectWithTag("Player");
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            playerRigidbody.isKinematic = true;

            // Time.timeScale = 0;
            float elapsedTime = 0f;
            Color color = blackScreenImage.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                //blackScreenImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
                color.a = alpha; 
                 Debug.Log("change color alpha");
            
                blackScreenImage.color = color;
                yield return null;
            }
             blackScreen.SetActive(false);
             SceneManager.LoadScene("Level 2.1 Design");
            
        }
}
