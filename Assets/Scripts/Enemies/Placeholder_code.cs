// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using UnityEngine.AI;

// // public class Placeholder : MonoBehaviour
// // {
// //     private Rigidbody[] ragdollRigidbodies;
// //     private Collider[] ragdollColliders;
// //     private Collider[] hitboxColliders;

// //     private Animator animator;
// //     private NavMeshAgent navMeshAgent; 

// //     private bool isRagdoll = false;
// //     private Rigidbody[] ragdollBodies;
// //     private Dictionary<Transform, BoneTransform> boneTransforms = new Dictionary<Transform, BoneTransform>();

// //     private Dictionary<Transform, RelativePosition> relativePositions = new Dictionary<Transform, RelativePosition>();

// //     private Rigidbody torsoRigidbody;
// //     private Vector3 ragdollPosition; 
// //     private Quaternion ragdollRotation;

// //     private Collider mainCollider; 
// //     private Rigidbody mainRigidbody; 
// //     [SerializeField] private float fallThreshold = -5f;


// //     private class BoneTransform
// //     {
// //         public Vector3 localPosition;
// //         public Quaternion localRotation;

// //         public BoneTransform(Vector3 position, Quaternion rotation)
// //         {
// //             localPosition = position;
// //             localRotation = rotation;
// //         }
// //     }


// //     private class RelativePosition
// //     {
// //         public float relativeX;
// //         public float relativeZ;

// //         public RelativePosition(float relativeX, float relativeZ)
// //         {
// //             relativeX = relativeX;
// //             relativeZ = relativeZ;
// //         }
// //     }


// //     void Awake()
// //     {
// //         ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
// //         ragdollColliders = GetComponentsInChildren<Collider>();

// //         hitboxColliders = GetComponents<Collider>();

// //         animator = GetComponent<Animator>();
// //         navMeshAgent = GetComponent<NavMeshAgent>(); 

// //         SetRagdollActive(false);
// //     }


// //     void Start(){
// //         ragdollBodies = GetComponentsInChildren<Rigidbody>();
// //         mainCollider = GetComponent<Collider>(); 
// //         mainRigidbody = GetComponent<Rigidbody>(); 
// //     }

// //     void Update(){
// //         if (torsoRigidbody != null)
// //         {
// //             if (torsoRigidbody.position.y < fallThreshold)
// //             {
                
// //                 Destroy(gameObject);
// //                 Debug.Log("Enemy fell off the platform and was destroyed.");
// //             }
// //         }
// //     }

// //     public void SetRagdollActive(bool isActive)
// //     {
// //         foreach (Rigidbody rb in ragdollRigidbodies)
// //         {
// //             rb.isKinematic = !isActive;  
// //         }

// //         foreach (Collider col in ragdollColliders)
// //         {
// //             col.enabled = isActive;  
// //         }

// //         foreach (Collider hitbox in hitboxColliders)
// //         {
// //             hitbox.enabled = !isActive;
// //         }

// //         if (navMeshAgent != null)
// //         {
// //             navMeshAgent.enabled = !isActive;  
// //         }

// //         if (animator != null)
// //         {
// //             animator.enabled = !isActive;
// //         }
// //     }

// //     public void RecordBoneTransforms(bool before){
// //         foreach (Rigidbody rb in ragdollBodies)
// //         {
// //             Transform boneRb = rb.transform;
// //             if (before){
// //                 boneTransforms[boneRb] = new BoneTransform(boneRb.localPosition, boneRb.localRotation);
// //             } 

// //         }

// //         Debug.Log("Recorded bone transforms.");
// //     }


// //     public void GetRelativePositions(){

// //         Transform torsoTransform = transform.Find("peovis");
// //         Debug.Log("Torso position BEFORE x: " + torsoTransform.position.x + ", z: " + torsoTransform.position.z);


// //         foreach (Rigidbody rb in ragdollBodies)
// //         {
// //             Transform boneRb = rb.transform;
            
// //             relativePositions[boneRb] = new RelativePosition(boneRb.position.x - torsoTransform.position.x, boneRb.position.z - torsoTransform.position.z);
// //         }

// //     }

// //     private Vector3 ragdollFinalPosition;
// //     private Quaternion ragdollFinalRotation;

// //     private Vector3 initialCenterOfMass;
// //     private Vector3 ragdollCenterOfMass;


