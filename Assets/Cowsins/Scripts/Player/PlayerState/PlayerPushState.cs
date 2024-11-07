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

        public PlayerPushState(PlayerStates currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { 
            //     this.playerTransform =  playerTransform;
            //     this.coroutineHandler = coroutineHandler;
            this.playerTransform = currentContext.transform;
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
                pushEnemyComponent.PushEnemiesInRange(); // 调用 PushEnemiesInRange 方法
            }
            else
            {
                Debug.LogWarning("PushEnemy component not found on player.");
            }


        }

        public override void UpdateState()
        {
            
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
