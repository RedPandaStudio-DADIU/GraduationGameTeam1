using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyStateMachine : MonoBehaviour
{
    public static GlobalEnemyStateMachine Instance { get; private set; }

    private  List<EnemyStateController> enemies = new List<EnemyStateController>();
    private bool playerDetected = false;
    private Vector3 playerPosition;
    private bool isLost = true;
    private float notificationDistance = 5f;


    void Awake()
    {
        // Singleton pattern 
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

    void Start()
    {
        EnemyStateController[] enemiesArray = FindObjectsOfType<EnemyStateController>();

        foreach (EnemyStateController enemy in enemiesArray)
        {
            enemies.Add(enemy);
        }
        Debug.Log("Enemies: " + enemies.Count);

        isLost = true;
    }

    void Update(){

        isLost = true;

        foreach (EnemyStateController enemy in enemies)
        {
            if (enemy != null)
            {
                Debug.Log("Enemy: " + enemy.GetEnemy().name + " Player in range: " + enemy.GetPlayerInRange().ToString());

                if (enemy.GetPlayerInRange()){
                    isLost = false;
                    break;
                } 

            }
        }

        if(isLost){
            LosePlayer();
        }
    }

    //TODO pass enemy who detected the player
    public void DetectPlayer(Vector3 detectedPosition){
        Debug.Log("Inside DetectPlayer");

        if (!playerDetected)
        {
            playerDetected = true;
            playerPosition = detectedPosition;
            NotifyEnemies(true);
        }
    }

    public void LosePlayer(){
        Debug.Log("PLAYER IS LOST!");

        if (playerDetected)
        {
            // Debug.Log("PLAYER IS LOST!");
            playerDetected = false;
            NotifyEnemies(false);
        }
    }

    //TODO pass enemy who detected the player
    public void NotifyEnemies(bool detected){

        foreach (EnemyStateController enemy in enemies)
        {
            if (enemy != null)
            {
                //TODO find the enemies close by to the notifier
                if(detected){
                    Debug.Log("Notifying: " + enemy.GetEnemy().name);
                    enemy.SetPlayerPosition(playerPosition);
                    enemy.ChangeState(new EnemyAttackState());
                } else {
                    // Position unknown 
                    enemy.ChangeState(new EnemyIdleState());

                }

            }
        }

    }


    // Will be useful in the future for when enemies die
    public void RemoveEnemy(EnemyStateController enemy){
        if(enemies.Contains(enemy)){
            enemies.Remove(enemy);
        }
    }

    // Might be useful in the future if we're gonna spawn enemies later on
    public void AddEnemy(EnemyStateController enemy){
        if(!enemies.Contains(enemy)){
            enemies.Add(enemy);
        }
    }

}