// //     private RelativePosition GetNewPositionAfterRagdoll(Transform bone){
// //         float diffX = relativePositions[bone].relativeX;
// //         float diffZ = relativePositions[bone].relativeZ;

// //         Transform newTorsoTransform = transform.Find("peovis");
// //         Debug.Log("Torso position after ragdoll x: " + newTorsoTransform.position.x + ", z: " + newTorsoTransform.position.z);
// //         return new RelativePosition(diffX + newTorsoTransform.position.x, diffZ + newTorsoTransform.position.z);

// //     }

// //     private IEnumerator SmoothRecoverToStanding(){
// //         float transitionTime = 2.0f;  
// //         float elapsedTime = 0.0f;

// //         while (elapsedTime < transitionTime)
// //         {
// //             elapsedTime += Time.deltaTime;
// //             float t = elapsedTime / transitionTime;

// //             foreach (var entry in boneTransforms)
// //             {
// //                 Transform bone = entry.Key;
// //                 BoneTransform savedTransform = entry.Value;
                
// //                 RelativePosition locs = GetNewPositionAfterRagdoll(bone);

// //                 // Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);
// //                 // Vector3 startPosition = new Vector3(locs.relativeX, savedTransform.localPosition.y, locs.relativeZ);
// //                 Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);

// //                 bone.localPosition = Vector3.Lerp(bone.localPosition, startPosition, t);

// //                 bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
// //             }

// //             yield return null;  
// //         }


// //         Debug.Log("Smooth recovery to standing complete.");

        
// //         UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
// //         if (agent != null)
// //         {
// //             agent.enabled = true;  
// //         }
// //     }

// //     // private IEnumerator SmoothRecoverToStanding()
// //     // {
// //     //     float transitionTime = 2.0f;  
// //     //     float elapsedTime = 0.0f;

// //     //     // Calculate displacement in X and Z axes
// //     //     Vector3 displacementXZ = new Vector3(
// //     //         ragdollCenterOfMass.x - initialCenterOfMass.x,
// //     //         0, // Maintain the original Y position
// //     //         ragdollCenterOfMass.z - initialCenterOfMass.z
// //     //     );

// //     //     while (elapsedTime < transitionTime)
// //     //     {
// //     //         elapsedTime += Time.deltaTime;
// //     //         float t = elapsedTime / transitionTime;

// //     //         foreach (var entry in boneTransforms)
// //     //         {
// //     //             Transform bone = entry.Key;
// //     //             BoneTransform savedTransform = entry.Value;

// //     //             // Calculate target position with displacement applied in X and Z
// //     //             Vector3 targetPosition = savedTransform.localPosition + displacementXZ;

// //     //             // Interpolate position and rotation for a smooth transition
// //     //             bone.localPosition = Vector3.Lerp(bone.localPosition, targetPosition, t);
// //     //             bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
// //     //         }

// //     //         yield return null;  
// //     //     }

// //     //     // Final alignment to ensure all bones are set correctly
// //     //     foreach (var entry in boneTransforms)
// //     //     {
// //     //         Transform bone = entry.Key;
// //     //         BoneTransform savedTransform = entry.Value;

// //     //         bone.localPosition = savedTransform.localPosition + displacementXZ;
// //     //         bone.localRotation = savedTransform.localRotation;
// //     //     }

// //     //     Debug.Log("Smooth recovery to standing complete.");
// //     // }

// //     private Rigidbody FindTorsoRigidbody(){
// //         Transform torsoTransform = transform.Find("peovis");
// //         return torsoTransform?.GetComponent<Rigidbody>();
// //     }

// //     public void RecoverFromRagdoll(){
        
// //         // SetRagdollMode(false);
// //         SetRagdollActive(false);
// //         StartCoroutine(SmoothRecoverToStanding());
// //         Debug.Log("Recovering from ragdoll...");
// //     }

     
// //     public void ApplyForce(Vector3 forceDirection, float force){
        
// //         if (torsoRigidbody == null)
// //         {
// //             torsoRigidbody = FindTorsoRigidbody();
// //         }
        
// //         if (torsoRigidbody != null)
// //         {
// //             torsoRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
// //             Debug.Log("Applied force to torso.");
// //         }
// //         else
// //         {
// //             Debug.LogError("Torso Rigidbody not found!");
// //         }
// //     }


