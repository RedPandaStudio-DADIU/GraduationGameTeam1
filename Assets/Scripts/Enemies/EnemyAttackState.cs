using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using AK.Wwise;
using UnityEngine.SceneManagement;

public class EnemyAttackState : IEnemyState
{
    private bool isAttacking = false;
    private Coroutine shootingCoroutine;
    private float specialStateIntervals = 5f;
    private bool isSpecialStateActive = false;
    private Coroutine specialAttackCoroutine;
    private int sceneNumber = 1;

    public void OnEnter(EnemyStateController stateController){
        // Debug.Log("Entering Attack State");
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetNavAgent();
        }
        if(stateController.GetEnemy().CompareTag("Boss")){

            specialAttackCoroutine = stateController.StartCoroutine(SpecialAttackCoroutine(stateController));
        }

        stateController.GetAnimator().SetBool("IsAttacking", true);
        if(!stateController.GetIsHuman() && !stateController.GetInAFight() && sceneNumber==1){
            if(stateController.GetPlayer().GetComponent<PlayerStats>().health <= (stateController.GetPlayer().GetComponent<PlayerStats>().maxHealth/2)){
                stateController.GetMusicManager().CheckIfSameState("CombatIntense");
            } else {
                stateController.GetMusicManager().CheckIfSameState("Combat");
            }
        }

        if(stateController.GetEnemy().CompareTag("Boss") && !stateController.GetEnemy().GetComponent<Boss>().GetSaidFirstLine()){
            stateController.GetEnemy().GetComponent<Boss>().SetSaidFirstLine();
            stateController.GetEnemy().GetComponent<Boss>().PostInitialEvent();
        } else{
            stateController.GetEnemy().SetSwitchValue("EnemyStatusSwitch", "Attacking");
        }


        
        // if(!stateController.GetIsHuman()){
        //     Transform child = stateController.FindChildByName(stateController.GetEnemy().transform, "HealthSlider");
        //     child.gameObject.SetActive(true);
        // }

    }
    public void OnUpdate(EnemyStateController stateController){
        Debug.Log("Inside Attack State: " + stateController.GetEnemy().name);
        if(stateController.GetEnemy().GetIsMovable()){
            stateController.SetAgentsDestination();
        }
        if (stateController.CanSeePlayer())
        {
            if(!isAttacking){
                // stateController.GetEnemy().Attack();
                // stateController.GetAnimator().SetBool("IsAttacking", false);

                isAttacking = true;
                shootingCoroutine = stateController.StartCoroutine(ContinuousShooting(stateController));
            }

            if (stateController.ReachedStoppingDistance()){
                stateController.GetAnimator().SetBool("IsShooting", true);
                // stateController.GetAnimator().SetBool("IsAttacking", false);

            } else {
                stateController.GetAnimator().SetBool("IsAttacking", true);

            }
           
        } 


        if(!stateController.GetIsHuman() && !stateController.GetInAFight() && stateController.GetMusicManager().GetCurrentState()!="Control tower" && sceneNumber==1){
            if(stateController.GetPlayer().GetComponent<PlayerStats>().health <= (stateController.GetPlayer().GetComponent<PlayerStats>().maxHealth/2)){
                stateController.GetMusicManager().CheckIfSameState("CombatIntense");
            } else {
                stateController.GetMusicManager().CheckIfSameState("Combat");
            }
        }

        // if(!stateController.GetIsHuman()){
        //     Transform child = stateController.FindChildByName(stateController.GetEnemy().transform, "HealthSlider");
        //     child.gameObject.SetActive(true);
        // }
        // else {
        //     // reaches destination - goes into idle state
        //     if(stateController.CheckIfReachedDestination()){
        //         stateController.ChangeState(new EnemyIdleState());
        //     }
        // }

        // stateController.PrintAgentDestination();

    }
    public void OnExit(EnemyStateController stateController){
        // Debug.Log("Exiting Attack State");
        isAttacking = false;
        if (shootingCoroutine != null)
        {
            stateController.StopCoroutine(shootingCoroutine);
        }
        
        shootingCoroutine = null;

        if(stateController.GetEnemy().CompareTag("Boss")){

            if (specialAttackCoroutine != null)
            {
                stateController.StopCoroutine(specialAttackCoroutine);
            }
            specialAttackCoroutine = null;
        }
        // stateController.GetAnimator().SetBool("IsAttacking", false);
        // stateController.GetAnimator().SetBool("IsShooting", false);

        // isSpecialStateActive = false;

        // if(!stateController.GetIsHuman()){
        //     Transform child = stateController.FindChildByName(stateController.GetEnemy().transform, "HealthSlider");
        //     child.gameObject.SetActive(false);
        // }

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

    private IEnumerator SpecialAttackCoroutine(EnemyStateController stateController)
    {
        while (true)
        {
            yield return new WaitForSeconds(specialStateIntervals);

            // if (!isSpecialStateActive)
            // {
            stateController.ChangeState(new BossSpecialState());
            // }
        }
    }

}
