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
    float transitionTime = 2.0f;  // 过渡时间
    float elapsedTime = 0.0f;

    // 在恢复过程中逐步调整每个骨骼的局部位置和旋转
    while (elapsedTime < transitionTime)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / transitionTime;

        foreach (var entry in boneTransforms)
        {
            Transform bone = entry.Key;
            BoneTransform savedTransform = entry.Value;

            // 插值恢复局部位置
            bone.localPosition = Vector3.Lerp(bone.localPosition, savedTransform.localPosition, t);
            // 插值恢复局部旋转
            bone.localRotation = Quaternion.Lerp(bone.localRotation, savedTransform.localRotation, t);
        }

        yield return null;  // 等待下一帧
    }

    // 确保最终完全恢复
    foreach (var entry in boneTransforms)
    {
        Transform bone = entry.Key;
        BoneTransform savedTransform = entry.Value;

        bone.localPosition = savedTransform.localPosition;
        bone.localRotation = savedTransform.localRotation;
    }

    Debug.Log("Smooth recovery to standing complete.");

    // 恢复完成后，重新启用 NavMeshAgent
    NavMeshAgent agent = GetComponent<NavMeshAgent>();
    if (agent != null)
    {
        agent.enabled = true;  // 启用 NavMeshAgent
    }
}

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

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = !isRagdollActive;  // 禁用或启用 NavMeshAgent
        }

        Debug.Log("Setting Ragdoll mode to: " + isRagdollActive);

        mainRigidbody.isKinematic = isRagdollActive;
        mainCollider.enabled = !isRagdollActive;
    }

    public void SwitchToRagdollAndApplyForce(Vector3 forceDirection, float force)
    {
        Debug.Log("Switching to ragdoll and applying force. Force direction: " + forceDirection + ", Force: " + force);
        
        RecordBoneTransforms();

        SetRagdollMode(true); 

        // add the force
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

    private Rigidbody FindTorsoRigidbody()
    {
        Transform torsoTransform = transform.Find("Pelvis");
        return torsoTransform?.GetComponent<Rigidbody>();
    }

    public void RecoverFromRagdoll()
    {
        
        SetRagdollMode(false);

        StartCoroutine(SmoothRecoverToStanding());
        Debug.Log("Recovering from ragdoll...");
    }

     


}
