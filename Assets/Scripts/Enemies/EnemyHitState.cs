using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class EnemyHitState : IEnemyState
{
    private bool isDying = false;
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Hit State " + stateController.name);
        isDying = false;
        if(stateController.GetEnemy().GetHealth()<=0){
            Debug.Log("0 HEALTH - into death");
            isDying = true;
            stateController.ChangeState(new EnemyDieState());
        } else{
            stateController.GetEnemy().GetRagdollController().SetRagdollActive(true);
            stateController.GetEnemy().GetRagdollController().ApplyForce(stateController.GetForceDirection(), stateController.GetForce());
        }

        if(stateController.GetEnemy().CompareTag("Boss") && stateController.GetEnemy().GetHealth() == 40f){
            stateController.GetEnemy().GetComponent<Boss>().PlayHitOrDeadSound(false);
        }

        stateController.gameObject.GetComponent<EnemyWeaponController>().enabled = false;
        Transform weapon = stateController.gameObject.transform.Find("WeaponHolder");
        weapon.gameObject.SetActive(false);
    }
    public void OnUpdate(EnemyStateController stateController){
       
    }
    public void OnExit(EnemyStateController stateController){     
        if(stateController.GetEnemy().GetHealth()>0){
            // stateController.GetEnemy().GetRagdollController().RecoverFromRagdoll();
            if(!isDying){
                stateController.gameObject.GetComponent<EnemyWeaponController>().enabled = true;

                Transform weapon = stateController.gameObject.transform.Find("WeaponHolder");
                weapon.gameObject.SetActive(true);
            }
           

        }

    }


}

