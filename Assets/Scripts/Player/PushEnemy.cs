using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class PushEnemy : MonoBehaviour
{
    
    [SerializeField] private float pushForce = 500f; 
    [SerializeField] private float pushRadius = 3f; 
    [SerializeField] private float damage = 10f;  
    //[SerializeField] private GameObject legRig;
     private Animator animator; 
     private WeaponController weaponController;

     private bool isLegRigInitialized = false;
    [SerializeField] private string pushAnimationTrigger = "kickT"; 


    private float ragdollDuration = 5.0f; 
 

    
    // Start is called before the first frame update
    void Start()
    {
         weaponController = GetComponent<WeaponController>();

          Debug.Log("Waiting for weapon initialization to find Leg rig...");


        //  GameObject legRig = GameObject.FindWithTag("kick");

        //     if (legRig != null)
        //     {
        //         animator = legRig.GetComponent<Animator>();
        //         if (animator == null)
        //         {
        //             Debug.LogWarning("Animator component not found on the object with tag 'kick'.");
        //         }
        //         else
        //         {
        //             Debug.Log("Animator successfully found and assigned via Tag.");
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogWarning("No object found with the tag 'kick'.");
        //     }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     PushEnemiesInRange();
        // }
        if (isLegRigInitialized) return;

        if (weaponController != null && weaponController.weapon != null)
        {
            
            GameObject legRig = GameObject.FindWithTag("kick");

            if (legRig != null)
            {
                animator = legRig.GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogWarning("Animator component not found on the object with tag 'kick'.");
                }
                else
                {
                    Debug.Log("Animator successfully found and assigned via Tag.");
                    isLegRigInitialized = true; 
                }
            }
            else
            {
                Debug.LogWarning("No object found with the tag 'kick'.");
            }
        }


    }

    public void PushEnemiesInRange()
    {   
         if (!PlayPushAnimation())
        {
            Debug.LogWarning("Cannot play push animation: Animator not initialized.");
        }
        PlayPushAnimation();
        
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

    private bool PlayPushAnimation()
    {
        // 检查 animator 是否已初始化
        if (animator == null)
        {
            Debug.LogWarning("Animator not yet initialized. Cannot play animation.");
            return false;
        }

        // 播放动画
        animator.SetTrigger("kickT");
        Debug.Log("Push animation triggered.");
        return true;
    }

}