// // }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyRagdollController : MonoBehaviour
// {
//     private Rigidbody[] ragdollRigidbodies;
//     private Collider[] ragdollColliders;
//     private Collider[] hitboxColliders;

//     private Animator animator;
//     private NavMeshAgent navMeshAgent; 

//     private bool isRagdoll = false;
//     private Rigidbody[] ragdollBodies;
//     private Dictionary<Transform, BoneTransform> boneTransforms = new Dictionary<Transform, BoneTransform>();

//     // private Dictionary<Transform, RelativePosition> relativePositions = new Dictionary<Transform, RelativePosition>();

//     private Rigidbody torsoRigidbody;
//     private Vector3 ragdollPosition; 
//     private Quaternion ragdollRotation;
//     private Transform referenceBone; 

//     private Collider mainCollider; 
//     private Rigidbody mainRigidbody; 
//     [SerializeField] private float fallThreshold = -5f;


//     private class BoneTransform
//     {
//         public Vector3 localPosition;
//         public Quaternion localRotation;
//         public Vector3 worldPosition;
//         public Quaternion worldRotation;

//         public BoneTransform(Vector3 position, Quaternion rotation,  Vector3 worldPos, Quaternion worldRot)
//         {
//             localPosition = position;
//             localRotation = rotation;
//             worldPosition = worldPos;
//             worldRotation = worldRot;
//         }
//     }


//     void Awake()
//     {
//         ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
//         ragdollColliders = GetComponentsInChildren<Collider>();

//         hitboxColliders = GetComponents<Collider>();

//         animator = GetComponent<Animator>();
//         navMeshAgent = GetComponent<NavMeshAgent>(); 

//         SetRagdollActive(false);
//     }


//     void Start(){
//         ragdollBodies = GetComponentsInChildren<Rigidbody>();
//         mainCollider = GetComponent<Collider>(); 
//         mainRigidbody = GetComponent<Rigidbody>(); 

//         referenceBone = FindBoneByName("peovis");

//         RecordBoneTransforms();
//     }

//     private Transform FindBoneByName(string boneName)
//     {
//         foreach (Transform child in GetComponentsInChildren<Transform>())
//         {
//             if (child.name == boneName)
//             {
//                 return child;
//             }
//         }
//         Debug.LogWarning($"Bone with name '{boneName}' not found!");
//         return null;
//     }

//     void Update(){
//         if (torsoRigidbody != null)
//         {
//             if (torsoRigidbody.position.y < fallThreshold)
//             {
                
//                 Destroy(gameObject);
//                 Debug.Log("Enemy fell off the platform and was destroyed.");
//             }
//         }
//     }

//     public void SetRagdollActive(bool isActive)
//     {   
//         if (animator != null)
//         {
//             animator.enabled = !isActive; 
//         }

//         foreach (Rigidbody rb in ragdollRigidbodies)
//         {
//             rb.isKinematic = !isActive;  
//         }

//         foreach (Collider col in ragdollColliders)
//         {
//             col.enabled = isActive;  
//         }

//         foreach (Collider hitbox in hitboxColliders)
//         {
//             hitbox.enabled = !isActive;
//         }

//         if (navMeshAgent != null)
//         {
//             navMeshAgent.enabled = !isActive;  
//         }

//         if (animator != null)
//         {
//             animator.enabled = !isActive;
//         }
//     }

//     // public void RecordBoneTransforms(){
//     //     foreach (Rigidbody rb in ragdollBodies)
//     //     {
//     //         Transform boneRb = rb.transform;
//     //         boneTransforms[boneRb] = new BoneTransform(boneRb.localPosition, boneRb.localRotation);
            
//     //     }

//     //     Debug.Log("Recorded bone transforms.");
//     // }

//     // private void RecordBoneTransforms()
//     // {
//     //     boneTransforms.Clear();

//     //     foreach (var rb in ragdollBodies)
//     //     {
//     //         Transform bone = rb.transform;
//     //         // if (bone == referenceBone) continue; // Skip the reference bone

//     //         // Store local offset relative to reference bone
//     //         Vector3 localOffset = referenceBone.InverseTransformPoint(bone.position);
//     //         Quaternion localRotation = Quaternion.Inverse(referenceBone.rotation) * bone.rotation;
//     //         boneTransforms[bone] = new BoneTransform(localOffset, localRotation);
//     //     }
//     // }

