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
        // play dying animation?
        Destroy(this.gameObject);
    }

    public override void LosePlayer(Vector3 playerPosition){
        // Immediately goes into IdleState
        GetComponent<EnemyStateController>().ChangeState(new EnemyIdleState());

    }

}
