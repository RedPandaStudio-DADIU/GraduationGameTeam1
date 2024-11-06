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
    private Vector3 forceDirection;
    private float force;

    void Start()
    {
        currentState = new EnemyIdleState();
        currentState.OnEnter(this);
        playerTransform = GameObject.FindWithTag("Player").transform;
        // playerBodyTransform =  GameObject.Find("Capsule").transform;
        playerBodyTransform =  GameObject.FindWithTag("Player").transform;

        enemy = GetComponent<EnemyBaseClass>();
    }

    void Update()
    {
        currentState.OnUpdate(this);

        if(this.GetEnemy().GetHealth() == 0){
            ChangeState(new EnemyDieState());
        }
    }

    public void ChangeState(IEnemyState newState){
        previousState = currentState;
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
        // navMeshAgent.destination = playerTransform.position;
        navMeshAgent.destination = playerBodyTransform.position;
        navMeshAgent.stoppingDistance = enemy.GetStoppingDistance();
        Debug.Log("Enemy: " + enemy.name + ", stopping distance: "+ enemy.GetStoppingDistance());
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

        // Debug.Log("Inside CanSeePlayer");
        // Debug.Log("Player Body Position: " + playerBodyTransform.position);

        // Check if enemy within attacking distance of the player
        float attackDistance = enemy.GetAttackDistance();


        Vector3 directionToPlayer = playerBodyTransform.position - enemy.transform.position;
        // Vector3 directionToPlayer = playerTransform.position - enemy.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        // Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer > attackDistance){
            playerInRange = false;
            return playerInRange;
        }
        // Debug.Log("Distance check passed by: " + enemy.name);


        // Check if enemy can see the player - player in the field of view of an enemy
        float fieldOfViewAngle = enemy.GetFieldOfView();

        float angleToPlayer = Vector3.Angle(enemy.transform.forward, directionToPlayer);
        if (angleToPlayer > fieldOfViewAngle){
            playerInRange = false;
            return playerInRange;
        }  

        // Debug.Log("Angle check passed by: " + enemy.name);


        Debug.DrawRay(enemy.transform.position, directionToPlayer.normalized * distanceToPlayer, Color.red);

        // Check whether there are no obstacles on the way to the player
        if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.CompareTag("Player"))
            // if (hit.collider.CompareTag("PlayerBody"))
            {
                // GlobalEnemyStateMachine.Instance.DetectPlayer(playerTransform.position);
                GlobalEnemyStateMachine.Instance.DetectPlayer(playerBodyTransform.position, this.GetEnemy());

                playerInRange = true;
                // Debug.Log("Raycast check passed by: " + enemy.name);

                return playerInRange;
            }
        }

        // Debug.Log("Raycast check not passed " + enemy.name);

        playerInRange = false;
        return playerInRange;

    }

    public bool CheckIfReachedDestination(){
        if (!enemy.GetComponent<NavMeshAgent>().pathPending){
            if (enemy.GetComponent<NavMeshAgent>().remainingDistance<= enemy.GetComponent<NavMeshAgent>().stoppingDistance){
                if (!enemy.GetComponent<NavMeshAgent>().hasPath || enemy.GetComponent<NavMeshAgent>().velocity.sqrMagnitude == 0f){
                    return true;
                }
            }
        }

        return false;
    }

    public bool GetPlayerInRange(){
        return this.playerInRange;
    }

    public void SetPlayerInRange(bool flag){
        this.playerInRange = flag;
    }
    
    public float GetForce(){
        return this.force;
    }

    public void SetForce(float force){
        this.force = force;
    }

    public Vector3 GetForceDirection(){
        return this.forceDirection;
    }

    public void SetForceDirection(Vector3 forceDirection){
        this.forceDirection = forceDirection;
    }

    public IEnemyState GetPreviousState(){
        return this.previousState;
    }

    public void Recovery(float ragdollDuration){
        StartCoroutine(RecoverAfterDelay(this, ragdollDuration));
    }

    IEnumerator RecoverAfterDelay(EnemyStateController enemy, float delay)
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
            Debug.Log("Enemy " + enemy.name + " is recovering from ragdoll");
            IEnemyState prevState = enemy.GetPreviousState();
            if(prevState is EnemyAttackState || prevState is EnemyIdleState){
                enemy.ChangeState(prevState);
            }

        } else {
            Debug.Log("Enemy is null!!");
        }
    }


}
