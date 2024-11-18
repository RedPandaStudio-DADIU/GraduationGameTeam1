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
    private Dictionary<Transform, BoneTransform> boneTransforms = new Dictionary<Transform, BoneTransform>();

    private Rigidbody torsoRigidbody;
    private Transform middleBodyBone; // Cached reference to the hips/pelvis bone
    private Collider mainCollider; 
    private Rigidbody mainRigidbody; 
    [SerializeField] private float fallThreshold = -5f;
    [SerializeField] private float standUpDuration = 1.5f; // Duration for stand-up transition
    [SerializeField] private float animatorEnableDelay = 0.5f; // Delay before enabling Animator
    [SerializeField] private LayerMask groundLayer; // Ground layer for raycast check

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

        // middleBodyBone = FindBoneByName("middle body");
        middleBodyBone = FindBoneByName("spine_02.x");

        SetRagdollActive(false);
        ActivateWeakSpotsIfExist();
    }

    void Start()
    {
        mainCollider = GetComponent<Collider>(); 
        mainRigidbody = GetComponent<Rigidbody>(); 

        RecordBoneTransforms();
    }

    private void ActivateWeakSpotsIfExist()
    {

        foreach (Collider collider in ragdollColliders)
        {
            if (collider.gameObject.CompareTag("WeakSpot"))
            {
                collider.enabled = true;
            }
        }
    }

    private Transform FindBoneByName(string boneName)
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name == boneName)
            {
                return child;
            }
        }
        Debug.LogWarning($"Bone with name '{boneName}' not found!");
        return null;
    }

    void Update()
    {
        if (torsoRigidbody != null && torsoRigidbody.position.y < fallThreshold)
        {
            Destroy(gameObject);
            Debug.Log("Enemy fell off the platform and was destroyed.");
        }
    }

    public void SetRagdollActive(bool isActive)
    {   
        if (animator != null)
        {
            animator.enabled = !isActive; 
        }

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

        isRagdoll = isActive;

        if (isActive)
        {
            RecordBoneTransforms(); // Record transforms when entering ragdoll state
        }
    }

    private void RecordBoneTransforms()
    {
        boneTransforms.Clear();
        foreach (var rb in ragdollRigidbodies)
        {
            Transform bone = rb.transform;
            boneTransforms[bone] = new BoneTransform(bone.localPosition, bone.localRotation);
        }
    }

    public void RecoverFromRagdoll()
    {
        SetRagdollActive(false);
        StartCoroutine(StandUpCoroutine());
    }

    private IEnumerator StandUpCoroutine()
    {
        // Align the main GameObject to the hips position after ragdoll
        AlignPositionToHips();

        // Smooth transition of bones to initial positions
        float elapsedTime = 0f;
        while (elapsedTime < standUpDuration)
        {
            foreach (var kvp in boneTransforms)
            {
                Transform bone = kvp.Key;
                BoneTransform originalTransform = kvp.Value;

                bone.localPosition = Vector3.Lerp(bone.localPosition, originalTransform.localPosition, elapsedTime / standUpDuration);
                bone.localRotation = Quaternion.Lerp(bone.localRotation, originalTransform.localRotation, elapsedTime / standUpDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var kvp in boneTransforms)
        {
            kvp.Key.localPosition = kvp.Value.localPosition;
            kvp.Key.localRotation = kvp.Value.localRotation;
        }


        yield return new WaitForSeconds(animatorEnableDelay);

        if (animator != null) animator.enabled = true;
        if (navMeshAgent != null) navMeshAgent.enabled = true;
    }

    private void AlignPositionToHips()
    {
        if (middleBodyBone == null) return; 

        Vector3 originalBonePosition = middleBodyBone.position;

        transform.position = originalBonePosition;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("No ground detected below the character.");
        }

        middleBodyBone.position = originalBonePosition;
    }

    private Rigidbody FindTorsoRigidbody()
    {
        // Transform torsoTransform = transform.Find("middle body");
        Transform torsoTransform = transform.Find("spine_02.x");
        return torsoTransform?.GetComponent<Rigidbody>();
    }

    Transform FindChildRecursive(Transform parent, string targetName)
    {
        if (parent.name == targetName)
        {
            return parent;
        }

        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, targetName);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void ApplyForce(Vector3 forceDirection, float force)
    {
        if (torsoRigidbody == null)
        {
            // torsoRigidbody = FindTorsoRigidbody();
            // torsoRigidbody = FindChildRecursive(transform, "middle body")?.GetComponent<Rigidbody>();
            torsoRigidbody = FindChildRecursive(transform, "spine_02.x")?.GetComponent<Rigidbody>();

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
