using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace cowsins
{

public class OurMagazine : Pickeable
{
    [SerializeField] private int bulletAmount = 30;
    [SerializeField] private TextMeshProUGUI bulletsUIText;
    private GameObject Player;
    private WeaponController weaponController;

    private int popID=1; 
    private bool showed = false;
    

    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Player= GameObject.FindWithTag("Player");
        weaponController = Player.GetComponent<WeaponController>();
      
        bulletsUIText.text = $"{weaponController.id.totalBullets }";
        //weaponController.id.totalBullets = 45;
        Debug.Log($"start with weaponController.id.totalBullets : {weaponController.id.totalBullets}");

    }

    public override void Interact(Transform player)
    {
        base.Interact(player);
       // cachedPlayer = player; 

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
                
            

        Debug.Log("Picked up magzine");
       // WeaponController weaponController = player.GetComponent<WeaponController>();
        Debug.Log($"weaponController.id.totalBullets : {weaponController.id.totalBullets}");


        if (bulletsUIText != null)
            {
                weaponController.id.totalBullets += bulletAmount;
                int currentBullets = 0;
                if(weaponController.id.totalBullets - 15 >=0){
                    currentBullets = weaponController.id.totalBullets - 15;
                }
                
                //int currentBullets = int.Parse(bulletsUIText.text);  // Assuming bullets are displayed as an integer
                //currentBullets += bulletAmount;  // Add the picked up amount to the current bullets
                //weaponController.id.totalBullets += bulletAmount;
                bulletsUIText.text = $"{weaponController.id.totalBullets}";  // Update the text field
                Debug.Log($"Updated bullets UI: {weaponController.id.totalBullets}");
            }

        Destroy(this.gameObject);
    }

    public void UpdateBulletsUIText()
    {
        //private Transform Player= GameObject.FindWithTag("Player");
        if (bulletsUIText != null&& Player != null) 
        {
            WeaponController weaponController = Player.GetComponent<WeaponController>();
            if(weaponController.id.totalBullets > 15){
                bulletsUIText.text = $"{weaponController.id.totalBullets - 15}";
            } else{
                bulletsUIText.text = $"{0}";
            }
            
            Debug.Log($"reload and Updated bullets UI: {weaponController.id.totalBullets }");
        }
    }

    private void Update()
    {
         if (InputManager.reloading) 
            {
                Debug.Log("input reload");
                UpdateBulletsUIText();
            }
    }

   
    }
} // namespace cowsins
