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

    private bool isRagdoll = false;
    private Rigidbody[] ragdollBodies;
    private Dictionary<Transform, BoneTransform> boneTransforms = new Dictionary<Transform, BoneTransform>();
   
    private Rigidbody torsoRigidbody;
    private Vector3 ragdollPosition; 
    private Quaternion ragdollRotation;

    private Collider mainCollider; 
    private Rigidbody mainRigidbody; 
    public float fallThreshold = -5f;


    private class BoneTransform
    {
        public Vector3 localPosition;
        public Quaternion localRotation;

        public BoneTransform(Vector3 position, Quaternion rotation)
        {
            localPosition = position;
            localRotation = rotation;
        }
    }


    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        hitboxColliders = GetComponents<Collider>();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>(); 

        SetRagdollActive(false);
    }


    void Start(){
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        mainCollider = GetComponent<Collider>(); 
        mainRigidbody = GetComponent<Rigidbody>(); 
    }

    void Update(){
        if (torsoRigidbody != null)
        {
            if (torsoRigidbody.position.y < fallThreshold)
            {
                
                Destroy(gameObject);
                Debug.Log("Enemy fell off the platform and was destroyed.");
            }
        }
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

    public void RecordBoneTransforms(){
        foreach (Rigidbody rb in ragdollBodies)
        {
            Transform bone = rb.transform;
            boneTransforms[bone] = new BoneTransform(bone.localPosition, bone.localRotation);
        }
        Debug.Log("Recorded bone transforms.");
    }

    private IEnumerator SmoothRecoverToStanding(){
        float transitionTime = 2.0f;  
        float elapsedTime = 0.0f;


        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;

            foreach (var entry in boneTransforms)
            {
                Transform bone = entry.Key;
                BoneTransform savedTransform = entry.Value;

            
                bone.localPosition = Vector3.Lerp(bone.localPosition, savedTransform.localPosition, t);
            
                bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
            }

            yield return null;  
        }

        transform.position += finalPositionOffset;
        transform.rotation *= finalRotationOffset;
    
        foreach (var entry in boneTransforms)
        {
            Transform bone = entry.Key;
            BoneTransform savedTransform = entry.Value;

            bone.localPosition = savedTransform.localPosition;
            bone.localRotation = savedTransform.localRotation;
        }

        Debug.Log("Smooth recovery to standing complete.");

        
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;  
        }
    }


    private Rigidbody FindTorsoRigidbody(){
        Transform torsoTransform = transform.Find("peovis");
        return torsoTransform?.GetComponent<Rigidbody>();
    }

    public void RecoverFromRagdoll(){
        
        // SetRagdollMode(false);
        SetRagdollActive(false);
        StartCoroutine(SmoothRecoverToStanding());
        Debug.Log("Recovering from ragdoll...");
    }

     
    public void ApplyForce(Vector3 forceDirection, float force){
        
        if (torsoRigidbody == null)
            {
                torsoRigidbody = FindTorsoRigidbody();
            }
            
            if (torsoRigidbody != null)
            {
                torsoRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
                Debug.Log("Applied force to torso.");
            }
            else
            {
                Debug.LogError("Torso Rigidbody not found!");
            }
        }



}
