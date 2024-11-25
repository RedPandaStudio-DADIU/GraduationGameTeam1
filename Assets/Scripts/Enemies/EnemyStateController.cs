using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using cowsins;

public class EnemyStateController : MonoBehaviour
{
    
    private IEnemyState currentState;
    private IEnemyState previousState;
    private EnemyBaseClass enemy;
    private Transform playerTransform;
    private Transform playerBodyTransform;

    private bool playerInRange = false;
    private Vector3 forceDirection;
    [SerializeField] private float force = 300f;
    [SerializeField] private List<Transform> targetList = new List<Transform>();
    [SerializeField] private bool isHuman = false;
    [SerializeField] private Animator animator;
    [SerializeField] private bool inAFight = false;
    private MusicManager musicManager;
    private Queue<Transform> targetQueue;
    private GameObject player;
    // [SerializeField] private bool isInRagdoll = false;
    private bool isInRecovery = false;


    void Awake()
    {
        currentState = new EnemyIdleState();
        currentState.OnEnter(this);
        
        if (targetList.Count > 0){
            targetQueue = new Queue<Transform>(targetList); 
        } else{
            targetQueue = new Queue<Transform>();
        }
        
        player = GameObject.FindWithTag("Player");
        targetQueue.Enqueue(player.transform);
        
        
        // playerTransform = GameObject.FindWithTag("Player").transform;
        // playerBodyTransform =  GameObject.FindWithTag("Player").transform;

        playerTransform = playerBodyTransform = targetQueue.Dequeue();
        Debug.Log("Player Transform (TAREGT transform) is " + playerTransform.position + " name: " + playerTransform.gameObject.name);

        enemy = GetComponent<EnemyBaseClass>();
        Debug.LogWarning("Enemy: " + enemy + " Name: " + enemy.name);
        musicManager = GameObject.Find("GeneralSoundAmbience").GetComponent<MusicManager>();

    }

    void Update()
    {
        currentState.OnUpdate(this);

        Debug.LogWarning("Current state for: " + this.GetEnemy().name + " is: " + currentState);

        if(this.GetEnemy().GetHealth() <= 0 && currentState is not EnemyDieState){
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
            // if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Human") )
            // if (hit.collider.CompareTag("PlayerBody"))
            if(isHuman){
                Debug.LogWarning("Raycast hit: " + hit.collider.tag + "object: " + hit.collider.gameObject.name + " for human: " + this.GetEnemy().name);
            }
            if (hit.collider.CompareTag("Player") || (hit.collider.CompareTag("Human") && !isHuman) || (hit.collider.CompareTag("Enemy")&& isHuman) )
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

    public bool ReachedStoppingDistance(){
        float stopDistance = enemy.GetStoppingDistance();

        Vector3 directionToPlayer = playerBodyTransform.position - enemy.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Debug.Log("Distance to player: " + distanceToPlayer + " enemy: " + this.GetEnemy().name);
        if (distanceToPlayer <= stopDistance){
            return true;
        } 
        return false;
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

    public void PrintAgentDestination(){
        Debug.LogWarning("Destination: " +   enemy.GetComponent<NavMeshAgent>().destination  + "of AI Agent: " + this.gameObject.name);
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
        isInRecovery = true;
        StartCoroutine(RecoverAfterDelay(this, ragdollDuration));
    }

    IEnumerator RecoverAfterDelay(EnemyStateController enemy, float delay)
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
            Debug.Log("Enemy " + enemy.name + " is recovering from ragdoll");
            enemy.GetEnemy().GetComponent<EnemyRagdollController>().RecoverFromRagdoll();
            IEnemyState prevState = enemy.GetPreviousState();
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(2f);
            enemy.ChangeState(new EnemyAttackState());
            

            // if(prevState is EnemyAttackState || prevState is EnemyIdleState){
            //     enemy.ChangeState(prevState);
                
            // } else {
            //     enemy.ChangeState(new EnemyIdleState());
            // }

        } else {
            Debug.Log("Enemy is null!!");
        }
        isInRecovery = false;
    }


    public void LooseLifeFight(){
        forceDirection = -transform.forward; 
        this.GetEnemy().SetHealth(0f); // then in update it will enter DieState
    }

    public void SwitchTarget(){
        // Debug.LogWarning("Switch target, old player transform: "+ playerTransform+ ", name: " + playerTransform.gameObject.name);
        if(targetQueue.Count > 0){
            playerTransform = playerBodyTransform = targetQueue.Dequeue();
            // Debug.LogWarning("Switch target, new player transform: "+ playerTransform+ ", name: " + playerTransform.gameObject.name);
            SetAgentsDestination();
            // this.ChangeState(new EnemyAttackState());

            this.gameObject.GetComponent<EnemyWeaponController>().SetPlayerTransform(playerTransform);

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemy.GetComponent<NavMeshAgent>().angularSpeed);

            if(playerTransform == GameObject.FindWithTag("Player").transform){
                this.inAFight = false;
            }

        }
    }

    public bool GetIsHuman(){
        return this.isHuman;
    }

    public MusicManager GetMusicManager(){
        return this.musicManager;
    }

    public IEnemyState GetCurrentState(){
        return this.currentState;
    }

    public Animator GetAnimator(){
        return this.animator;
    }

    public bool GetInAFight(){
        return this.inAFight;
    }

    public GameObject GetPlayer(){
        return this.player;
    }

    // public bool GetisInRagdoll(){
    //     return this.isInRagdoll;
    // }

    // public void SetisInRagdoll(bool ragdollValue){
    //     this.isInRagdoll = ragdollValue;
    // }

    public bool GetisInRecovery(){
        return this.isInRecovery;
    }

    public void SetisInRecovery(bool recovery){
        this.isInRecovery = recovery;
    }

}

