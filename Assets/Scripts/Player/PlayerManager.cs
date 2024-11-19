using UnityEngine;
using UnityEngine.SceneManagement;
using cowsins;

public class PlayerManager : MonoBehaviour
{
    // private bool positionSet = false;
    private bool dataSet = false;

    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }
    void FixedUpdate(){
        if(!dataSet ){
           // Debug.Log("Inside Player Manager: Health: "+ PlayerDataManager.Instance.playerHealth);
            if(PlayerDataManager.Instance.playerHealth > 0){
                GameObject.FindWithTag("Player").GetComponent<PlayerStats>().health = PlayerDataManager.Instance.playerHealth;
                UIEvents.basicHealthUISetUp?.Invoke(PlayerDataManager.Instance.playerHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().shield, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxShield);

            }

            WeaponController wc = GameObject.FindWithTag("Player").GetComponent<WeaponController>();
            if(wc != null && PlayerDataManager.Instance.inventory.Length>0){
                int index = 0;

                foreach(Weapon_SO weapon in PlayerDataManager.Instance.weapons){
                    
                    Debug.LogError("Identification weapon player manager: " + weapon);

                    if (weapon == null)
                    {
                        continue;
                    } 

                    AddWeaponToInventory(wc, index, PlayerDataManager.Instance.bulletsLeftInMagazine[index], weapon.magazineSize, weapon);
                    index++;
                }

                wc.currentWeapon = PlayerDataManager.Instance.currentWeaponIndex;
                dataSet = true;
            }

            
        }
        
    }

    
        private void AddWeaponToInventory(WeaponController weaponController, int slot, int currentBullets, int totalBullets, Weapon_SO weapon)
        {
            var weaponPicked = Instantiate(weapon.weaponObject, weaponController.weaponHolder);
            weaponPicked.transform.localPosition = weapon.weaponObject.transform.localPosition;

            weaponController.inventory[slot] = weaponPicked;

            if (weaponController.currentWeapon == slot)
            {
                weaponController.weapon = weapon;
                // ApplyAttachments(weaponController);
                weaponController.UnHolster(weaponPicked.gameObject, true);
                weaponPicked.gameObject.SetActive(true);
            }
            else
            {
                weaponPicked.gameObject.SetActive(false);
            }

            UpdateWeaponBullets(weaponController.inventory[slot].GetComponent<WeaponIdentification>(), currentBullets, totalBullets, weapon);

            UpdateWeaponUI(weaponController, slot, weapon);
        }

        private void UpdateWeaponBullets(WeaponIdentification weaponIdentification, int currentBullets, int totalBullets, Weapon_SO weapon)    
        {
            weaponIdentification.bulletsLeftInMagazine = currentBullets;
            weaponIdentification.totalBullets = totalBullets;
        }

        private void UpdateWeaponUI(WeaponController weaponController, int slot, Weapon_SO weapon)
        {
            weaponController.slots[slot].weapon = weapon;
            weaponController.slots[slot].GetImage();
        }

}
