using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistroyableObstacle : MonoBehaviour
{
    private int hitCount = 0; 
    public int maxHits = 3;   
    public float pushForce = 2f; 
    void Start()
    {
        
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in childTransforms)
        {
            
            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                 rb.useGravity = false; 
            }

            
            if (child.gameObject.GetComponent<DistroyableObstacle>() == null)
            {
                child.gameObject.AddComponent<DistroyableObstacle>();
            }
        }
    }

    
    public void TakeHit(Vector3 hitDirection)
    {
        hitCount++;
        Debug.Log(gameObject.name + " has been hit " + hitCount + " times.");

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
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
