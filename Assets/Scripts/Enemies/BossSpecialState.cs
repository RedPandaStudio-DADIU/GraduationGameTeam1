using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class BossSpecialState : IEnemyState
{
    private Coroutine chargeBeforeShotCoroutine;
    private float chargeTime = 3f;
    private float backToAttackDelay = 3f;

    public void OnEnter(EnemyStateController stateController){
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

    }

    private IEnumerator ChargeBeforeShotCoroutine(EnemyStateController stateController)
    {
        while (true)
        {
            if (stateController.GetEnemy().CompareTag("Boss") && stateController.GetEnemy().GetChargeSound() != null)
            {
                stateController.GetEnemy().GetChargeSound().Post(stateController.GetEnemy().gameObject);
            }
            yield return new WaitForSeconds(chargeTime);

            if (stateController.GetEnemy().CompareTag("Boss"))
            {
                stateController.GetEnemy().SpecialAttack(stateController.GetPlayerTransform());
                yield return new WaitForSeconds(backToAttackDelay);

                stateController.ChangeState(stateController.GetPreviousState());
            }
        }
    }

}

