using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitScanBasic : MonoBehaviour
{
    
    [SerializeField] private Camera playerCamera;  
    [SerializeField] private float range = 100f;   
    [SerializeField] private float damage = 20f;  
    [SerializeField] private float chargeDamage = 80f;
    [SerializeField] private float pushForce = 100f;
    [SerializeField] private float chargedPushForce = 300f;

    [SerializeField] private LayerMask pushableLayer;
    
    [SerializeField] private Image chargeProgressBar; 
    [SerializeField] private float explosionRadius = 5f;
    private float rightClickHoldTime = 0f;  
    private bool isRightClickPressed = false;
    private bool isChargedAttack = false;
    private float chargeTime = 2f; 
    private float ragdollDuration = 5.0f; 

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event gunshotEvent;
    [SerializeField] private AK.Wwise.Event chargedGunshotEvent;
    [SerializeField] private AK.Wwise.Event chargingEvent;         // Charging sound
    [SerializeField] private AK.Wwise.Event chargeCompleteEvent;   // Charge complete sound
    [SerializeField] private AK.Wwise.Event explosionEvent;   

    // [SerializeField] private string rifleSoundBankName = "MainSoundFXBank";
    //private uint rifleSoundBankID;

    // Start is called before the first frame update
    void Start()
    {
        if (chargeProgressBar != null)
        {
            chargeProgressBar.fillAmount = 0;  
        }
        // AkSoundEngine.LoadBank(rifleSoundBankName, out rifleSoundBankID);
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
                     chargingEvent.Post(gameObject);

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

                if (rightClickHoldTime >= chargeTime && chargeCompleteEvent != null)
                {
                    chargeCompleteEvent.Post(gameObject); // Play charge complete sound
                    isRightClickPressed = false; // Prevent repeated triggering
                }
            }


			if (Input.GetMouseButtonUp(1))
			{
				isRightClickPressed = false;
                 chargingEvent.Stop(gameObject);


				if (rightClickHoldTime >= chargeTime)
                {
                    ChargedShoot();
                    
                }
                
                if (chargeProgressBar != null)
                {
                    chargeProgressBar.fillAmount = 0;
                    chargeProgressBar.gameObject.SetActive(false); 
                }
                    
			}


        
    }

    private void Shoot()
    {
        
        gunshotEvent.Post(gameObject);

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

                    if(enemy.GetEnemy().GetHealth() > 0){
                        StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
                    }

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

    private void ChargedShoot()
    {
        RaycastHit hit;
        chargedGunshotEvent.Post(gameObject);

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Charged Hit: " + hit.transform.name);

            
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyStateController enemy = hit.transform.GetComponent<EnemyStateController>();

                // EnemyRagdollController enemyRagdollController = hit.transform.GetComponent<EnemyRagdollController>();
                // if (enemyRagdollController != null)
                if (enemy.GetEnemy().GetRagdollController() != null)
                {
                    
                    // enemyRagdollController.SetRagdollActive(true);
                    enemy.GetEnemy().GetRagdollController().SetRagdollActive(true);

                    Vector3 pushDirection = hit.point - playerCamera.transform.position;
                    pushDirection = pushDirection.normalized;


                    // // enemy.SetForceDirection(pushDirection);
                    // // enemy.SetForce(pushForce);
                    enemy.ChangeState(new EnemyHitState());

                  
                    // enemyRagdollController.ApplyForce(pushDirection, chargedPushForce);
                    enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);

                    // // if(enemy.GetEnemy().GetHealth() > 0){
                    // //     StartCoroutine(RecoverAfterDelay(enemy, ragdollDuration));
                    // // }

                
                    PushNearbyEnemies(hit.transform.position, chargedPushForce, explosionRadius);
                }
            }
        }
    }


    public void PushNearbyEnemies(Vector3 center, float force, float explosionRadius )
    {
        explosionEvent.Post(gameObject);
        
        Collider[] hitColliders = Physics.OverlapSphere(center, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyRagdollController nearbyEnemyRagdollController = hitCollider.GetComponent<EnemyRagdollController>();
            EnemyStateController nearbyEnemyStateController = hitCollider.GetComponent<EnemyStateController>();
            // EnemyRagdollController nearbyEnemyRagdollController = nearbyEnemyStateController.GetEnemy().GetRagdollController();

            if (nearbyEnemyRagdollController != null)
            {
               
                nearbyEnemyRagdollController.SetRagdollActive(true);

                Vector3 pushDirection = hitCollider.transform.position - center;
                pushDirection = pushDirection.normalized;

                nearbyEnemyStateController.ChangeState(new EnemyHitState());
             
                nearbyEnemyRagdollController.ApplyForce(pushDirection, force);

                // if(nearbyEnemyStateController.GetEnemy().GetHealth() > 0){
                //         StartCoroutine(RecoverAfterDelay(nearbyEnemyStateController, ragdollDuration));
                // }

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
