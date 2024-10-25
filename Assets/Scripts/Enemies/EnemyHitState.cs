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
            stateController.GetEnemy().GetRagdollController().RecordBoneTransforms();


        }
    }
    public void OnUpdate(EnemyStateController stateController){
       
    }
    public void OnExit(EnemyStateController stateController){
        // Get recorded position in the enter function
        // Get the posittion now - during the exit
        // Find the difference 
        // Apply the transformation to the recorded position from the enter function (move it to where you're now) 
        
        // possible problems: Trying to stand up in the place where there is a wall or some other object

        stateController.GetEnemy().GetRagdollController().RecoverFromRagdoll();

        // stateController.GetEnemy().GetRagdollController().RecordBoneTransforms();

        // Debug.Log("Exiting Hit State");
    }


}

