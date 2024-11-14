using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushEnemy : MonoBehaviour
{
    
    [SerializeField] private float pushForce = 500f; 
    [SerializeField] private float pushRadius = 3f; 
    [SerializeField] private float damage = 10f;  

    private float ragdollDuration = 5.0f; 
 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     PushEnemiesInRange();
        // }
    }

    public void PushEnemiesInRange()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            
            if (hitCollider.CompareTag("Enemy"))
            {
                PushLogic(hitCollider);
               
            } else if(hitCollider.CompareTag("Boss")){
                if(hitCollider.GetComponent<Collider>().gameObject.GetComponent<Boss>().GetAreWeakSpotsDefeated()){
                    PushLogic(hitCollider);
                }
            }
        }
    }


    private void PushLogic(Collider hitCollider){
        EnemyStateController enemy = hitCollider.GetComponent<EnemyStateController>();

        if (enemy != null)
        {
            enemy.GetEnemy().DecreaseHealth(damage);
            // Debug.LogWarning("Enemy health: " + enemy.GetEnemy().GetHealth());

            Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
            Debug.Log("Pushing enemy: " + hitCollider.name + " with direction: " + pushDirection);
            
            // enemy.SwitchToRagdollAndApplyForce(pushDirection, pushForce);
            enemy.SetForceDirection(pushDirection);
            enemy.SetForce(pushForce);

            enemy.ChangeState(new EnemyHitState());

            if(enemy.GetEnemy().GetHealth() > 0){
                StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
            }
        }
    }

    IEnumerator RecoverAfterDelay(EnemyStateController enemy, float delay)
    {
        
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
            Debug.Log("Enemy " + enemy.name + " is recovering from ragdoll");
            IEnemyState prevState = enemy.GetPreviousState();
            enemy.ChangeState(prevState);
        }
    }

}
