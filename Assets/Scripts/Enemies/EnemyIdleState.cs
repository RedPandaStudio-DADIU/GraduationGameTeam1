using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Idle State " + stateController.name);
    }
    public void OnUpdate(EnemyStateController stateController){
        if (stateController.CanSeePlayer())
        {
            // Debug.Log("Can See Player");
            // Transition to Attack State
            stateController.ChangeState(new EnemyAttackState());
        }
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Idle State");
    }


}

