using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBasic : MonoBehaviour
{
    
    public Camera playerCamera;  
    public float range = 100f;   
    public float damage = 10f;  
    public float pushForce = 100f;
     

     public LayerMask pushableLayer;
    
   
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
                
                EnemyBaseClass enemy = hit.transform.GetComponent<EnemyBaseClass>();
                if (enemy != null)
                {
                    
                    enemy.DecreaseHealth(damage);

                    
                    Vector3 pushDirection = hit.point - playerCamera.transform.position; 
                    pushDirection = pushDirection.normalized;

                    enemy.SwitchToRagdollAndApplyForce(pushDirection, pushForce); 
                }
            }
            else if (((1 << hit.collider.gameObject.layer) & pushableLayer) != 0)
            {
                Debug.Log("Hit destructible object: " + hit.transform.name);

                DistroyableObstacle destructible = hit.transform.GetComponent<DistroyableObstacle>();
                if (destructible != null)
                {
                    Vector3 hitDirection = hit.transform.position - playerCamera.transform.position;
                    hitDirection = hitDirection.normalized;

                    destructible.TakeHit(hitDirection);
                }
            } 
            else
            {
                Debug.Log("Hit object is not in pushable layer.");
            }   

            
        }
        else
        {
            Debug.Log("No object hit.");
        }

    }

    

}
