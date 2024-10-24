using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRagdollController : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Collider[] hitboxColliders;

    private Animator animator;
    private NavMeshAgent navMeshAgent; 

    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        hitboxColliders = GetComponents<Collider>();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>(); 

        SetRagdollActive(false);
    }
    public void SetRagdollActive(bool isActive)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !isActive;  
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = isActive;  
        }

        foreach (Collider hitbox in hitboxColliders)
        {
            hitbox.enabled = !isActive;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = !isActive;  
        }

        if (animator != null)
        {
            animator.enabled = !isActive;
        }
    }
}
