using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistroyableObstacle : MonoBehaviour
{
    private int hitCount = 0; 
    public int maxHits = 3;   
    public float pushForce = 20f; 
    public float objectMass = 10f;
    public float objectDrag = 2f; 
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
           
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has been destroyed.");
        }
    }
}
