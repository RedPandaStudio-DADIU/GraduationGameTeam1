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

        [SerializeField] private AK.Wwise.Event chargingEvent;
        [SerializeField] private AK.Wwise.Event chargedShotEvent;
        [SerializeField] private AK.Wwise.Event chargeRdEvent;



        private float rightClickHoldTime = 0f;  
        private bool isRightClickPressed = false;
        private bool isChargedAttack = false;
        private bool hasStoppedCharging = false;
        private bool hasPlayedReadySound = false;
        private float chargeTime = 2f; 
        private float ragdollDuration = 5.0f; 
        private float delay = 5f;


        void Start()
        {
            if (chargeProgressBar != null)
            {
                chargeProgressBar.fillAmount = 0;  
            }
        }


        void Update()
        {

            
             if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                chargingEvent.Post(gameObject); 
                 Debug.Log("begain to charge");
                 hasStoppedCharging = false;
                hasPlayedReadySound = false;

            }

            if (Mouse.current.rightButton.IsPressed()) {
                
                 
                if (chargeProgressBar != null)
                {
                    Debug.Log("Logging inside if statement");

                    rightClickHoldTime += Time.deltaTime;
                    
                    if (chargeProgressBar != null)
                    {
                        chargeProgressBar.fillAmount = Mathf.Clamp01(rightClickHoldTime / chargeTime);
                    }
                    if (rightClickHoldTime >= chargeTime &&!hasPlayedReadySound)
                    {
                        hasPlayedReadySound = true;
                        chargeRdEvent.Post(gameObject);
                        Debug.Log("play ready sound");
                    }
                }

                    
            }
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                chargingEvent.Stop(gameObject); 
                Debug.Log("stop to charge");
                if (rightClickHoldTime >= chargeTime){
                    Shoot();
                    chargedShotEvent.Post(gameObject);
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
                        enemy.SetForce(chargedPushForce);
                        enemy.SetForceDirection(pushDirection);

                        enemy.GetEnemy().DecreaseHealth(chargeDamage);

                        enemy.ChangeState(new EnemyHitState());
                        
                        if(enemy.GetEnemy().GetHealth() > 0){
                            // enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);
                            StartCoroutine(RecoverAfterDelay(enemy, delay));
                        }

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
                    nearbyEnemyStateController.SetForce(chargedPushForce);
                    nearbyEnemyStateController.SetForceDirection(pushDirection);

                    nearbyEnemyStateController.GetEnemy().DecreaseHealth(chargeDamage);
                    nearbyEnemyStateController.ChangeState(new EnemyHitState());


                    if(nearbyEnemyStateController.GetEnemy().GetHealth() > 0){
                        StartCoroutine(RecoverAfterDelay(nearbyEnemyStateController, delay));
                        // nearbyEnemyRagdollController.ApplyForce(pushDirection, force);
                    }

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
