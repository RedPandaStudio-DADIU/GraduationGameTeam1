using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not used anywhere - can probably be deleted
public class AddBoxCollider : MonoBehaviour
{
    void Start()
    {
        
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform child in childTransforms)
        {
            
            if (child.gameObject.GetComponent<BoxCollider>() == null)
            {
                child.gameObject.AddComponent<BoxCollider>();
                Debug.Log("BoxCollider added to: " + child.name);
            }
        }
    }
}
