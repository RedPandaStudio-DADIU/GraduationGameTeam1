using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace cowsins
{
    public class PlayerPushState : PlayerBaseState
    {
        private float pushForce = 500f; 
        private float pushRadius = 3f; 
        private float damage = 10f; 

        private float ragdollDuration = 5.0f;  
        private Transform playerTransform;
        private global::PushEnemy pushEnemyComponent;

        //private Animator animator; 
        //private readonly string pushAnimationName = "Kick"; 

        public PlayerPushState(PlayerStates currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { 
            //     this.playerTransform =  playerTransform;
            //     this.coroutineHandler = coroutineHandler;
            this.playerTransform = currentContext.transform;
            //this.animator = currentContext.GetComponent<Animator>();
           // animator = currentContext.GetComponentInChildren<Animator>();
            //animator = playerTransform.GetComponentInChildren<Animator>();
            // Transform legRig = playerTransform.Find("Leg rig (1)");
            // if (legRig != null)
            // {
            //     animator = legRig.GetComponent<Animator>();
            // }
            // Debug.Log($"Animator is {(animator != null ? "found" : "null")}");


            
             }

        public override void EnterState()
        {
            //PushEnemiesInRange();
            Debug.Log("Entered Push State");

             if (pushEnemyComponent == null)
            {
                pushEnemyComponent = playerTransform.GetComponent<PushEnemy>();
            }

            if (pushEnemyComponent != null)
            {
                pushEnemyComponent.PushEnemiesInRange(); 
            }
            else
            {
                Debug.LogWarning("PushEnemy component not found on player.");
            }

            // if (animator != null)
            // {
            //     Debug.Log("turn the trigger");
            //     animator.SetTrigger("kickT");
            // }
            // else
            // {
            //     Debug.LogWarning("Animator not found on player.");
            // }


        }

        public override void UpdateState()
        {
            //PrintAnimatorState();
        //    if (IsAnimationFinished())
        //     {
        //         CheckSwitchState();
        //     } 
            CheckSwitchState();
        }

        public override void FixedUpdateState() { }

        public override void ExitState()
        {
           
            Debug.Log("Exiting Push State");
        }

        public override void CheckSwitchState()
        {
            
            if (!InputManager.push)
            {
                SwitchState(_factory.Default());
            }
        }

        // private bool IsAnimationFinished()
        // {
        //     if (animator == null) return true;
        //     Debug.Log("Exiting animation");

        //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //     //return stateInfo.IsName(pushAnimationName) && stateInfo.normalizedTime >= 1f;
        //     return stateInfo.normalizedTime >= 1f;
        // }

        // private void PrintAnimatorState()
        // {
        //     if (animator == null)
        //     {
        //         Debug.LogError("Animator is null!");
        //         return;
        //     }

        //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 获取第0层（Base Layer）的状态信息
        //     Debug.Log($"Current Animation Name: {stateInfo.fullPathHash}");
        //     Debug.Log($"IsPlayingKick: {stateInfo.IsName("Kick")}");
        //     Debug.Log($"Normalized Time: {stateInfo.normalizedTime}");
        // }

        // private void PushEnemiesInRange()
        // {
        //      Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);

        //     foreach (Collider hitCollider in hitColliders)
        //     {
                
        //         if (hitCollider.CompareTag("Enemy"))
        //         {
                    
        //             // EnemyBaseClass enemy = hitCollider.GetComponent<EnemyBaseClass>();
        //             EnemyStateController enemy = hitCollider.GetComponent<EnemyStateController>();


        //             if (enemy != null)
        //             {
        //                 enemy.GetEnemy().DecreaseHealth(damage);
        //                 // Debug.LogWarning("Enemy health: " + enemy.GetEnemy().GetHealth());

        //                 Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
        //                 Debug.Log("Pushing enemy: " + hitCollider.name + " with direction: " + pushDirection);
                        
        //                 // enemy.SwitchToRagdollAndApplyForce(pushDirection, pushForce);
        //                 enemy.SetForceDirection(pushDirection);
        //                 enemy.SetForce(pushForce);

        //                 enemy.ChangeState(new EnemyHitState());

        //                 if(enemy.GetEnemy().GetHealth() > 0){
        //                     StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
        //                 }
        //             }
        //         }
        //     }
        // }


        // private IEnumerator RecoverAfterDelay(EnemyStateController enemy, float delay)
        // {
        //     yield return new WaitForSeconds(delay);
        //     if (enemy != null)
        //     {
        //         Debug.Log("Enemy " + enemy.name + " is recovering from ragdoll");
        //         IEnemyState prevState = enemy.GetPreviousState();
        //         enemy.ChangeState(prevState);
        //     }
        // }
 

    }
}
