using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{

    private float health = 20f;
    private float attackDistance = 10f;
    private float fieldOfView = 180f;
    private float stoppingDistance = 4f;


    private bool isRagdoll = false;
    private Rigidbody[] ragdollBodies;
    private Rigidbody torsoRigidbody;

    private Collider mainCollider; 
    private Rigidbody mainRigidbody; 

    public Animator animator;


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
        this.health -= damage;
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



     void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        mainCollider = GetComponent<Collider>(); 
        mainRigidbody = GetComponent<Rigidbody>(); 

        
        SetRagdollMode(false);
    }

    public void SetRagdollMode(bool isRagdollActive)
    {
        isRagdoll = isRagdollActive;
        animator.enabled = !isRagdollActive; 

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !isRagdollActive; // If isRagdollActive is true, make the rigidbody kinematic (not affected by physics)
        }

        mainRigidbody.isKinematic = isRagdollActive;
        mainCollider.enabled = !isRagdollActive;
    }

    public void SwitchToRagdollAndApplyForce(Vector3 forceDirection, float force)
    {
        SetRagdollMode(true); 

        // add the force
        if (torsoRigidbody == null)
        {
            torsoRigidbody = FindTorsoRigidbody();
        }
        
        if (torsoRigidbody != null)
        {
            torsoRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
        }
    }

    private Rigidbody FindTorsoRigidbody()
    {
        Transform torsoTransform = transform.Find("Pelvis");
        return torsoTransform?.GetComponent<Rigidbody>();
    }



}
