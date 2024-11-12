using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Hit State " + stateController.name);
        if(stateController.GetEnemy().GetHealth()==0){
            stateController.ChangeState(new EnemyDieState());
        } else{
            stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
            stateController.GetEnemy().GetRagdollController().ApplyForce(stateController.GetForceDirection(), stateController.GetForce());
        }
    }
    public void OnUpdate(EnemyStateController stateController){
       
    }
    public void OnExit(EnemyStateController stateController){       
        stateController.GetEnemy().GetRagdollController().RecoverFromRagdoll();

    }


}

