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

    public bool CheckIfOnTop(Transform weakspot){
        if(queueWeakSpots.Count>0 && queueWeakSpots.Peek() == weakspot){
            return true;
        } else{
            return false;
        }
    }

    public void SetAreWeakSpotsDefeated(bool value){
        this.areWeakSpotsDefeated = value;
    }

    public bool GetAreWeakSpotsDefeated(){
        return this.areWeakSpotsDefeated;
    }
    
    public void RemoveFromQueue(){
        queueWeakSpots.Dequeue();
    }

    public bool CheckIfEmpty(){
        if(queueWeakSpots.Count == 0){
            return true;
        }
        return false;
    }

}
