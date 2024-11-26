using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class DiplomatEnemy : EnemyBaseClass
{
    
    void Awake(){
        this.SetHealth(120f);
        this.SetAttackDistance(20f);
        this.SetFieldOfView(180f);
        this.SetStoppingDistance(6f);
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

}
