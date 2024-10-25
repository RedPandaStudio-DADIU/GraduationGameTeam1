using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnemy : EnemyBaseClass
{

    public override void Attack(){
        Debug.Log("Attacking");
    }

    public override void Die(){
        // play dying animation
        Destroy(this.gameObject);
    }

}
