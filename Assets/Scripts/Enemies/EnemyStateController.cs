using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateController : MonoBehaviour
{
    
    private IEnemyState currentState;
    private IEnemyState previousState;
    private EnemyBaseClass enemy;
    private Transform playerTransform;
    private Transform playerBodyTransform;

    private bool playerInRange = false;

    void Start()
    {
        currentState = new EnemyIdleState();
        currentState.OnEnter(this);
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerBodyTransform =  GameObject.Find("Capsule").transform;

        enemy = GetComponent<EnemyBaseClass>();
    }

    void Update()
    {
        currentState.OnUpdate(this);
        previousState = currentState;
    }

    public void ChangeState(IEnemyState newState){
        currentState.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
    }

    public EnemyBaseClass GetEnemy(){
        return this.enemy;
    }

    public Transform GetPlayerTransform(){
        return this.playerTransform;
    }

    public void SetPlayerPosition(Vector3 position){
        this.playerTransform.localPosition = position;
    }


    public void SetAgentsDestination(){
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        navMeshAgent.destination = playerTransform.position;
        navMeshAgent.stoppingDistance = enemy.GetStoppingDistance();
        // navMeshAgent.angularSpeed = 0f;
        Vector3 direction = navMeshAgent.velocity.normalized;


        // Adjust 
        if(direction != Vector3.zero){
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Quaternion targetRotation = Quaternion.LookRotation(playerBodyTransform.position);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime *0f);
        }
    }


    public void SetNavAgent(){
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();

        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }
        SetAgentsDestination();
    }


    public bool CanSeePlayer(){

        // Check if enemy within attacking distance of the player
        float attackDistance = enemy.GetAttackDistance();


        Vector3 directionToPlayer = playerBodyTransform.position - enemy.transform.position;
        // Vector3 directionToPlayer = playerTransform.position - enemy.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > attackDistance){
            playerInRange = false;
            return playerInRange;
        }

        // Check if enemy can see the player - player in the field of view of an enemy
        float fieldOfViewAngle = enemy.GetFieldOfView();

        float angleToPlayer = Vector3.Angle(enemy.transform.forward, directionToPlayer);
        if (angleToPlayer > fieldOfViewAngle){
            playerInRange = false;
            return playerInRange;
        }  

        Debug.DrawRay(enemy.transform.position, directionToPlayer.normalized * distanceToPlayer, Color.red);

        // Check whether there are no obstacles on the way to the player
        if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // GlobalEnemyStateMachine.Instance.DetectPlayer(playerTransform.position);
                GlobalEnemyStateMachine.Instance.DetectPlayer(playerBodyTransform.position);

                playerInRange = true;
                return playerInRange;
            }
        }

        Debug.Log("Raycast check not passed " + enemy.name);

        playerInRange = false;
        return playerInRange;

    }

    public bool GetPlayerInRange(){
        return this.playerInRange;
    }

    public void SetPlayerInRange(bool flag){
        this.playerInRange = flag;
    }

}
