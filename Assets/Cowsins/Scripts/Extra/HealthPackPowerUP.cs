using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cowsins
{
    public class HealthPackPowerUp : PowerUp
    {
         [Tooltip("Amount of health to be restored")] [Range(.1f, 1000), SerializeField] private float healAmount;
       
        public override void Interact(PlayerStats player)
        {
            
            if (player != null)
            {
                player.Heal(healAmount); 
                used = true; 
                timer = reappearTime; 
                Destroy(gameObject); 
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !used)
            {
                
                 Debug.Log("collide.");
                if (InputManager.heal) 
                {
                    Debug.Log("heal.");
                    Interact(other.GetComponent<PlayerStats>());
                }
            }
        }
    }
}