using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : EnemyBaseClass
{

    public override void Attack(){
        // turn towards the player and do the raycast
        Debug.Log("Attacking");
    }

    public override void Die(){
        // play dying animation
        Destroy(this.gameObject);
    }

}