//     // private void RecordBoneTransforms()
//     // {
//     //     boneTransforms.Clear();

//     //     foreach (var rb in ragdollBodies)
//     //     {
//     //         Transform bone = rb.transform;

//     //         // Store local offset relative to reference bone
//     //         Vector3 localOffset = referenceBone.InverseTransformPoint(bone.position);
//     //         Quaternion localRotation = Quaternion.Inverse(referenceBone.rotation) * bone.rotation;
//     //         boneTransforms[bone] = new BoneTransform(localOffset, localRotation, bone.position); // Store world position
//     //     }

//     //     // Store the reference bone's original position and rotation as well
//     //     Vector3 referenceWorldPosition = referenceBone.position; // Store the original world position
//     //     Vector3 referenceLocalOffset = Vector3.zero; // It's the reference point, so offset is zero
//     //     Quaternion referenceLocalRotation = Quaternion.identity; // No rotation change
//     //     boneTransforms[referenceBone] = new BoneTransform(referenceLocalOffset, referenceLocalRotation, referenceWorldPosition);
//     // }

//     private void RecordBoneTransforms()
//     {
//         boneTransforms.Clear();

//         foreach (var rb in ragdollBodies)
//         {
//             Transform bone = rb.transform;

//             // Store local offset relative to reference bone
//             Vector3 localOffset = referenceBone.InverseTransformPoint(bone.position);
//             Quaternion localRotation = Quaternion.Inverse(referenceBone.rotation) * bone.rotation;

//             // Store the world position and rotation of the bone
//             boneTransforms[bone] = new BoneTransform(localOffset, localRotation, bone.position, bone.rotation);
//         }

//         // Store the reference bone's original position and rotation as well
//         Vector3 referenceWorldPosition = referenceBone.position; // Store the original world position
//         Quaternion referenceWorldRotation = referenceBone.rotation; // Store the original world rotation
//         Vector3 referenceLocalOffset = Vector3.zero; // It's the reference point, so offset is zero
//         Quaternion referenceLocalRotation = Quaternion.identity; // No rotation change

//         // Save the reference bone's transforms
//         boneTransforms[referenceBone] = new BoneTransform(referenceLocalOffset, referenceLocalRotation, referenceWorldPosition, referenceWorldRotation);
//     }


//     private IEnumerator SmoothRecoverToStanding(){
//         float transitionTime = 2.0f;  
//         float elapsedTime = 0.0f;

//         while (elapsedTime < transitionTime)
//         {
//             elapsedTime += Time.deltaTime;
//             float t = elapsedTime / transitionTime;

//             foreach (var entry in boneTransforms)
//             {
//                 Transform bone = entry.Key;
//                 BoneTransform savedTransform = entry.Value;
                
//                 // RelativePosition locs = GetNewPositionAfterRagdoll(bone);

//                 // Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);
//                 // Vector3 startPosition = new Vector3(locs.relativeX, savedTransform.localPosition.y, locs.relativeZ);
//                 Vector3 startPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);

//                 bone.localPosition = Vector3.Lerp(bone.localPosition, startPosition, t);

//                 bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
//             }

//             yield return null;  
//         }


//         Debug.Log("Smooth recovery to standing complete.");

        
//         UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//         if (agent != null)
//         {
//             agent.enabled = true;  
//         }
//     }

//     private Rigidbody FindTorsoRigidbody(){
//         Transform torsoTransform = transform.Find("peovis");
//         return torsoTransform?.GetComponent<Rigidbody>();
//     }

//     // public void RecoverFromRagdoll(){
        
//     //     SetRagdollActive(false);
//     //     StartCoroutine(SmoothRecoverToStanding());
//     //     Debug.Log("Recovering from ragdoll...");
//     // }

//     // public void RecoverFromRagdoll(){
//     //     SetRagdollActive(false);
       
//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         bone.localPosition = new Vector3(bone.localPosition.x, savedTransform.localPosition.y, bone.localPosition.z);
//     //         bone.localRotation = savedTransform.localRotation;
//     //     }
//     // }

//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);

//     //     Vector3 landedPosition = referenceBone.position;

//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         // Apply the saved local offset relative to the current reference bone position
//     //         Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //         Quaternion targetRotation = referenceBone.rotation * savedTransform.localRotation;

