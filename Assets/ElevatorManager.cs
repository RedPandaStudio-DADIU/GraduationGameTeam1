using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorManager : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            PlayerStartPosition.playerSpawnPosition = new Vector3(-0.32f,0.8f, -3f);
            // other.gameObject.transform.position = new Vector3(-0.32f, 0.8f, -3f);
            SceneManager.LoadScene("Level 2.1 Design");
        }
    }
}
