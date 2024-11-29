using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using AK.Wwise;

public class StandardEnemy : EnemyBaseClass
{
    [SerializeField] private AK.Wwise.Event takeDamageEvent;
    [SerializeField] private AK.Wwise.Event barkEvent;


    void Awake(){
        this.SetHealth(60f);
        this.SetAttackDistance(25f);
        this.SetFieldOfView(180f);
        this.SetStoppingDistance(15f);
    }

    void Start(){
        this.barkEvent.Post(this.gameObject);
    }

    public override void Attack(){
        // turn towards the player and do the raycast
        Debug.Log("Attacking");
        //GetComponent<EnemyWeaponController>().HandleHitscanProjectileShot();
    }

    public override void Die(){
        // play dying animation?
        Destroy(this.gameObject);
    }

    public override void LosePlayer(Vector3 playerPosition){
        // Immediately goes into IdleState
        GetComponent<EnemyStateController>().ChangeState(new EnemyIdleState());

    }

    public override AK.Wwise.Event GetDamageSound(){
        return this.takeDamageEvent;
    }

}
