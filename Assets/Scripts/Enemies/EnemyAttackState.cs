using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class EnemyAttackState : IEnemyState
{
    private bool isAttacking = false;
    private Coroutine shootingCoroutine;

    public void OnEnter(EnemyStateController stateController){
        // Debug.Log("Entering Attack State");
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetNavAgent();
        }
    }
    public void OnUpdate(EnemyStateController stateController){
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetAgentsDestination();
        }
        if (stateController.CanSeePlayer())
        {
            if(!isAttacking){
                // stateController.GetEnemy().Attack();
                isAttacking = true;
                shootingCoroutine = stateController.StartCoroutine(ContinuousShooting(stateController));
            }
           
        } else {
            // reaches destination - goes into idle state
            if(stateController.CheckIfReachedDestination()){
                stateController.ChangeState(new EnemyIdleState());
            }
        }

        
    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Attack State");
        isAttacking = false;
        if (shootingCoroutine != null)
        {
            stateController.StopCoroutine(shootingCoroutine);
        }
        shootingCoroutine = null;

    }


    private IEnumerator ContinuousShooting(EnemyStateController stateController)
    {
        while (isAttacking)
        {
            if (stateController.GetCurrentState() is EnemyAttackState) // Ensure enemy is still in attack state
            {
                stateController.gameObject.GetComponent<EnemyWeaponController>().HandleHitscanProjectileShot();
            // yield return new WaitForSeconds(stateController.gameObject.GetComponent<EnemyWeaponController>().GetFireRate()*10f);
            } else {
                yield break;
            }
            float delay = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(delay);

        }
    }


}
