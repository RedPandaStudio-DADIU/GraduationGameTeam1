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
    private float notificationDistance = 10f;


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
    public void DetectPlayer(Vector3 detectedPosition, EnemyBaseClass enemy){
        Debug.Log("Inside DetectPlayer");

        if (!playerDetected)
        {
            playerDetected = true;
            playerPosition = detectedPosition;
            NotifyEnemies(true, enemy);
        }
    }

    public void LosePlayer(){
        Debug.Log("PLAYER IS LOST!");

        if (playerDetected)
        {
            // Debug.Log("PLAYER IS LOST!");
            playerDetected = false;
            NotifyEnemies(false, null);
        }
    }

    //TODO pass enemy who detected the player
    public void NotifyEnemies(bool detected, EnemyBaseClass enemyDetecting){

        foreach (EnemyStateController enemy in enemies)
        {
            if (enemy != null)
            {
                //TODO find the enemies close by to the notifier
                if(detected){
                    if(IsEnemyCloseBy(enemyDetecting, enemy.GetEnemy())){
                        Debug.Log("Notifying: " + enemy.GetEnemy().name);
                        // enemy.SetPlayerPosition(playerPosition);
                        enemy.ChangeState(new EnemyAttackState());
                    }
                    
                } else {
                    // Position unknown 
                    if(enemy.GetEnemy() is not ShieldEnemy) {
                        enemy.GetEnemy().LosePlayer(playerPosition);
                    }
                    // enemy.ChangeState(new EnemyIdleState());

                }

            }
        }

    }

    public bool IsEnemyCloseBy(EnemyBaseClass enemyDetecting, EnemyBaseClass randomEnemy){
        // Get the distance between enemy who detected player and other enemies
        Vector3 direction = enemyDetecting.transform.position - randomEnemy.transform.position;
        float distance = direction.magnitude;

        if (distance > notificationDistance){
           return false;
        } else {
            return true;
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
