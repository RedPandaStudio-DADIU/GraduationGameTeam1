using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public abstract class EnemyBaseClass : MonoBehaviour
{
    [SerializeField] private bool isMovable = true;

    // Remove serialize field after proper health damage done
    [SerializeField] private float health = 80f;

    private float attackDistance = 10f;
    private float fieldOfView = 180f;
    [SerializeField] private float stoppingDistance = 4f;
    
    public Animator animator;

    // [Header("Sound Events")]
    // [SerializeField] private AK.Wwise.Event hitSoundEvent;
    //[SerializeField] private AK.Wwise.Event deathSoundEvent;

    public abstract void Attack();
    public abstract void Die();
    public abstract void LosePlayer(Vector3 playerPosition);
    public abstract AK.Wwise.Event GetDamageSound();

    
    public virtual float GetMaxHealth(){
        return 0f;
    }

    public virtual void SpecialAttack(Transform playe){
        Debug.Log("Inside Special Attack");
    }

    public virtual AK.Wwise.Event GetChargeSound(){
        return null;
    }
    
    public virtual AK.Wwise.Event GetSpecialAttackSound(){
        return null;
    }

    public Vector3 DetectPlayer(){
        return new Vector3(0, 0, 0);
    }

    public float GetHealth(){
        return this.health;
    }

    public void SetHealth(float health){
        this.health = health;
    }

    public void DecreaseHealth(float damage){
        if(this.health > 0){
            if(damage > this.health){
                SetHealth(0f);
            } else {
                this.health -= damage;
            }
            
        }

        // if (hitSoundEvent != null)
        // {
        //     hitSoundEvent.Post(gameObject);
        //     Debug.Log("Played hit sound for enemy.");
        // }

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

    public EnemyRagdollController GetRagdollController(){
        return this.GetComponent<EnemyRagdollController>();
    }

    public void SetSwitchValue(string switchGroup, string switchValue){
        AkSoundEngine.SetSwitch(switchGroup, switchValue, this.gameObject);
    }

}
