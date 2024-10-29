using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Die State " + stateController.name);
        stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
    
        // Collider[] colliders = stateController.GetEnemy().GetComponentsInChildren<Collider>();
        // foreach (Collider col in colliders)
        // {
        //     col.enabled = false;
        // }

        Rigidbody rb = stateController.GetEnemy().GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // stateController.GetEnemy().gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        stateController.GetEnemy().GetComponent<NavMeshAgent>().enabled = false;

        stateController.GetEnemy().tag = "Untagged";


    }
    public void OnUpdate(EnemyStateController stateController){
        // Activate Ragdoll + deactivate navmesh agent + shooting collider
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Die State");
    }

}

