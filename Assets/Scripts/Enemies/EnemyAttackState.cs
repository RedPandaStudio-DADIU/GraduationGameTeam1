using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        // Debug.Log("Entering Attack State");
        stateController.SetNavAgent();
    }
    public void OnUpdate(EnemyStateController stateController){
        stateController.SetAgentsDestination();
        if (stateController.CanSeePlayer())
        {
            stateController.GetEnemy().Attack();
        } 
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Attack State");
    }
}
