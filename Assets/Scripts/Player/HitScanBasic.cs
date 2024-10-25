using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBasic : MonoBehaviour
{
    
    public Camera playerCamera;  
    public float range = 100f;   
    public float damage = 10f;  
    public float pushForce = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            
            if (hit.collider.CompareTag("Enemy"))
            {
                
                // EnemyBaseClass enemy = hit.transform.GetComponent<EnemyBaseClass>();
                EnemyStateController enemy = hit.transform.GetComponent<EnemyStateController>();

                if (enemy != null)
                {
                    enemy.GetEnemy().DecreaseHealth(damage);
                    // enemy.DecreaseHealth(damage);
                    
                    Vector3 pushDirection = hit.point - playerCamera.transform.position; 
                    pushDirection = pushDirection.normalized;

                    // enemy.SwitchToRagdollAndApplyForce(pushDirection, pushForce); 
                    enemy.SetForceDirection(pushDirection);
                    enemy.SetForce(pushForce);
                    enemy.ChangeState(new EnemyHitState());

                }
            }    
            
        }
    }

}
