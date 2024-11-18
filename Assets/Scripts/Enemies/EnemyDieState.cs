using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using cowsins;

public class EnemyDieState : IEnemyState
{
    private bool rigidbodyChanged = false;
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Die State " + stateController.gameObject.name);
        stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
        if(!stateController.GetIsHuman()){
            stateController.GetEnemy().GetRagdollController().ApplyForce(stateController.GetForceDirection(), stateController.GetForce());
        }
        
        stateController.gameObject.GetComponent<EnemyWeaponController>().enabled = false;
        Transform weapon = stateController.gameObject.transform.Find("WeaponHolder");
        Rigidbody rb = weapon.gameObject.AddComponent<Rigidbody>();
        if(rb!=null){
            rb.mass = 1f;
            rb.drag = 0.5f;
            rb.angularDrag = 0.05f;
            rb.useGravity = true;
        }

        if(stateController.GetEnemy().gameObject.GetComponent<Animator>() != null){
            stateController.GetEnemy().gameObject.GetComponent<Animator>().enabled = false;
        }

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

