using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cowsins
{
    public class WeaponAnimator : MonoBehaviour
    {
        private PlayerMovement player;
        private WeaponController wc;
        private InteractManager interactManager;
        private Rigidbody rb;

        void Start()
        {
            player = GetComponent<PlayerMovement>();
            wc = GetComponent<WeaponController>();
            interactManager = GetComponent<InteractManager>();
            rb = GetComponent<Rigidbody>();

            //StartCoroutine(DelayedInitialization());
        }

        // IEnumerator DelayedInitialization()
        // {
        //     yield return new WaitForSeconds(0.1f);

        //     player = GetComponent<PlayerMovement>();
        //     wc = GetComponent<WeaponController>();
        //     interactManager = GetComponent<InteractManager>();
        //     rb = GetComponent<Rigidbody>();

        //     if (wc.inventory == null || wc.inventory.Length == 0)
        //     {
        //         Debug.LogError("Inventory is not initialized even after delay.");
        //     }
        // }


        // Update is called once per frame
        void FixedUpdate()
        {
            //if (wc.inventory[wc.currentWeapon] == null) return;
            if (wc == null)
            {
                wc = GetComponent<WeaponController>();
                if (wc == null)
                {
                    Debug.LogError("WeaponController is still not initialized in Update.");
                    return;
                }
                else
                {
                    Debug.Log("WeaponController successfully initialized in Update.");
                }
            }

            if (wc.inventory.Length == 0)
            {
                Debug.LogError("Inventory length is 0.");
                return;
            }


            if (wc.inventory == null || wc.inventory.Length == 0)
            {
                Debug.LogError("Inventory is not initialized or empty.");
                return;
            }

            if (wc.currentWeapon < 0 || wc.currentWeapon >= wc.inventory.Length)
            {
                Debug.LogError($"Current weapon index {wc.currentWeapon} is out of range. Inventory length: {wc.inventory.Length}");
                return;
            }
             var weapon = wc.inventory[wc.currentWeapon];
            if (weapon == null)
            {
                Debug.LogError($"Current weapon in inventory is null at index {wc.currentWeapon}.");
                return;
            }

            Animator currentAnimator = wc.inventory[wc.currentWeapon].GetComponentInChildren<Animator>();

            if (player.wallRunning && !wc.Reloading)
            {
                CowsinsUtilities.PlayAnim("walking", currentAnimator);
                return;

            }
            if (wc.Reloading || wc.shooting || player.isCrouching || !player.grounded || rb.velocity.magnitude < 0.1f || wc.isAiming
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("Unholster")
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("reloading")
                || currentAnimator.GetCurrentAnimatorStateInfo(0).IsName("shooting"))
            {
                CowsinsUtilities.StopAnim("walking", currentAnimator);
                CowsinsUtilities.StopAnim("running", currentAnimator);
                return;
            }

            if (rb.velocity.magnitude > player.crouchSpeed && !wc.shooting && player.currentSpeed < player.runSpeed && player.grounded && !interactManager.inspecting) CowsinsUtilities.PlayAnim("walking", currentAnimator);
            else CowsinsUtilities.StopAnim("walking", currentAnimator);

            if (player.currentSpeed >= player.runSpeed && player.grounded) CowsinsUtilities.PlayAnim("running", currentAnimator);
            else CowsinsUtilities.StopAnim("running", currentAnimator);
        }

        public void StopWalkAndRunMotion()
        {
            if (!wc) return; // Ensure there is a reference for the Weapon Controller before running the following code
            Animator weapon = wc.inventory[wc.currentWeapon].GetComponentInChildren<Animator>();
            CowsinsUtilities.StopAnim("inspect", weapon);
            CowsinsUtilities.StopAnim("walking", weapon);
            CowsinsUtilities.StopAnim("running", weapon);
        }
    }

}