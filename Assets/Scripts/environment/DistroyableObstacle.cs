using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistroyableObstacle : MonoBehaviour
{
    // Serialize Field private
    private int hitCount = 0; 
    [SerializeField] private int maxHits = 7;   
    [SerializeField] private float pushForce = 30f; 
    [SerializeField] private float objectMass = 10f;
    [SerializeField] private float objectDrag = 2f; 

    [SerializeField] private bool isExplosive = false;  
    [SerializeField] private float explosionRadius = 5f;  
    [SerializeField] private HitScanBasic hitScanBasic;

    void Start()
    {
        
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.mass = objectMass;    
        rb.drag = objectDrag;   
        rb.useGravity = true;

        hitScanBasic = FindObjectOfType<HitScanBasic>();


    }

    
    public void TakeHit(Vector3 hitDirection)
    {
        hitCount++;
        Debug.Log(gameObject.name + " has been hit " + hitCount + " times.");

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            
            rb.AddForce(hitDirection * pushForce, ForceMode.Impulse);
        }

        if (hitCount >= maxHits)
        {
            if (isExplosive)
            {
                Explode(); 
            }
            else
            {
                Destroy(gameObject); 
                Debug.Log(gameObject.name + " has been destroyed.");
            }
        }
    }

    private void Explode()
    {
        Debug.Log(gameObject.name + " exploded!");

        if (hitScanBasic != null)
        {
            hitScanBasic.PushNearbyEnemies(transform.position, pushForce, explosionRadius); 
        }
        
    }



}
