using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;


public class EnemyIdleState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Idle State " + stateController.name);
        stateController.GetAnimator().SetBool("IsShooting", false);
        stateController.GetAnimator().SetBool("IsAttacking", false);
        // stateController.GetEnemy().SetSwitchValue("EnemyStatusSwitch", "TakingDamage");
    }
    public void OnUpdate(EnemyStateController stateController){
        Debug.Log("Inside Idle State: " + stateController.GetEnemy().name);

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

