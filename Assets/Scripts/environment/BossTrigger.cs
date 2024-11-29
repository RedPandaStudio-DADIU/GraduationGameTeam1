using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossTrigger : MonoBehaviour
{
     [SerializeField] Image popUpDisplayUI; 
    // Start is called before the first frame update
    void Start()
    {
        popUpDisplayUI.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the door trigger.");
            popUpDisplayUI.gameObject.SetActive(true); 
        }
    }

}
