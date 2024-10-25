using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplomatEnemy : EnemyBaseClass
{
    
    void Awake(){
        this.SetHealth(10f);
        this.SetAttackDistance(25f);
        this.SetFieldOfView(180f);
        this.SetStoppingDistance(6f);
    }

    // assign different gun

    public override void Attack(){
        Debug.Log("Diplomat Attacking");
    }

    public override void Die(){
        // play dying animation
        Destroy(this.gameObject);
    }

}
