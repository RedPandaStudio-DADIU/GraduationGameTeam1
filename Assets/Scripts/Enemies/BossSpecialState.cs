using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class BossSpecialState : IEnemyState
{
    private Coroutine chargeBeforeShotCoroutine;
    private float chargeTime = 5f;
    private float backToAttackDelay = 3f;

    public void OnEnter(EnemyStateController stateController){
        stateController.GetAnimator().SetBool("IsSpecialAttack", true);
        chargeBeforeShotCoroutine = stateController.StartCoroutine(ChargeBeforeShotCoroutine(stateController));

    }
    public void OnUpdate(EnemyStateController stateController){

        
    }
    public void OnExit(EnemyStateController stateController){
        if (chargeBeforeShotCoroutine != null)
        {
            stateController.StopCoroutine(chargeBeforeShotCoroutine);
        }
        chargeBeforeShotCoroutine = null;
        stateController.GetAnimator().SetBool("IsSpecialAttack", false);


    }

    private IEnumerator ChargeBeforeShotCoroutine(EnemyStateController stateController)
    {
        //while (true)
        //{
            Debug.LogWarning("Entering the ChargeBeforeShot Coroutine: " + Time.time);
            if (stateController.GetEnemy().CompareTag("Boss") && stateController.GetEnemy().GetChargeSound() != null)
            {
                stateController.GetEnemy().GetComponent<Boss>().GetSpecialAttackSound().Post(stateController.GetEnemy().gameObject);
                stateController.GetEnemy().GetComponent<Boss>().InstantiateAttackPrepVFX();
                stateController.GetEnemy().GetChargeSound().Post(stateController.GetEnemy().gameObject);
                // add the instantiation of the vfx
            }
            yield return new WaitForSeconds(chargeTime);

            if (stateController.GetEnemy().CompareTag("Boss"))
            {
                stateController.GetEnemy().SpecialAttack(stateController.GetPlayerTransform());
                yield return new WaitForSeconds(backToAttackDelay);

                stateController.ChangeState(stateController.GetPreviousState());
            }

            stateController.GetEnemy().GetComponent<Boss>().StopAttackPrepVFX();
        //}
    }

}

