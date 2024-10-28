using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitScanBasic : MonoBehaviour
{
    
    public Camera playerCamera;  
    public float range = 100f;   
    public float damage = 10f;  
    public float chargeDamage = 80f;
    public float pushForce = 100f;
    public float chargedPushForce = 300f;

     public LayerMask pushableLayer;
    
    public Image chargeProgressBar; 
    public float explosionRadius = 5f;
    private float rightClickHoldTime = 0f;  
    private bool isRightClickPressed = false;
    private bool isChargedAttack = false;
    private float chargeTime = 2f; 

   
    // Start is called before the first frame update
    void Start()
    {
        if (chargeProgressBar != null)
        {
            chargeProgressBar.fillAmount = 0;  
        }
     }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }

            if (Input.GetMouseButtonDown(1)) 
                {
                    isRightClickPressed = true;

                    rightClickHoldTime = 0f;  
                    if (chargeProgressBar != null)
                    {
                        chargeProgressBar.fillAmount = 0;
                        chargeProgressBar.gameObject.SetActive(true);  
                    }
                
                }

            if (isRightClickPressed)
            {
                rightClickHoldTime += Time.deltaTime;
                
                if (chargeProgressBar != null)
                {
                    chargeProgressBar.fillAmount = Mathf.Clamp01(rightClickHoldTime / chargeTime);
                }
            }


			if (Input.GetMouseButtonUp(1))
			{
				isRightClickPressed = false;

				if (rightClickHoldTime >= chargeTime)
                {
                    ChargedShoot();
                }
                
                if (chargeProgressBar != null)
                {
                    chargeProgressBar.fillAmount = 0;
                    chargeProgressBar.gameObject.SetActive(false); // 隐藏进度条
                }
                    
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

    void ChargedShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Charged Hit: " + hit.transform.name);

            
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyRagdollController enemyRagdollController = hit.transform.GetComponent<EnemyRagdollController>();
                if (enemyRagdollController != null)
                {
                    
                    enemyRagdollController.SetRagdollActive(true);

                    Vector3 pushDirection = hit.point - playerCamera.transform.position;
                    pushDirection = pushDirection.normalized;

                  
                    enemyRagdollController.ApplyForce(pushDirection, chargedPushForce);

                
                    PushNearbyEnemies(hit.transform.position, chargedPushForce, explosionRadius);
                }
            }
        }
    }


    public void PushNearbyEnemies(Vector3 center, float force, float explosionRadius )
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyRagdollController nearbyEnemyRagdollController = hitCollider.GetComponent<EnemyRagdollController>();
            if (nearbyEnemyRagdollController != null)
            {
               
                nearbyEnemyRagdollController.SetRagdollActive(true);

                Vector3 pushDirection = hitCollider.transform.position - center;
                pushDirection = pushDirection.normalized;

             
                nearbyEnemyRagdollController.ApplyForce(pushDirection, force);
            }
        }   
    }

}
