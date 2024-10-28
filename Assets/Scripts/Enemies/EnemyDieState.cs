using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Die State " + stateController.name);
        stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
    }
    public void OnUpdate(EnemyStateController stateController){
        // Activate Ragdoll + deactivate navmesh agent + shooting collider
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Die State");
    }

}

