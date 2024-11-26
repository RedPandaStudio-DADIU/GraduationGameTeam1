/// <summary>
/// This script belongs to cowsins� as a part of the cowsins� FPS Engine. All rights reserved. 
/// </summary>using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace cowsins
{
    public class Bullet : MonoBehaviour
    {
        private Camera playerCamera;  
        [SerializeField] private float bigRagdollForce = 600f;
        [SerializeField] private float smallRagdollForce = 200f;
        [SerializeField] private float ragdollDuration = 4.0f; 
        [SerializeField] private float smallRagdollDuration = 3.0f;

        [HideInInspector] public bool isEnemy = false; 
        [HideInInspector] public bool isHuman = false; 

        [HideInInspector] public float speed;
        [HideInInspector] public float damage;
        [HideInInspector] public Vector3 destination;
        [HideInInspector] public bool gravity;
        [HideInInspector] public Transform player;
        [HideInInspector] public bool hurtsPlayer;
        [HideInInspector] public bool explosionOnHit;
        [HideInInspector] public GameObject explosionVFX;
        [HideInInspector] public float explosionRadius;
        [HideInInspector] public float explosionForce;
        [HideInInspector] public float criticalMultiplier;
        [HideInInspector] public float duration;

        private bool projectileHasAlreadyHit = false; // Prevent from double hitting issues

        private void Start()
        {
            transform.LookAt(destination);
            Invoke(nameof(DestroyProjectile), duration);
            playerCamera = Camera.main;
            ragdollDuration = 3f;
        }

        private void Update()
        {
            transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (projectileHasAlreadyHit) return;

            // if (other.CompareTag("Critical"))
            // {
            //     // DamageTarget(other.transform, damage * criticalMultiplier, true);
            //     Debug.Log("Critical");
            //     GameObject parentObject = other.transform.parent.gameObject;
            //     Shoot(damage * criticalMultiplier, parentObject.GetComponent<Collider>());
            // }
            // else if (other.CompareTag("BodyShot"))
            // {
            //     // DamageTarget(other.transform, damage, false);
            //     Debug.Log("Body Shot!!");
            //     GameObject parentObject = other.transform.parent.gameObject;
            //     Shoot(damage, parentObject.GetComponent<Collider>());
            // } 



            // Set tag for enemy projectiles as EnemyBullet <- compare tag of this object (EnemyBullet) and of other (Enemy)

            if(other.CompareTag("Enemy") && this.gameObject.CompareTag("EnemyFire")){
                DestroyProjectile();
            } else if(other.CompareTag("Enemy") && !isHuman) {
                // DamageTarget(other.transform, damage, false);
                // Debug.LogError("Enemy " + other.gameObject.name + " hit!");
                if(this.CompareTag("ChargeShot")){
                    
                    player.gameObject.GetComponent<ChargedShot>().ShootCharge(other.gameObject, this.transform.position);
                    DestroyProjectile();
                } else {
                    if(!other.GetComponent<EnemyStateController>().GetInAFight()){
                        Debug.LogWarning("Enemy Shot!! Damage: " + damage);
                        Shoot(damage, other);
                    }
                    DestroyProjectile();

                }

            }  else if(other.CompareTag("Window")){
                DestroyProjectile();
            } 
            else if(other.CompareTag("WeakSpot")){
                Debug.LogWarning("Hit weak spot" + other.gameObject.name);
                other.gameObject.GetComponent<WeakSpot>().CheckIfCanBeDamaged();
            } else if(other.CompareTag("Boss")){
                Debug.LogWarning("Hit boss");

                if(other.gameObject.GetComponent<Boss>().GetAreWeakSpotsDefeated()){
                    Shoot(damage, other);
                }
            } 
            
            else if (other.CompareTag("Barrel"))
            {
                Debug.Log("Bullet hit barrel!");
                ExplosiveBarrel barrel = other.GetComponent<ExplosiveBarrel>();
                if (barrel != null)
                {
                    Debug.Log("Bullet cause explosive barrel to explode");
                    barrel.Die();
                }
                DestroyProjectile();
            }
            else if (other.GetComponent<IDamageable>() != null && !other.CompareTag("Player"))
            {
                DamageTarget(other.transform, damage, false);
            } 
            else if (other.CompareTag("Player") && isEnemy){
                Debug.Log("Enemy bullet hit the player with damage: " + damage + " player health: " + other.GetComponent<PlayerStats>().health);
                other.GetComponent<PlayerStats>().Damage(damage, false);
                DestroyProjectile();
                // DamageTarget(other.transform, damage, false);
            }
            else if((other.CompareTag("Enemy") && isHuman) || (other.CompareTag("Human") && isEnemy)){
                DestroyProjectile();
            }
            
            else if (IsGroundOrObstacleLayer(other.gameObject.layer))
            {
                DestroyProjectile();
            }
        }

        private void DamageTarget(Transform target, float dmg, bool isCritical)
        {
            var damageable = CowsinsUtilities.GatherDamageableParent(target);
            if (damageable != null)
            {
                damageable.Damage(dmg, isCritical);
                projectileHasAlreadyHit = true;
                DestroyProjectile();
            }
        }

        private void Shoot(float damage, Collider other){


            EnemyStateController enemy = other.GetComponent<EnemyStateController>();

            if (enemy != null)
            {
                // enemy.GetEnemy().GetComponent<EnemyHealth>().Damage(damage, false);
                enemy.GetEnemy().DecreaseHealth(damage);
                
                Vector3 pushDirection = this.transform.position - playerCamera.transform.position; 
                pushDirection = pushDirection.normalized;

                enemy.SetForceDirection(pushDirection);
                enemy.SetForce(bigRagdollForce);
                if(enemy.GetCurrentState() is not EnemyHitState){
                    enemy.ChangeState(new EnemyHitState());
                }

                if(enemy.GetEnemy().GetHealth() > 0){
                    Debug.Log("Inside health check");
                    if(!enemy.GetisInRecovery()){
                        enemy.Recovery(ragdollDuration);
                    }
                } else {
                    enemy.ChangeState(new EnemyDieState());
                    Debug.Log("Health check not passed");
                }

            }

        }

        private bool IsGroundOrObstacleLayer(int layer)
        {
            return layer == LayerMask.NameToLayer("Ground") || layer == LayerMask.NameToLayer("Object")  || layer == LayerMask.NameToLayer("Spaceships")
                || layer == LayerMask.NameToLayer("Grass") || layer == LayerMask.NameToLayer("Metal") ||
                layer == LayerMask.NameToLayer("Mud") || layer == LayerMask.NameToLayer("Wood") || layer == LayerMask.NameToLayer("Enemy");
        }

        private void DestroyProjectile()
        {
            if (explosionOnHit)
            {
                if (explosionVFX != null)
                {
                    var contact = GetComponent<Collider>().ClosestPoint(transform.position);
                    Instantiate(explosionVFX, contact, Quaternion.identity);
                }

                Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

                foreach (var collider in colliders)
                {
                    var damageable = collider.GetComponent<IDamageable>();
                    var playerMovement = collider.GetComponent<PlayerMovement>();
                    var rigidbody = collider.GetComponent<Rigidbody>();

                    if (damageable != null)
                    {
                        // Calculate the distance ratio and damage based on the explosion radius
                        float distanceRatio = 1 - Mathf.Clamp01(Vector3.Distance(collider.transform.position, transform.position) / explosionRadius);
                        float dmg = damage * distanceRatio;

                        // Apply damage if the collider is a player and the explosion should hurt the player
                        if (collider.CompareTag("Player") && hurtsPlayer)
                        {
                            damageable.Damage(dmg, false);
                        }
                        // Apply damage if the collider is not a player
                        else if (!collider.CompareTag("Player"))
                        {
                            damageable.Damage(dmg, false);
                        }
                    }

                    if (playerMovement != null)
                    {
                        CamShake.instance.ExplosionShake(Vector3.Distance(CamShake.instance.gameObject.transform.position, transform.position));
                    }

                    if (rigidbody != null && collider != this)
                    {
                        rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius, 5, ForceMode.Force);
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
