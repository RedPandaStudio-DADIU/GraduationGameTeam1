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
        // Debug.LogWarning("On Scene Loaded for: " + scene.name);
        // Debug.Log("Current Position Before Update: " + transform.position);
        
        // transform.position = new Vector3(-0.32f, 0.8f, -3f);
        // Debug.Log("Position After Update: " + transform.position);
    }
    void FixedUpdate(){
        // if(!positionSet && (PlayerStartPosition.playerSpawnPosition != Vector3.zero)){
        //     transform.position = PlayerStartPosition.playerSpawnPosition;
        //     positionSet = true;
        //     // new Vector3(-0.32f, 0.8f, -3f);
        // }
        // &&  SceneManager.GetActiveScene().buildIndex==1
        if(!dataSet ){
            Debug.Log("Inside Player Manager: Health: "+ PlayerDataManager.Instance.playerHealth);
            if(PlayerDataManager.Instance.playerHealth > 0){
                GameObject.FindWithTag("Player").GetComponent<PlayerStats>().health = PlayerDataManager.Instance.playerHealth;
                UIEvents.basicHealthUISetUp?.Invoke(PlayerDataManager.Instance.playerHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().shield, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxShield);

            }

            WeaponController wc = GameObject.FindWithTag("Player").GetComponent<WeaponController>();
            // GameObject.FindWithTag("Player").GetComponent<WeaponController>().currentWeapon = PlayerDataManager.Instance.currentWeaponIndex;
            // // GameObject.FindWithTag("Player").GetComponent<WeaponController>().initialWeapons = new Weapon_SO[PlayerDataManager.Instance.inventory.Length];
            if(wc != null && PlayerDataManager.Instance.inventory.Length>0){
                // wc.inventory = new WeaponIdentification[PlayerDataManager.Instance.inventory.Length];
                int index = 0;

                foreach(Weapon_SO weapon in PlayerDataManager.Instance.weapons){
                    
                    // Debug.LogError("Identification: " + id + " weapon: "+id.weapon);

                    if (weapon == null)
                    {
                        continue;
                    } 
                    // // GameObject.FindWithTag("Player").GetComponent<WeaponController>().InstantiateWeapon(id.weapon, index, PlayerDataManager.Instance.bulletsLeftInMagazine[index], id.totalBullets);
                    // GameObject.FindWithTag("Player").GetComponent<WeaponController>().initialWeapons[index] = id.weapon;

                    AddWeaponToInventory(wc, index, PlayerDataManager.Instance.bulletsLeftInMagazine[index], weapon.magazineSize, weapon);
                    index++;
                }

                // GameObject.FindWithTag("Player").GetComponent<WeaponController>().GetInitialWeapons();

                // GameObject.FindWithTag("Player").GetComponent<WeaponController>().CreateInventoryUI();
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
