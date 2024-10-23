using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void OnEnter(EnemyStateController stateController){
        Debug.Log("Entering Idle State");
    }
    public void OnUpdate(EnemyStateController stateController){
        if (CanSeePlayer(stateController))
        {
            Debug.Log("Can See Player");
            // Transition to Attack State
            stateController.ChangeState(new EnemyAttackState());
        }
    }
    public void OnExit(EnemyStateController stateController){
        Debug.Log("Exiting Idle State");
    }

    private bool CanSeePlayer(EnemyStateController stateController){
        EnemyBaseClass enemy = stateController.GetEnemy();
        Transform playerTransform = stateController.GetPlayerTransform();

        // Check if enemy within attacking distance of the player
        float attackDistance = enemy.GetAttackDistance();

        Vector3 directionToPlayer = playerTransform.position - enemy.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > attackDistance) return false;

        Debug.Log("Distance check passed ");

        // Check if enemy can see the player - player in the field of view of an enemy
        float fieldOfViewAngle = enemy.GetFieldOfView();

        float angleToPlayer = Vector3.Angle(enemy.transform.forward, directionToPlayer);
        if (angleToPlayer > fieldOfViewAngle) return false;


        Debug.Log("Angle check passed ");

        Debug.DrawRay(enemy.transform.position, directionToPlayer.normalized * distanceToPlayer, Color.red);


        // Check whether there are no obstacles on the way to the player
        if (Physics.Raycast(enemy.transform.position, directionToPlayer, out RaycastHit hit, attackDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        Debug.Log("Raycast check not passed");

        return false;

    }
}

