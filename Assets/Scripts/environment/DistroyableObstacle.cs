using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistroyableObstacle : MonoBehaviour
{
    private int hitCount = 0; 
    public int maxHits = 3;   

    void Start()
    {
        
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in childTransforms)
        {
            
            if (child.gameObject.GetComponent<Rigidbody>() == null)
            {
                child.gameObject.AddComponent<Rigidbody>();
            }

            
            if (child.gameObject.GetComponent<DistroyableObstacle>() == null)
            {
                child.gameObject.AddComponent<DistroyableObstacle>();
            }
        }
    }

    
    public void TakeHit()
    {
        hitCount++;
        Debug.Log(gameObject.name + " has been hit " + hitCount + " times.");

        if (hitCount >= maxHits)
        {
           
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has been destroyed.");
        }
    }
}
