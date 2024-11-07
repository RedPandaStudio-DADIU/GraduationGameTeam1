using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private bool isAttacking = false;
    public void OnEnter(EnemyStateController stateController){
        // Debug.Log("Entering Attack State");
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetNavAgent();
        }
    }
    public void OnUpdate(EnemyStateController stateController){
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetAgentsDestination();
        }
        if (stateController.CanSeePlayer())
        {
            if(!isAttacking){
                stateController.GetEnemy().Attack();
                isAttacking = true;
            }
           
        } else {
            // reaches destination - goes into idle state
            if(stateController.CheckIfReachedDestination()){
                stateController.ChangeState(new EnemyIdleState());
            }
        }

        
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Attack State");
        isAttacking = false;
    }
}
