using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using cowsins;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Presets;
#endif

namespace cowsins
{

    public class EnemyWeaponController : MonoBehaviour
    {
        //References
        [Tooltip("An array that includes all your initial weapons.")] public Weapon_SO[] initialWeapons;

        public WeaponIdentification[] inventory;
        // public Transform playerTransform;
        private Transform playerTransform;

        public UISlot[] slots;

        public Weapon_SO weapon;

        private Transform[] firePoint;

        [Tooltip("Attach your weapon holder")] public Transform weaponHolder;

        //Variables

        [Tooltip("max amount of weapons you can have")] public int inventorySize;

        public bool shooting { get; private set; } = false;

        private float damagePerBullet;

        private float penetrationAmount;

        // Effects
        public Effects effects;

        public Events events;
        // Internal Use
        private int bulletsPerFire;

        public bool canShoot;

        RaycastHit hit;

        public int currentWeapon;

        private AudioClips audioSFX;

        public WeaponIdentification id;

        private WeaponAnimator weaponAnimator; 

        private GameObject muzzleVFX;

        private float fireRate;

        public bool holding;

        public delegate void PerformShootStyle();

        public PerformShootStyle performShootStyle;

        private AudioClip fireSFX;

        void Start(){
            GetInitialWeapons();
            playerTransform = this.gameObject.GetComponent<EnemyStateController>().GetPlayerTransform();
            Debug.Log("Player Transform in Enemy Weapon Controller: " + playerTransform.position + " name: " + playerTransform.gameObject.name);
        }

        public void HandleHitscanProjectileShot()
        {
            foreach (var p in firePoint)
            {
                canShoot = false; // since you have already shot, you will have to wait in order to being able to shoot again
                bulletsPerFire = weapon.bulletsPerFire;
                StartCoroutine(HandleShooting());

            }
            if (weapon.timeBetweenShots == 0) SoundManager.Instance.PlaySound(fireSFX, 0, weapon.pitchVariationFiringSFX, true, 0);
            Invoke(nameof(CanShoot), fireRate);
        }


        private IEnumerator HandleShooting()
        {
            int style = 1;

            int i = 0;
            while (i < bulletsPerFire)
            {
                if (weapon == null) yield break;
                shooting = true;

                // Determine if we want to add an effect for FOV
                if (weapon.applyFOVEffectOnShooting)
                {
                    float fovAdjustment = weapon.FOVValueToSubtract;
                    // mainCamera.fieldOfView -= fovAdjustment;
                }
                foreach (var p in firePoint)
                {
                    if (muzzleVFX != null)
                        Instantiate(muzzleVFX, p.position, transform.rotation, transform); // VFX
                }
                // CowsinsUtilities.ForcePlayAnim("shooting", inventory[currentWeapon].GetComponentInChildren<Animator>());
                if (weapon.timeBetweenShots != 0) SoundManager.Instance.PlaySound(fireSFX, 0, weapon.pitchVariationFiringSFX, true, 0);

                            
                // StartCoroutine(EnemyShooting());
                // if(weapon)
                ProjectileShot();

        

                i++;
            }
            shooting = false;
            yield break;
        }
        


        // private IEnumerator EnemyShooting()
        // {
        //     // Wait until the next time we should display our new distance
        //     yield return new WaitForSeconds(2);
        //     ProjectileShot();
        // }


        // fix destination problems 
        private void ProjectileShot()
        {
            Debug.Log("Inside Projectile Shot Enemy: " + this.gameObject.name);
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 shotDirection = directionToPlayer; 
            Ray ray = new Ray(transform.position, shotDirection);


            Debug.Log("Projectile Enemy Shooting");

            foreach (var p in firePoint)
            {
                Bullet bullet = Instantiate(weapon.projectile, p.position, p.rotation) as Bullet;
                if(this.gameObject.CompareTag("Human")){
                    bullet.isHuman = true;
                } else{
                    bullet.isEnemy = true;
                }
                Debug.Log("Instantiated bullet at position: " + p.position);

                bullet.hurtsPlayer = weapon.hurtsPlayer;
                bullet.explosionOnHit = weapon.explosionOnHit;
                bullet.explosionRadius = weapon.explosionRadius;
                bullet.explosionForce = weapon.explosionForce;
                bullet.criticalMultiplier = weapon.criticalDamageMultiplier;
                // bullet.destination = destination;
                // bullet.destination = playerTransform.position;
                bullet.destination = new Vector3(playerTransform.position.x, playerTransform.position.y+0.5f, playerTransform.position.z);
                bullet.player = this.transform;
                bullet.speed = weapon.speed;
                bullet.GetComponent<Rigidbody>().isKinematic = !weapon.projectileUsesGravity;
                Debug.Log("DAMAGE PER BULLET: " + weapon.damagePerBullet);
                bullet.damage = weapon.damagePerBullet;
                bullet.duration = weapon.bulletDuration;

                // Check if bullet has necessary components
                Debug.Log("Bullet set with destination: " + bullet.destination + " and speed: " + bullet.speed);
            }
        }


        private void CanShoot() => canShoot = true;

        public float GetFireRate(){
            return this.fireRate;
        }

        private void GetInitialWeapons()
        {
            if (initialWeapons.Length == 0) return;

            int i = 0;

            InstantiateWeapon(initialWeapons[0], 0, null, null);

            weapon = initialWeapons[0];
        }

        public void InstantiateWeapon(Weapon_SO newWeapon, int inventoryIndex, int? _bulletsLeftInMagazine, int? _totalBullets)
        {
            // Instantiate Weapon
            var weaponPicked = Instantiate(newWeapon.weaponObject, weaponHolder);
            weaponPicked.transform.localPosition = newWeapon.weaponObject.transform.localPosition;
            // weaponPicked.transform.position = new Vector3(transform.position.x+1, transform.position.y+2f, transform.position.z);

            // Destroy the Weapon if it already exists in the same slot
            if (inventory[inventoryIndex] != null) Destroy(inventory[inventoryIndex].gameObject);

            // Set the Weapon
            inventory[inventoryIndex] = weaponPicked;

            // Select weapon if it is the current Weapon
            if (inventoryIndex == currentWeapon)
            {
                weapon = newWeapon;
            }
            else weaponPicked.gameObject.SetActive(false);

            List<AttachmentIdentifier_SO> attachments = null;
            int magCapacityAdded = 0;


            // if _bulletsLeftInMagazine is null, calculate magazine size. If not, simply assign _bulletsLeftInMagazine
            inventory[inventoryIndex].bulletsLeftInMagazine = _bulletsLeftInMagazine ?? (newWeapon.magazineSize + magCapacityAdded);
            inventory[inventoryIndex].totalBullets = _totalBullets ??
            (newWeapon.limitedMagazines
                ? newWeapon.magazineSize * newWeapon.totalMagazines
                : newWeapon.magazineSize);


            firePoint = inventory[0].FirePoint;

        }

        public void SetPlayerTransform(Transform newPlayerTransform){
            this.playerTransform = newPlayerTransform;
        }

        void OnDisable()
        {
            StopAllCoroutines(); 
            CancelInvoke();
        }

    }

}



