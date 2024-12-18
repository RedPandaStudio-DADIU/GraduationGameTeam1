using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using cowsins;

public class Boss : DiplomatEnemy
{
    [SerializeField] private List<Transform> weakSpots = new List<Transform>();
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private float explosionRange = 20f;
    [SerializeField] private float damageEpxlosion = 20f;
    [SerializeField] private GameObject attackPreparationEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject weakSpotDefeatedEffect;

    [SerializeField] private AK.Wwise.Event bossExplosionSoundEvent;
    [SerializeField] private AK.Wwise.Event bossChargeSoundEvent;

    [SerializeField] private AK.Wwise.Event dialogueXaga;
    [SerializeField] private AK.Wwise.Event specialChargeXaga;

    [SerializeField] private AK.Wwise.State lostArm;
    [SerializeField] private AK.Wwise.State lostLeg;
    [SerializeField] private AK.Wwise.State lostStomach;

    [SerializeField] private AK.Wwise.State nearDeath;
    [SerializeField] private AK.Wwise.State Dead;
    [SerializeField] private AK.Wwise.State firstLine;

    private Queue<Transform> queueWeakSpots;
    private Queue<Material> queueMaterials;

    private Queue<AK.Wwise.State> xagaStates = new Queue<AK.Wwise.State>();

    private bool areWeakSpotsDefeated = false;
    private Renderer renderer;
    private GameObject instantiatedExplosion;
    private float maxHealth;
    private bool saidInitialLine = false;

    void Awake()
    {
        this.SetHealth(400f);
        this.SetStoppingDistance(8f);
        this.SetAttackDistance(20f);
        queueWeakSpots = new Queue<Transform>(weakSpots);
        queueMaterials = new Queue<Material>(materials);
        renderer = transform.Find("Xaga").GetComponent<Renderer>();
        maxHealth = this.GetHealth();
    }

    void Start(){
        xagaStates.Enqueue(lostArm);
        xagaStates.Enqueue(lostLeg);
        xagaStates.Enqueue(lostStomach);
        dialogueXaga.Post(gameObject);
    }

    public bool CheckIfOnTop(Transform weakspot)
    {
        if (queueWeakSpots.Count > 0 && queueWeakSpots.Peek() == weakspot)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAreWeakSpotsDefeated(bool value)
    {
        this.areWeakSpotsDefeated = value;
    }

    public bool GetAreWeakSpotsDefeated()
    {
        return this.areWeakSpotsDefeated;
    }

    public void RemoveFromQueue()
    {
        Transform weakspot = queueWeakSpots.Dequeue();
        AK.Wwise.State state = xagaStates.Dequeue();
        state.SetValue();
        Material material = queueMaterials.Dequeue();
        this.renderer.material = material;
        GameObject weakspotDefeatedVFX = Instantiate(weakSpotDefeatedEffect, weakspot.position, Quaternion.identity);
        StartCoroutine(PlayAndStop(weakspotDefeatedVFX));
    }

    private IEnumerator PlayAndStop(GameObject effect)
    {
       
        yield return new WaitForSeconds(2f);
        Destroy(effect);

    }
    

    public bool CheckIfEmpty()
    {
        if (queueWeakSpots.Count == 0)
        {
            return true;
        }
        return false;
    }

    public override void SpecialAttack(Transform player)
    {
        Debug.Log("Entered special attack");
        // LayerMask obstacleLayer = LayerMask.GetMask("Player");
        LayerMask obstacleLayer = LayerMask.GetMask("Default", "Player", "Obstacle");

        GameObject explosion = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        explosion.GetComponent<VisualEffect>().Play();
        if (bossExplosionSoundEvent != null)
        {
            bossExplosionSoundEvent.Post(gameObject);
        }


        float distanceToPlayer = Vector3.Distance(player.position, this.transform.position);
        if (distanceToPlayer <= explosionRange)
        {
            Debug.Log("Player hit - distance check!");

            Vector3 directionToPlayer = (player.position - this.transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, directionToPlayer, out hit, explosionRange, obstacleLayer))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    player.GetComponent<PlayerStats>().Damage(damageEpxlosion, false);
                    Debug.Log("Player hit - explosion boss!");
                }
                else
                {
                    Debug.Log("Player hidden");
                }
            }
        }
    }

    public float GetMaxHealth(){
        return this.maxHealth;
    }

    public override AK.Wwise.Event GetChargeSound()
    {
        return this.bossChargeSoundEvent;
    }

    public override AK.Wwise.Event GetSpecialAttackSound()
    {
        return this.specialChargeXaga;
    }


    public GameObject GetAttackPrepVFX()
    {
        return this.attackPreparationEffect;
    }

    public void InstantiateAttackPrepVFX()
    {
        // instantiatedExplosion = Instantiate(attackPreparationEffect, transform.position, Quaternion.identity);
        attackPreparationEffect.SetActive(true);
        attackPreparationEffect.GetComponent<VisualEffect>().Play();
    }

    public void StopAttackPrepVFX()
    {
        // instantiatedExplosion.GetComponent<VisualEffect>().Stop();
        // Destroy(instantiatedExplosion);
        attackPreparationEffect.SetActive(false);
        attackPreparationEffect.GetComponent<VisualEffect>().Stop();

    }

    public void PlayHitOrDeadSound(bool isDead){
        if (isDead){
            Dead.SetValue();
        } else{
            nearDeath.SetValue();
        }
    }

    public void PlayHitSound(){
        base.GetDamageSound().Post(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    public void SetSaidFirstLine(){
        this.saidInitialLine = true;
    }

    public bool GetSaidFirstLine(){
        return this.saidInitialLine;
    }

    public void PostInitialEvent(){
        firstLine.SetValue();
    }

}
