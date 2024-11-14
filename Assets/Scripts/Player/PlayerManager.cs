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
    void Start(){
        // if(!positionSet && (PlayerStartPosition.playerSpawnPosition != Vector3.zero)){
        //     transform.position = PlayerStartPosition.playerSpawnPosition;
        //     positionSet = true;
        //     // new Vector3(-0.32f, 0.8f, -3f);
        // }
        if(!dataSet &&  SceneManager.GetActiveScene().buildIndex==1){
            Debug.Log("Inside Player Manager: Health: "+ PlayerDataManager.Instance.playerHealth);
            if(PlayerDataManager.Instance.playerHealth > 0){
                GameObject.FindWithTag("Player").GetComponent<PlayerStats>().health = PlayerDataManager.Instance.playerHealth;
                UIEvents.basicHealthUISetUp?.Invoke(PlayerDataManager.Instance.playerHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().shield, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxHealth, GameObject.FindWithTag("Player").GetComponent<PlayerStats>().maxShield);

            }

            GameObject.FindWithTag("Player").GetComponent<WeaponController>().currentWeapon = PlayerDataManager.Instance.currentWeaponIndex;
            GameObject.FindWithTag("Player").GetComponent<WeaponController>().initialWeapons = new Weapon_SO[PlayerDataManager.Instance.inventory.Length];
            int index = 0;

            foreach(WeaponIdentification id in PlayerDataManager.Instance.inventory){
                
                // Debug.LogError("Identification: " + id + " weapon: "+id.weapon);

                if (id.weapon == null)
                {
                    continue;
                } 
                // GameObject.FindWithTag("Player").GetComponent<WeaponController>().InstantiateWeapon(id.weapon, index, PlayerDataManager.Instance.bulletsLeftInMagazine[index], id.totalBullets);
                GameObject.FindWithTag("Player").GetComponent<WeaponController>().initialWeapons[index] = id.weapon;

                index++;
            }

            GameObject.FindWithTag("Player").GetComponent<WeaponController>().GetInitialWeapons();

            // GameObject.FindWithTag("Player").GetComponent<WeaponController>().CreateInventoryUI();

            dataSet = true;
        }
        
    }


}
