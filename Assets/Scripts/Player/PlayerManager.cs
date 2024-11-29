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
            
        }
        
    }
}
