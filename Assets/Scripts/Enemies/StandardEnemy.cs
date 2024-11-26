using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class StandardEnemy : EnemyBaseClass
{

    void Awake(){
        this.SetHealth(80f);
        this.SetAttackDistance(10f);
        this.SetFieldOfView(180f);
        this.SetStoppingDistance(4f);
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

}
