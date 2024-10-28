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
    private Dictionary<Transform, BoneTransform> boneTransformsAfter = new Dictionary<Transform, BoneTransform>();
    private Dictionary<Transform, Vector3> relativePositions = new Dictionary<Transform, Vector3>();

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

    public void RecordBoneTransforms(bool before){
        foreach (Rigidbody rb in ragdollBodies)
        {
            Transform boneRb = rb.transform;
            if (before){
                boneTransforms[boneRb] = new BoneTransform(boneRb.localPosition, boneRb.localRotation);
            } else {
                boneTransformsAfter[boneRb] = new BoneTransform(boneRb.localPosition, boneRb.localRotation);

            }


        }

        // Transform torso = null;

        // foreach (var entry in boneTransforms)
        // {
        //     if (entry.Key.name == "Torus") // Use your actual torso bone name here
        //     {
        //         torso = entry.Key;
        //         break;
        //     }
        // }

        // // Store the initial relative positions of the bones to the torso
        // foreach (var entry in boneTransforms)
        // {
        //     Transform bone = entry.Key;
        //     BoneTransform savedTransform = entry.Value;

        //     // Calculate the initial relative position of each bone to the torso
        //     Vector3 relativePosition = bone.position - torso.position; // Using global position for accuracy
        //     relativePositions[bone] = relativePosition;
        // }


        Debug.Log("Recorded bone transforms.");
    }

    private Vector3 ragdollFinalPosition;
    private Quaternion ragdollFinalRotation;

    public void CaptureRagdollFinalPosition(){
        ragdollFinalPosition = transform.position;
        ragdollFinalRotation = transform.rotation;
    }

    // private IEnumerator SmoothRecoverToStanding()
    // {
    //     float transitionTime = 2.0f;  
    //     float elapsedTime = 0.0f;

    //     // Assume you have captured the final position and rotation of the ragdoll
    //     Vector3 ragdollFinalPosition = transform.position; // Final position after ragdoll
    //     Quaternion ragdollFinalRotation = transform.rotation; // Final rotation after ragdoll

    //     // Find the torso transform
    //     Transform torso = null;

    //     foreach (var entry in boneTransforms)
    //     {
    //         if (entry.Key.name == "Torus") // Use your actual torso bone name here
    //         {
    //             torso = entry.Key;
    //             break;
    //         }
    //     }

    //     if (torso == null)
    //     {
    //         Debug.LogError("Torso transform not found in boneTransforms dictionary.");
    //         yield break; // Exit if the torso is not found
    //     }

    //     // // Store the initial relative positions of the bones to the torso
    //     // Dictionary<Transform, Vector3> relativePositions = new Dictionary<Transform, Vector3>();
    //     // foreach (var entry in boneTransforms)
    //     // {
    //     //     Transform bone = entry.Key;
    //     //     BoneTransform savedTransform = entry.Value;

    //     //     // Calculate the initial relative position of each bone to the torso
    //     //     Vector3 relativePosition = bone.position - torso.position; // Using global position for accuracy
    //     //     relativePositions[bone] = relativePosition;
    //     // }

    //     while (elapsedTime < transitionTime)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         float t = elapsedTime / transitionTime;

    //         // Set the torso to its final position smoothly
    //         torso.position = Vector3.Lerp(torso.position, ragdollFinalPosition, t);
    //         torso.rotation = Quaternion.Lerp(torso.rotation, ragdollFinalRotation, t);

    //         // Update the position of each bone based on the torso's new position
    //         foreach (var entry in boneTransforms)
    //         {
    //             Transform bone = entry.Key;
    //             BoneTransform savedTransform = entry.Value;

    //             // Calculate the new position based on the torso's position
    //             Vector3 targetPosition = torso.position + relativePositions[bone];
    //             bone.position = Vector3.Lerp(bone.position, targetPosition, t);

    //             // Smoothly transition to the saved local rotation
    //             bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
    //         }

    //         yield return null;  
    //     }

    //     // Final adjustments to ensure all bones are set correctly
    //     foreach (var entry in boneTransforms)
    //     {
    //         Transform bone = entry.Key;
    //         BoneTransform savedTransform = entry.Value;

    //         // Position bones based on the torso's final position + their relative offset
    //         bone.position = torso.position + relativePositions[bone];
    //         bone.localRotation = savedTransform.localRotation; // Set final local rotation
    //     }

    //     Debug.Log("Smooth recovery to standing complete.");

    //     // Enable NavMeshAgent if required
    //     UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //     if (agent != null)
    //     {
    //         agent.enabled = true;  
    //     }
    // }



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

                // bone.localPosition = Vector3.Lerp(bone.localPosition, savedTransform.localPosition, t);
                
                Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);
                bone.localPosition = Vector3.Lerp(bone.localPosition, startPosition, t);

                bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
            }

            yield return null;  
        }
    
        foreach (var entry in boneTransforms)
        {
            Transform bone = entry.Key;
            BoneTransform savedTransform = entry.Value;
            
            Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);
            bone.localPosition = startPosition;
            // bone.localPosition = savedTransform.localPosition;
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
