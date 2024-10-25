using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushEnemy : MonoBehaviour
{
    
    public float pushForce = 500f; 
    public float pushRadius = 3f; 

    private float ragdollDuration = 5.0f; 
 

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PushEnemiesInRange();
        }
    }

    void PushEnemiesInRange()
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            
            if (hitCollider.CompareTag("Enemy"))
            {
                
                EnemyBaseClass enemy = hitCollider.GetComponent<EnemyBaseClass>();

                if (enemy != null)
                {
                    
                    Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                    Debug.Log("Pushing enemy: " + hitCollider.name + " with direction: " + pushDirection);
                    
                    enemy.SwitchToRagdollAndApplyForce(pushDirection, pushForce);

                    StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
                }
            }
        }
    }
    IEnumerator RecoverAfterDelay(EnemyBaseClass enemy, float delay)
    {
        
        yield return new WaitForSeconds(delay);

        if (enemy != null)
        {
        enemy.RecoverFromRagdoll();
        Debug.Log("Enemy " + enemy.name + " is recovering from ragdoll");
         }
    }

}