//     //         bone.position = targetPosition;
//     //         bone.rotation = targetRotation;
//     //     }

//     //     // Finally, set the reference bone to an upright position
//     //     referenceBone.rotation = Quaternion.Euler(0, referenceBone.rotation.eulerAngles.y, 0);
//     // }


//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);
        
//     //     // Step 1: Get the current position of the reference bone
//     //     Vector3 currentReferencePosition = referenceBone.position;

//     //     // Step 2: Restore the Y position to the original height (assuming savedTransform holds the original Y value)
//     //     Vector3 newReferencePosition = new Vector3(currentReferencePosition.x, boneTransforms[referenceBone].localPosition.y, currentReferencePosition.z);
        
//     //     // Set the reference bone to the new Y position while keeping X and Z the same
//     //     referenceBone.position = newReferencePosition;

//     //     // Step 3: Align bones based on the new reference position
//     //     // foreach (var entry in boneTransforms)
//     //     // {
//     //     //     Transform bone = entry.Key;
//     //     //     BoneTransform savedTransform = entry.Value;

//     //     //     // Apply the saved local offset relative to the current reference bone position
//     //     //     Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //     //     Quaternion targetRotation = referenceBone.rotation * savedTransform.localRotation;

//     //     //     // Update the bone position and rotation
//     //     //     bone.position = targetPosition;
//     //     //     bone.rotation = targetRotation;
//     //     // }

//     //     // // Step 4: Finally, set the reference bone to an upright position
//     //     // referenceBone.rotation = Quaternion.Euler(0, referenceBone.rotation.eulerAngles.y, 0);
//     // }


//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);
        
//     //     // Step 1: Get the stored original world position of the reference bone
//     //     Vector3 originalReferenceWorldPosition = boneTransforms[referenceBone].worldPosition;

//     //     // Step 2: Restore the Y position to the original world height
//     //     Vector3 newReferencePosition = new Vector3(
//     //         originalReferenceWorldPosition.x, // Keep the original X position
//     //         originalReferenceWorldPosition.y, // Restore original Y position
//     //         originalReferenceWorldPosition.z  // Keep the original Z position
//     //     );

//     //     // Set the reference bone to the new position
//     //     referenceBone.position = newReferencePosition;

//     //     // Step 3: Align bones based on the new reference position
//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         // Apply the saved local offset relative to the current reference bone position
//     //         Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //         Quaternion targetRotation = referenceBone.rotation * savedTransform.localRotation;

//     //         // Update the bone position and rotation
//     //         bone.position = targetPosition;
//     //         bone.rotation = targetRotation;
//     //     }

//     //     // Step 4: Finally, set the reference bone to an upright position
//     //     referenceBone.rotation = Quaternion.Euler(0, referenceBone.rotation.eulerAngles.y, 0);
//     // }


//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);
        
//     //     // Step 1: Get the stored original world position and rotation of the reference bone
//     //     Vector3 originalReferenceWorldPosition = boneTransforms[referenceBone].worldPosition;
//     //     Quaternion originalReferenceWorldRotation = boneTransforms[referenceBone].worldRotation;

//     //     // Step 2: Restore the Y position to the original world height
//     //     Vector3 newReferencePosition = new Vector3(
//     //         originalReferenceWorldPosition.x, // Keep the original X position
//     //         originalReferenceWorldPosition.y, // Restore original Y position
//     //         originalReferenceWorldPosition.z  // Keep the original Z position
//     //     );

//     //     // Set the reference bone to the new position
//     //     referenceBone.position = newReferencePosition;

//     //             // Step 4: Finally, set the reference bone to the upright position using the original world rotation
//     //     referenceBone.rotation = originalReferenceWorldRotation; // Use the saved world rotation

//     //     // Step 3: Align bones based on the new reference position and rotation
//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         // Apply the saved local offset relative to the current reference bone position
//     //         Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //         Quaternion targetRotation = referenceBone.rotation * savedTransform.localRotation;

//     //         // Update the bone position and rotation
//     //         bone.position = targetPosition;
//     //         bone.rotation = targetRotation;
//     //     }


//     // }

//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);
        
//     //     // Step 1: Get the current position and rotation of the reference bone
//     //     Vector3 landedReferencePosition = referenceBone.position;
//     //     Quaternion landedReferenceRotation = referenceBone.rotation;

