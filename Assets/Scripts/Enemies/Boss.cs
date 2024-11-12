using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : DiplomatEnemy
{
    [SerializeField] private List<Transform> weakSpots = new List<Transform>();
    private Queue<Transform> queueWeakSpots;
    private bool areWeakSpotsDefeated = false;
        
    void Awake(){
        this.SetHealth(200f);
        queueWeakSpots = new Queue<Transform>(weakSpots);

    }

    // public void DecreaseHealthBoss(float damage){
    //     if (areWeakSpotsDefeated){
    //         if(this.health > 0){
    //             this.health -= damage;
    //         }

    //         if (hitSoundEvent != null)
    //         {
    //             hitSoundEvent.Post(gameObject);
    //             Debug.Log("Played hit sound for boss.");
    //         }
    //     }

    // }
    
}
