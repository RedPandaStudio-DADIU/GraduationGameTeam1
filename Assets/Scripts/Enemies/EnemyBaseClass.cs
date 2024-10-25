using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseClass : MonoBehaviour
{
    [SerializeField] private bool isMovable = true;

    // Remove serialize field after proper health damage done
    [SerializeField] private float health = 20f;

    private float attackDistance = 10f;
    private float fieldOfView = 180f;
    private float stoppingDistance = 4f;
    public float fallThreshold = -5f;

    private bool isRagdoll = false;
    private Rigidbody[] ragdollBodies;
    private Dictionary<Transform, BoneTransform> boneTransforms = new Dictionary<Transform, BoneTransform>();
   
    private Rigidbody torsoRigidbody;
    private Vector3 ragdollPosition; 
    private Quaternion ragdollRotation;

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

    public void RecordBoneTransforms()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            Transform bone = rb.transform;
            boneTransforms[bone] = new BoneTransform(bone.localPosition, bone.localRotation);
        }
        Debug.Log("Recorded bone transforms.");
    }
    private IEnumerator SmoothRecoverToStanding()
{
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

    private Collider mainCollider; 
    private Rigidbody mainRigidbody; 

    public Animator animator;


    public abstract void Attack();
    public abstract void Die();
    public abstract void LosePlayer(Vector3 playerPosition);



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

    public EnemyRagdollController GetRagdollController(){
        return this.GetComponent<EnemyRagdollController>();
    }


     void Start()
    {
        // ragdollBodies = GetComponentsInChildren<Rigidbody>();
        // mainCollider = GetComponent<Collider>(); 
        // mainRigidbody = GetComponent<Rigidbody>(); 

        
        // SetRagdollMode(false);
    }

    void Update()
    {
        
        // if (torsoRigidbody != null)
        // {
        //     if (torsoRigidbody.position.y < fallThreshold)
        //     {
                
        //         Destroy(gameObject);
        //         Debug.Log("Enemy fell off the platform and was destroyed.");
        //     }
        // }
    }

    // public void SetRagdollMode(bool isRagdollActive)
    // {
    //     isRagdoll = isRagdollActive;
    //      if (animator != null)
    //     {
    //         animator.enabled = !isRagdollActive; 
    //     }
    //     foreach (Rigidbody rb in ragdollBodies)
    //     {
    //         if (rb != null)
    //         {
    //             rb.isKinematic = !isRagdollActive; 
    //         }
        
    //     }

    //     UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    //     if (agent != null)
    //     {
    //         agent.enabled = !isRagdollActive;  
    //     }

    //     Debug.Log("Setting Ragdoll mode to: " + isRagdollActive);

    //     mainRigidbody.isKinematic = isRagdollActive;
    //     mainCollider.enabled = !isRagdollActive;
    // }

    // public void SwitchToRagdollAndApplyForce(Vector3 forceDirection, float force)
    // {
    //     Debug.Log("Switching to ragdoll and applying force. Force direction: " + forceDirection + ", Force: " + force);
        
    //     RecordBoneTransforms();

    //     SetRagdollMode(true); 

    //     // add the force
    //     if (torsoRigidbody == null)
    //     {
    //         torsoRigidbody = FindTorsoRigidbody();
    //     }
        
    //     if (torsoRigidbody != null)
    //     {
    //         torsoRigidbody.AddForce(forceDirection * force, ForceMode.Impulse);
    //         Debug.Log("Applied force to torso.");
    //     }
    //     else
    //     {
    //         Debug.LogError("Torso Rigidbody not found!");
    //     }
        

    // }

    // private Rigidbody FindTorsoRigidbody()
    // {
    //     Transform torsoTransform = transform.Find("Pelvis");
    //     return torsoTransform?.GetComponent<Rigidbody>();
    // }

    // public void RecoverFromRagdoll()
    // {
        
    //     SetRagdollMode(false);

    //     StartCoroutine(SmoothRecoverToStanding());
    //     Debug.Log("Recovering from ragdoll...");
    // }

     


}
