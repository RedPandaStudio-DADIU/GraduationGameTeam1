using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDieState : IEnemyState
{
    private bool rigidbodyChanged = false;
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Die State " + stateController.name);
        stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
        stateController.GetEnemy().GetRagdollController().ApplyForce(stateController.GetForceDirection(), stateController.GetForce());

        // Collider[] colliders = stateController.GetEnemy().GetComponentsInChildren<Collider>();
        // foreach (Collider col in colliders)
        // {
        //     col.enabled = false;
        // }


    }
    public void OnUpdate(EnemyStateController stateController){
        // Activate Ragdoll + deactivate navmesh agent + shooting collider

        if(!rigidbodyChanged){
             Rigidbody rb = stateController.GetEnemy().GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // stateController.GetEnemy().gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            stateController.GetEnemy().GetComponent<NavMeshAgent>().enabled = false;

            stateController.GetEnemy().tag = "Untagged";
            rigidbodyChanged = true;
        }

       
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Die State");
    }

}

