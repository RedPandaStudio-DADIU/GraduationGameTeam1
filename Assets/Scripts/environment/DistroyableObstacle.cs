using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistroyableObstacle : MonoBehaviour
{
    private int hitCount = 0; 
    public int maxHits = 7;   
    public float pushForce = 30f; 
    public float objectMass = 10f;
    public float objectDrag = 2f; 

    public bool isExplosive = false;  
    public float explosionRadius = 5f;  
    public HitScanBasic hitScanBasic;

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
        yield return new WaitForSeconds(3f);
        Destroy(gameObject); 

        if (hitScanBasic != null)
        {
            hitScanBasic.PushNearbyEnemies(transform.position, pushForce, explosionRadius); 
        }
        
    }



}
