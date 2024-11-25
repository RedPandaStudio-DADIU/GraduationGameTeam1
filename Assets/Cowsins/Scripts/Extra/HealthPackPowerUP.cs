using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace cowsins
{
    public class HealthPackPowerUp : MonoBehaviour
    {
        [Tooltip("Amount of health to be restored")] [Range(.1f, 1000), SerializeField] private float healAmount;
        [SerializeField] private Image healthPackUI;


         private PlayerStats player;

          private bool able = false;
          private bool used = false;

       
        // public override void Interact(PlayerStats player)
        // {
            
        //     if (player != null)
        //     {
        //         player.Heal(healAmount); 
        //          if (healthPackUI != null)
        //         {
        //             SetUIAlpha(0.5f); 
        //         }
        //         // used = true; 
        //         // timer = reappearTime; 
        //          Destroy(gameObject); 
        //     }
        // }

        // private void OnTriggerStay(Collider other)
        // {
        //     if (other.CompareTag("Player") && !used)
        //     {
                
        //          Debug.Log("collide.healpack");
        //          used = true;
        //         timer = reappearTime;
        //         if (healthPackUI != null)
        //         {
        //             SetUIAlpha(1f); 
        //         }

        //         if (InputManager.heal) 
        //         {
        //             Debug.Log("heal.");
        //             Interact(other.GetComponent<PlayerStats>());
        //         }
        //     }
        // }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !used)
            {
                Debug.Log("Player entered health pack trigger.");
                able = true;
                player = other.GetComponent<PlayerStats>();
               
                if (healthPackUI != null)
                {
                    SetUIAlpha(1f); 
                }
            }
        }

        private void Update()
        {
            
            if ( InputManager.heal && able)
            {
                Debug.Log("Healing player.");
                player.Heal(healAmount); 
                SetUIAlpha(0.5f); 
                used= true;
                able = false;
                Destroy(gameObject); 

            }
        }



        private void SetUIAlpha(float alpha)
        {
            if (healthPackUI != null)
            {
                Color color = healthPackUI.color;
                color.a = alpha; 
                healthPackUI.color = color; 
            }
        }
    }
}