using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : EnemyBaseClass
{
    
    void Awake(){
        this.SetHealth(150f);
        this.SetAttackDistance(10f);
        this.SetFieldOfView(120f);
        this.SetStoppingDistance(0f);
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

}
