using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;



namespace cowsins

{
    public class ChargedShot : MonoBehaviour
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
        [SerializeField] private WeaponController weaponController;

        private float rightClickHoldTime = 0f;  
        private bool isRightClickPressed = false;
        private bool isChargedAttack = false;
        private float chargeTime = 2f; 
        private float ragdollDuration = 5.0f; 


        void Start()
        {
            if (chargeProgressBar != null)
            {
                chargeProgressBar.fillAmount = 0;  
            }
        }


        void Update()
        {

            
            if (Mouse.current.rightButton.IsPressed()) {
                
                if (chargeProgressBar != null)
                {
                    Debug.Log("Logging inside if statement");

                    rightClickHoldTime += Time.deltaTime;
                    
                    if (chargeProgressBar != null)
                    {
                        chargeProgressBar.fillAmount = Mathf.Clamp01(rightClickHoldTime / chargeTime);
                    }
                    
                }
            } else {
                if (rightClickHoldTime >= chargeTime){
                    Shoot();
                }

                rightClickHoldTime = 0f;
                chargeProgressBar.fillAmount = 0;
            }

        }

        public void ChargedShoot()
        {
            UIEvents.onChargedShot?.Invoke(chargeProgressBar.fillAmount);
            // chargeProgressBar.gameObject.SetActive(false);

        }

        private void Shoot(){

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
            {
                // Insert the shooting logic
                Debug.Log("Charged Hit: " + hit.transform.name);
                weaponController.HandleSecondaryHitscanProjectileShot();
                // weaponController.HitscanShot();

                
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyStateController enemy = hit.transform.GetComponent<EnemyStateController>();

                    if (enemy.GetEnemy().GetRagdollController() != null)

                    {
                        enemy.GetEnemy().GetRagdollController().SetRagdollActive(true);

                        Vector3 pushDirection = hit.point - playerCamera.transform.position;
                        pushDirection = pushDirection.normalized;


                        enemy.ChangeState(new EnemyHitState());
                        enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);

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
                EnemyStateController nearbyEnemyStateController = hitCollider.GetComponent<EnemyStateController>();

                if (nearbyEnemyRagdollController != null)
                {
                
                    nearbyEnemyRagdollController.SetRagdollActive(true);

                    Vector3 pushDirection = hitCollider.transform.position - center;
                    pushDirection = pushDirection.normalized;

                    nearbyEnemyStateController.ChangeState(new EnemyHitState());
                
                    nearbyEnemyRagdollController.ApplyForce(pushDirection, force);

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

}
