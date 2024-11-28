using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTrigger : MonoBehaviour
{
    private int popID=2; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PopupManager popupManager = FindObjectOfType<PopupManager>();
             Debug.Log("pop3 at here.");
             if (popupManager ==null ) 
            {
                
                Debug.Log("no popup manager.");
            }
             else {
                popupManager.CompletepopUp(); 
                gameObject.SetActive(false); 
                Debug.Log("Task " + popID + " completed.");
            }
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
