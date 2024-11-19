using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    [SerializeField] private float noHitsToDestroy = 3f;
    [SerializeField] private Boss boss;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CheckIfCanBeDamaged(){
        if(boss.CheckIfOnTop(this.transform)){
            noHitsToDestroy -= 1;

            if(noHitsToDestroy == 0){
                FallOff();
            }
        }
    }

    public void FallOff(){
        boss.RemoveFromQueue();
        // Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        // if(rb!=null){
        //     rb.mass = 1f;
        //     rb.drag = 0.5f;
        //     rb.angularDrag = 0.05f;
        //     rb.useGravity = true;
        // }

        if(boss.CheckIfEmpty()){
            boss.SetAreWeakSpotsDefeated(true);
        }

        Debug.LogWarning("Weakspot " + this.gameObject.name + " defeated!");
    }
}
