using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    [SerializeField] private bool isMovable = true;

    private float health = 20f;
    private float attackDistance = 10f;
    private float fieldOfView = 180f;
    private float stoppingDistance = 4f;


    public abstract void Attack();
    public abstract void Die();


    public Vector3 DetectPlayer(){
        return new Vector3(0, 0, 0);
    }
    public void LosePlayer(){
    }

    public float GetHealth(){
        return this.health;
    }

    public void SetHealth(float health){
        this.health = health;
    }

    public void DecreaseHealth(float damage){
        if(this.health > 0){
            this.health -= damage;
        }
    }

    public float GetAttackDistance(){
        return this.attackDistance;
    }

    public void SetAttackDistance(float distance){
        this.attackDistance = distance;
    }

    public float GetFieldOfView(){
        return this.fieldOfView;
    }

    public void SetFieldOfView(float fieldOfView){
        this.fieldOfView = fieldOfView;
    }

    public float GetStoppingDistance(){
        return this.stoppingDistance;
    }

    public void SetStoppingDistance(float distance){
        this.stoppingDistance = distance;
    }

    public bool GetIsMovable(){
        return this.isMovable;
    }

    public void SetIsMovable(bool movable){
        this.isMovable = movable;
    }

}
