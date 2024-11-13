using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private bool positionSet = false;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        Debug.LogWarning("On Scene Loaded for: " + scene.name);
        Debug.Log("Current Position Before Update: " + transform.position);
        
        transform.position = new Vector3(-0.32f, 0.8f, -3f);
        Debug.Log("Position After Update: " + transform.position);
    }
    void FixedUpdate(){
        if(!positionSet && (PlayerStartPosition.playerSpawnPosition != Vector3.zero)){
            transform.position = PlayerStartPosition.playerSpawnPosition;
            positionSet = true;
            // new Vector3(-0.32f, 0.8f, -3f);
        }
    }

}
