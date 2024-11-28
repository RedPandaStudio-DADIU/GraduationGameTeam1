using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class PushEnemy : MonoBehaviour
{
    
    [SerializeField] private float pushForce = 500f; 
    [SerializeField] private float pushRadius = 3f; 
    [SerializeField] private float damage = 10f;  
    [SerializeField] private AK.Wwise.Event kickEvent;  
    [SerializeField] private GameObject doorManager;  

    private float ragdollDuration = 5.0f; 
 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PushEnemiesInRange()
    {
        kickEvent.Post(this.gameObject);
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            
            if (hitCollider.CompareTag("Enemy") && !(hitCollider.gameObject.GetType() == typeof(ShieldEnemy)))
            {
                PushLogic(hitCollider);
               
            } else if(hitCollider.CompareTag("Boss")){
                if(hitCollider.GetComponent<Collider>().gameObject.GetComponent<Boss>().GetAreWeakSpotsDefeated()){
                    PushLogic(hitCollider);
                }
            } else if(hitCollider.CompareTag("Door")){
                // hitCollider.gameObject.GetComponent<Door>().KickTheDoor();
                doorManager.GetComponent<Door>().KickTheDoor();
            }
        }
    }


    private void PushLogic(Collider hitCollider){
        EnemyStateController enemy = hitCollider.GetComponent<EnemyStateController>();

        if (enemy != null)
        {
            enemy.GetEnemy().DecreaseHealth(damage);

            Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
            Debug.Log("Pushing enemy: " + hitCollider.name + " with direction: " + pushDirection);
            
            enemy.SetForceDirection(pushDirection);
            enemy.SetForce(pushForce);
            // enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, pushForce);

            // enemy.ChangeState(new EnemyHitState());

            // if(enemy.GetEnemy().GetHealth() > 0){
            //     StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
            // }

            enemy.SetShouldRagdoll(true);
            if(enemy.GetCurrentState() is not EnemyHitState){
                enemy.ChangeState(new EnemyHitState());
            }


            if(enemy.GetEnemy().GetHealth() > 0){
                Debug.Log("Inside health check");
                if(!enemy.GetisInRecovery()){
                    enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, pushForce);
                    enemy.Recovery(ragdollDuration);
                }
            } else {
                enemy.ChangeState(new EnemyDieState());
                Debug.Log("Health check not passed");
            }

        }
    }

    // IEnumerator RecoverAfterDelay(EnemyStateController enemy, float delay)
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
