
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

         private int popID=5; 
    private bool showed = false;


        void Start()
        {
            if (chargeProgressBar != null)
            {
                chargeProgressBar.fillAmount = 0;  
            }
             PopupManager popupManager = FindObjectOfType<PopupManager>();
             
        }


        void Update()
        {

            
            if (Mouse.current.rightButton.wasPressedThisFrame && this.gameObject.GetComponent<WeaponController>().weapon != null && this.gameObject.GetComponent<WeaponController>().id.bulletsLeftInMagazine > this.gameObject.GetComponent<WeaponController>().weapon.ammoCostPerFire2)
            {
                chargingEvent.Post(gameObject); 
                Debug.Log("begain to charge");
                hasStoppedCharging = false;
                hasPlayedReadySound = false;
                this.gameObject.GetComponent<WeaponController>().canShoot = false;

            }

            if (Mouse.current.rightButton.IsPressed() && this.gameObject.GetComponent<WeaponController>().weapon != null && this.gameObject.GetComponent<WeaponController>().id.bulletsLeftInMagazine > this.gameObject.GetComponent<WeaponController>().weapon.ammoCostPerFire2) {
                this.gameObject.GetComponent<WeaponController>().canShoot = false;

                 
                if (chargeProgressBar != null)
                {
                    // Debug.Log("Logging inside if statement");

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

                    PopupManager popupManager = FindObjectOfType<PopupManager>();
       

                    if (popupManager ==null ) 
                    {
                        
                        Debug.Log("no pop manager.");
                    }
                    else
                    {
                        if (popupManager.GetCurrentpopUpID()== popID&& !showed)
                        {
                            popupManager.CompletepopUp();
                            showed = true;
                            Debug.Log("Taskpop " + popID + " completed.");
                        }
                    }   
                }

                rightClickHoldTime = 0f;
                chargeProgressBar.fillAmount = 0;
                this.gameObject.GetComponent<WeaponController>().canShoot = true;


            }

        }

        public void ChargedShoot()
        {
            UIEvents.onChargedShot?.Invoke(chargeProgressBar.fillAmount);
            // chargeProgressBar.gameObject.SetActive(false);

        }

        private void Shoot(){
            weaponController.HandleSecondaryHitscanProjectileShot();

        }

                
                
        public void ShootCharge(GameObject enemyObject, Vector3 projectilePosition){
            
            if (enemyObject.CompareTag("Enemy"))
            {
                HandleShotLogic(enemyObject, projectilePosition);
            }
            else if(enemyObject.CompareTag("Boss")){
                if(enemyObject.GetComponent<Boss>().GetAreWeakSpotsDefeated()){
                    HandleShotLogic(enemyObject, projectilePosition);
                }
            }
        }

        private void HandleShotLogic(GameObject enemyObject, Vector3 projectilePosition){

            EnemyStateController enemy = enemyObject.transform.GetComponent<EnemyStateController>();

            if (enemy.GetEnemy().GetRagdollController() != null)

            {
                enemy.GetEnemy().GetRagdollController().SetRagdollActive(true);

                // Vector3 pushDirection = hit.point - playerCamera.transform.position;
                Vector3 pushDirection = -enemy.transform.forward;

                pushDirection = pushDirection.normalized;
                enemy.SetForce(chargedPushForce);
                enemy.SetForceDirection(pushDirection);

                enemy.GetEnemy().DecreaseHealth(chargeDamage);

                enemy.SetShouldRagdoll(true);
                if(enemy.GetCurrentState() is not EnemyHitState){
                    enemy.ChangeState(new EnemyHitState());
                }

                // enemy.ChangeState(new EnemyHitState());
                
                // if(enemy.GetEnemy().GetHealth() > 0){
                //     enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);
                //     StartCoroutine(RecoverAfterDelay(enemy, delay));
                // }

                if(enemy.GetEnemy().GetHealth() > 0){
                    Debug.Log("Inside health check");
                    if(!enemy.GetisInRecovery()){
                        enemy.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);
                        enemy.Recovery(ragdollDuration);
                    }
                } else {
                    enemy.ChangeState(new EnemyDieState());
                    Debug.Log("Health check not passed");
                }


                PushNearbyEnemies(projectilePosition, chargedPushForce, explosionRadius);
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
                    // nearbyEnemyStateController.ChangeState(new EnemyHitState());


                    // if(nearbyEnemyStateController.GetEnemy().GetHealth() > 0){
                    //     StartCoroutine(RecoverAfterDelay(nearbyEnemyStateController, delay));
                    //     // nearbyEnemyRagdollController.ApplyForce(pushDirection, force);
                    // }

                    nearbyEnemyStateController.SetShouldRagdoll(true);
                    if(nearbyEnemyStateController.GetCurrentState() is not EnemyHitState){
                        nearbyEnemyStateController.ChangeState(new EnemyHitState());
                    }

                    if(nearbyEnemyStateController.GetEnemy().GetHealth() > 0){
                        Debug.Log("Inside health check");
                        if(!nearbyEnemyStateController.GetisInRecovery()){
                            nearbyEnemyStateController.GetEnemy().GetRagdollController().ApplyForce(pushDirection, chargedPushForce);
                            nearbyEnemyStateController.Recovery(ragdollDuration);
                        }
                    } else {
                        nearbyEnemyStateController.ChangeState(new EnemyDieState());
                        Debug.Log("Health check not passed");
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
