using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cowsins
{
    public class HealthPackPowerUp : PowerUp
    {
        [Tooltip("Amount of health to be restored")] [Range(.1f, 1000), SerializeField] private float healAmount;
        [SerializeField] private Image healthPackUI;

       
        public override void Interact(PlayerStats player)
        {
            
            if (player != null)
            {
                player.Heal(healAmount); 
                 if (healthPackUI != null)
                {
                    SetUIAlpha(0.5f); 
                }
                // used = true; 
                // timer = reappearTime; 
                 Destroy(gameObject); 
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !used)
            {
                
                 Debug.Log("collide.healpack");
                 used = true;
                timer = reappearTime;
                if (healthPackUI != null)
                {
                    SetUIAlpha(1f); 
                }

                if (InputManager.heal) 
                {
                    Debug.Log("heal.");
                    Interact(other.GetComponent<PlayerStats>());
                }
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