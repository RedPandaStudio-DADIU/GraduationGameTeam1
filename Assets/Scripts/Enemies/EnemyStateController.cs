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

    void Start()
    {
        currentState = new EnemyIdleState();
        currentState.OnEnter(this);
        playerTransform = GameObject.FindWithTag("Player").transform;
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

    public void SetAgentsDestination(){
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        navMeshAgent.destination = playerTransform.position;
    }

    public void SetNavAgent(){
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();

        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }
        SetAgentsDestination();
    }
}
