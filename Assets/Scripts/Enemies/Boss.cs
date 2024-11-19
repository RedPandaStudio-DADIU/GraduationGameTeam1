using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;

public class Boss : DiplomatEnemy
{
    [SerializeField] private List<Transform> weakSpots = new List<Transform>();
    [SerializeField] private float explosionRange = 200f;
    [SerializeField] private float damageEpxlosion = 20f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject weakSpotDefeatedEffect;

    [SerializeField] private AK.Wwise.Event bossExplosionSoundEvent;
    [SerializeField] private AK.Wwise.Event bossChargeSoundEvent;


    private Queue<Transform> queueWeakSpots;
    private bool areWeakSpotsDefeated = false;


    void Awake()
    {
        this.SetHealth(200f);
        this.SetStoppingDistance(8f);
        queueWeakSpots = new Queue<Transform>(weakSpots);

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
        GameObject weakspotDefeatedVFX = Instantiate(weakSpotDefeatedEffect, weakspot.position, Quaternion.identity);

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

    public override AK.Wwise.Event GetChargeSound()
    {
        return this.bossChargeSoundEvent;
    }

}
