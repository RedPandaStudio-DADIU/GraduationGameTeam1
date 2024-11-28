using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using AK.Wwise;


public class DiplomatEnemy : EnemyBaseClass
{
    [SerializeField] private AK.Wwise.Event takeDamageEvent;
    [SerializeField] private AK.Wwise.Event barkEvent;


    void Awake(){
        this.SetHealth(90f);
        this.SetAttackDistance(35f);
        this.SetFieldOfView(180f);
        this.SetStoppingDistance(25f);
    }

    void Start(){
        this.barkEvent.Post(this.gameObject);
    }

    // assign different gun

    public override void Attack(){
        Debug.Log("Diplomat Attacking");
        GetComponent<EnemyWeaponController>().HandleHitscanProjectileShot();
    }

    public override void Die(){
        // play dying animation
        Destroy(this.gameObject);
    }

    public override void LosePlayer(Vector3 playerPosition){
        // Gets the last known player position and goes there - agent - destination and then IdleState()
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = playerPosition;
    }


    public override AK.Wwise.Event GetDamageSound(){
        return this.takeDamageEvent;
    }
}
