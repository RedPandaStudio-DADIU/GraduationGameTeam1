using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using cowsins;
using AK.Wwise;

public class EnemyDieState : IEnemyState
{
    private bool rigidbodyChanged = false;
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Die State " + stateController.gameObject.name);
        stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
        if(!stateController.GetIsHuman()){
            stateController.GetEnemy().GetRagdollController().ApplyForce(stateController.GetForceDirection(), stateController.GetForce());
        }

        stateController.GetAnimator().SetBool("IsAttacking", false);
        stateController.GetAnimator().SetBool("IsShooting", false);

        if(stateController.GetEnemy().gameObject.GetComponent<Animator>() != null){
            stateController.GetEnemy().gameObject.GetComponent<Animator>().enabled = false;
        }

        if(stateController.GetEnemy().CompareTag("Boss")){
            stateController.GetEnemy().GetComponent<Boss>().PlayHitOrDeadSound(true);
            // stateController.GetEnemy().GetComponent<Boss>().PlayHitOrDeadSound(false);
            stateController.GetMusicManager().CheckIfSameState("Win");
            stateController.StartEndGame();

        } else{
            stateController.GetEnemy().SetSwitchValue("EnemyStatusSwitch", "Dying");

        }

        if(!stateController.GetIsHuman()){
            stateController.gameObject.GetComponent<EnemyHealth>().enabled = false;
            stateController.GetEnemy().GetComponentInChildren<Canvas>().enabled = false;
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


        stateController.GetEnemy().GetComponent<NavMeshAgent>().enabled = false;
        stateController.GetEnemy().tag = "Untagged";
        if(stateController.GetEnemy().GetComponent<Rigidbody>() != null){
            stateController.GetEnemy().GetComponent<EnemyRagdollController>().SetRagdollActive(true);
            stateController.GetEnemy().GetComponent<Rigidbody>().isKinematic = true;
        }



        // if(!stateController.GetIsHuman()){
        //     Transform child = stateController.FindChildByName(stateController.GetEnemy().transform, "HealthSlider");
        //     child.gameObject.SetActive(false);
        // }


        // if(!rigidbodyChanged){
        //     Rigidbody rbc = stateController.GetEnemy().GetComponent<Rigidbody>();
        //     if (rbc != null)
        //     {
        //         rbc.isKinematic = true;
        //     }

        //     // stateController.GetEnemy().gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        //     stateController.GetEnemy().GetComponent<NavMeshAgent>().enabled = false;

        //     stateController.GetEnemy().tag = "Untagged";
        //     rigidbodyChanged = true;
        // }


    }
    public void OnUpdate(EnemyStateController stateController){

        if(stateController.GetEnemy().gameObject.GetComponent<Animator>() != null){
            stateController.GetEnemy().gameObject.GetComponent<Animator>().enabled = false;
        }

       
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Die State");
    }



}

