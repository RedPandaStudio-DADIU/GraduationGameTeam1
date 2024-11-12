/// <summary>
/// This script belongs to cowsins� as a part of the cowsins� FPS Engine. All rights reserved. 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cowsins
{
    /// <summary>
    /// Incline your weapon whenever you crouch
    /// </summary>
    public class CrouchTilt : MonoBehaviour
    {

        [Tooltip("Rotation desired when crouching"), SerializeField] private Vector3 tiltRot, tiltPosOffset;

        [Tooltip("Tilting / Rotation velocity"), SerializeField] private float tiltSpeed;

        // [HideInInspector] public PlayerMovement player;
        [SerializeField] private PlayerMovement player;
        private GameObject playerTest;

        [HideInInspector] public WeaponController wp;

        private bool crouching;

        private Quaternion origRot;

        private Vector3 origPos;

        void Start()
        {
            playerTest = GameObject.FindGameObjectWithTag("Player");//.GetComponent<PlayerMovement>();
            if (playerTest == null) {
                Debug.LogError("Player game object component not found on Player object!");
            }
            // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

            // wp = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
            if (!playerTest.TryGetComponent(out player))
            {
                Debug.LogError("1——PlayerMovement component not found on Player object!");
            }

            if (!playerTest.TryGetComponent(out wp))
            {
                Debug.LogError("1——WeaponController component not found on Player object!");
            }
                    
            origRot = transform.localRotation;
            origPos = transform.localPosition;
            if (player == null) {
                Debug.LogError("?PlayerMovement component not found on Player object!");
            }
            if (wp == null) {
                Debug.LogError("?WeaponController component not found on Player object!");
            }

        }

        // IEnumerator DelayedInit()
        // {
        //     yield return new WaitForSeconds(0.1f);

        //     playerTest = GameObject.FindGameObjectWithTag("Player");
        //     if (playerTest == null)
        //     {
        //         Debug.LogError("Player game object not found with tag 'Player'!");
        //         yield break;
        //     }

        //     if (!playerTest.TryGetComponent(out player))
        //     {
        //         Debug.LogError("PlayerMovement component not found on Player object!");
        //     }

        //     if (!playerTest.TryGetComponent(out wp))
        //     {
        //         Debug.LogError("WeaponController component not found on Player object!");
        //     }

        //     origRot = transform.localRotation;
        //     origPos = transform.localPosition;
        // }

        // void Start()
        // {
        //     StartCoroutine(DelayedInit());
        // }


        void Update()
        {
            if (player == null || wp == null)
            {
                Debug.LogWarning("PlayerMovement or WeaponController still not ready, escape Update。");
                return;
            }
            
            
            // If we are crouching + not aiming Tilt
            if (player.isCrouching && !wp.isAiming)
            {
                crouching = true;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(tiltRot), Time.deltaTime * tiltSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, origPos + tiltPosOffset, Time.deltaTime * tiltSpeed);
            }
            else // If not, come back
            {
                crouching = false;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, origRot, Time.deltaTime * tiltSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, origPos, Time.deltaTime * tiltSpeed);
            }
            if (crouching && wp.isAiming)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, origRot, Time.deltaTime * tiltSpeed);
                transform.localPosition = Vector3.Lerp(transform.localPosition, origPos, Time.deltaTime * tiltSpeed);
            }
        }
    }
}