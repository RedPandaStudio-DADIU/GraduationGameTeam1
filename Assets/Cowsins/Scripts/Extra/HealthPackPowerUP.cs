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
          private bool showed = false;

          private int popID=0; 

          private PopupManager popupManager;
 
         private void Start()
        {
            // Find the PopupManager in the scene
            popupManager = FindObjectOfType<PopupManager>();
            if (popupManager == null)
            {
                Debug.LogError("PopupManager not found in the scene.");
            }
        }
        
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

                if (popupManager != null&& showed==false)
                {
                    popupManager.CompletepopUp();
                    showed = true;
                }

                Destroy(gameObject); 
 
            }

                if (popupManager != null && popupManager.GetCurrentpopUpID() != popID)
                {
                    return; // This popup is not the current task
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