//     //     // Step 2: Align the reference bone to be upright but maintain its current position
//     //     // Here, we set it to an upright orientation but preserve the y-axis rotation
//     //     Quaternion uprightRotation = Quaternion.Euler(0, landedReferenceRotation.eulerAngles.y, 0);
//     //     referenceBone.rotation = uprightRotation;

//     //     // Step 3: Align bones based on the new reference position and upright rotation
//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         // Apply the saved local offset relative to the new reference bone position
//     //         Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //         Quaternion targetRotation = uprightRotation * savedTransform.localRotation;

//     //         // Update the bone position and rotation to be upright at the new location
//     //         bone.position = targetPosition;
//     //         bone.rotation = targetRotation;
//     //     }
//     // }

//     // public void RecoverFromRagdoll()
//     // {
//     //     SetRagdollActive(false);
        
//     //     // Step 1: Get the current landing position and rotation of the reference bone
//     //     Vector3 landedReferencePosition = referenceBone.position;
//     //     Quaternion landedReferenceRotation = referenceBone.rotation;

//     //     // Step 2: Set the reference bone to an upright rotation, preserving the Y-axis rotation
//     //     Quaternion uprightRotation = Quaternion.Euler(0, landedReferenceRotation.eulerAngles.y, 0);
//     //     referenceBone.rotation = uprightRotation;

//     //     // Step 3: Calculate the offset needed to bring the reference bone to ground level
//     //     float yOffset = referenceBone.position.y - landedReferencePosition.y;

//     //     // Step 4: Adjust the reference bone to ground level
//     //     Vector3 groundedPosition = new Vector3(landedReferencePosition.x, landedReferencePosition.y - yOffset, landedReferencePosition.z);
//     //     referenceBone.position = groundedPosition;

//     //     // Step 5: Adjust bones relative to the new reference position and upright orientation
//     //     foreach (var entry in boneTransforms)
//     //     {
//     //         Transform bone = entry.Key;
//     //         BoneTransform savedTransform = entry.Value;

//     //         // Apply the saved local offset relative to the new grounded reference bone position
//     //         Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
//     //         Quaternion targetRotation = uprightRotation * savedTransform.localRotation;

//     //         // Update the bone position and rotation to be upright at the grounded position
//     //         bone.position = targetPosition;
//     //         bone.rotation = targetRotation;
//     //     }
//     // }

// //     public void RecoverFromRagdoll()
// //     {
// //         SetRagdollActive(false);
        
// //         // Step 1: Get the current landing position and rotation of the reference bone
// //         Vector3 landedReferencePosition = referenceBone.position;
// //         Quaternion landedReferenceRotation = referenceBone.rotation;

// //         // Step 2: Create an upright rotation while preserving the Y-axis rotation
// //         Quaternion uprightRotation = Quaternion.Euler(0, landedReferenceRotation.eulerAngles.y, 0);
// //         referenceBone.rotation = uprightRotation;

// //         // Step 3: Position bones relative to the newly upright reference bone
// //         foreach (var entry in boneTransforms)
// //         {
// //             Transform bone = entry.Key;
// //             BoneTransform savedTransform = entry.Value;

// //             // Calculate target position and rotation based on the upright reference bone
// //             Vector3 targetPosition = referenceBone.TransformPoint(savedTransform.localPosition);
// //             Quaternion targetRotation = uprightRotation * savedTransform.localRotation;

// //             // Update bone to target position and rotation
// //             bone.position = targetPosition;
// //             bone.rotation = targetRotation;
// //         }

// //         // Step 4: Move the main collider to the new position and rotation of the reference bone
// //         if (mainCollider != null)
// //         {
// //             mainCollider.transform.position = referenceBone.position;
// //             mainCollider.transform.rotation = uprightRotation;
// //         }
// //     }


// //     public void ApplyForce(Vector3 forceDirection, float force){
        
// //         if (torsoRigidbody == null)
// //         {
// //             torsoRigidbody = FindTorsoRigidbody();
// //         }
        
// //         if (torsoRigidbody != null)
// //         {
// //             torsoRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
// //             Debug.Log("Applied force to torso.");
// //         }
// //         else
// //         {
// //             Debug.LogError("Torso Rigidbody not found!");
// //         }
// //     }


// // }
