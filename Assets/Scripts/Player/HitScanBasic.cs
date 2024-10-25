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
    
    public float recoilAmount = 2f;           
    public float recoilRecoverySpeed = 5f;

    private Vector3 originalCameraRotation;   
    private bool isRecoiling = false;         
    private float recoilTimer = 0f; 
    private FirstPersonController playerController;  // 引用 FirstPersonController

    // Start is called before the first frame update
    void Start()
    {
        originalCameraRotation = playerCamera.transform.localEulerAngles;
        playerController = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            Shoot();
        }

         if (isRecoiling)
        {
            recoilTimer += Time.deltaTime * recoilRecoverySpeed;
            playerCamera.transform.localEulerAngles = Vector3.Lerp(playerCamera.transform.localEulerAngles, originalCameraRotation, recoilTimer);

            if (recoilTimer >= 1f)
            {
                isRecoiling = false; 
            }
        }
    }

    void Shoot()
    {
        if (playerController != null)
        {
            playerController.ApplyRecoil(2f);  // 这里的 2f 是后坐力的强度
        }

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
             ApplyRecoil();

            
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
                    destructible.TakeHit();
                }
            }    

            
        }
    }

    void ApplyRecoil()
    {
        
        Vector3 recoilRotation = playerCamera.transform.localEulerAngles;
        recoilRotation.x -= recoilAmount;  

        playerCamera.transform.localEulerAngles = recoilRotation;

       
        isRecoiling = true;
        recoilTimer = 0f;
    }

}
