using UnityEngine;
namespace cowsins
{
    public class BulletsPickeable : Pickeable
    {
        [Tooltip("How many bullets you will get"), SerializeField] private int amountOfBullets;

        [SerializeField] private Sprite bulletsIcon;

        [SerializeField] private GameObject bulletsGraphics;

        public override void Start()
        {
            image.sprite = bulletsIcon;
            Destroy(graphics.transform.GetChild(0).gameObject);
            Instantiate(bulletsGraphics, graphics);
            base.Start();
        }
        public override void Interact(Transform player)
        {
            if (player.GetComponent<WeaponController>().weapon == null) 
            {
                Debug.Log("No weapon equipped");
                return;
            }
            Debug.Log($"Before adding bullets: TotalBullets = {player.GetComponent<WeaponController>().id.totalBullets}");

            
            base.Interact(player);
            player.GetComponent<WeaponController>().id.totalBullets += amountOfBullets;
            Debug.Log($"After adding bullets: TotalBullets = {player.GetComponent<WeaponController>().id.totalBullets}");

            Destroy(this.gameObject);
        }
    }
}