using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;


public class ShieldEnemy : EnemyBaseClass
{
    [SerializeField] private AK.Wwise.Event takeDamageEvent;
    [SerializeField] private AK.Wwise.Event barkEvent;


    void Awake(){
        this.SetHealth(150f);
        this.SetAttackDistance(40f);
        this.SetFieldOfView(120f);
        this.SetStoppingDistance(0f);
    }

    void Start(){
        this.barkEvent.Post(this.gameObject);
    }

    // assign different gun

    public override void Attack(){
        Debug.Log("Shield Attacking" + this.GetAttackDistance());
    }

    public override void Die(){
        // play dying animation
        Destroy(this.gameObject);
    }

    public override void LosePlayer(Vector3 playerPosition){
        // Never loose the player - always charge him
    }

    public override AK.Wwise.Event GetDamageSound(){
        return this.takeDamageEvent;
    }

}
