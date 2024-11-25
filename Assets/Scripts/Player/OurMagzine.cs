using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace cowsins
{

public class OurMagzine : Pickeable
{
    [SerializeField] private int bulletAmount = 30;
    [SerializeField] private TextMeshProUGUI bulletsUIText;
   
    

    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //weaponController.id.totalBullets = 45;
        
    }

    public override void Interact(Transform player)
    {
        base.Interact(player);
        Debug.Log("Picked up magzine");
        WeaponController weaponController = player.GetComponent<WeaponController>();
        Debug.Log($"weaponController.id.totalBullets : {weaponController.id.totalBullets}");


        if (bulletsUIText != null)
            {
                int currentBullets = int.Parse(bulletsUIText.text);  // Assuming bullets are displayed as an integer
                currentBullets += bulletAmount;  // Add the picked up amount to the current bullets
                weaponController.id.totalBullets += bulletAmount;
                bulletsUIText.text = $"{currentBullets}";  // Update the text field
                Debug.Log($"Updated bullets UI: {currentBullets}");
            }

        Destroy(this.gameObject);
    }
         

   
}
} // namespace cowsins
