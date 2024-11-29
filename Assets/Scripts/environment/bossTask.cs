using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossTask : MonoBehaviour
{
   private Dictionary<int, Sprite> popUpDictionary = new Dictionary<int, Sprite>();

    [SerializeField] private Image popUpDisplayUI; 
     [SerializeField] private List<Sprite> popUpSprites;
      
    public int currentpopUpID = 0; 

    void Start()
    {
        
        for (int i = 0; i < popUpSprites.Count; i++)
        {
            if (popUpSprites[i] != null)
            {
                popUpDictionary.Add(i, popUpSprites[i]);
            }
            else
            {
                Debug.LogError($"pop {i} has a missing sprite in the list.");
            }
        }
        //UpdatepopUpUI();
        UpdatepopUpUI();
        popUpDisplayUI.gameObject.SetActive(false); 
        // Ensure the start door has a trigger collider and is set up properly
        // if (startDoor != null)
        // {
        //     Collider doorCollider = startDoor.GetComponent<Collider>();
        //     if (doorCollider != null && !doorCollider.isTrigger)
        //     {
        //         Debug.LogWarning("StartDoor collider should be set as a trigger.");
        //         doorCollider.isTrigger = true;
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatepopUpUI()
    {
        if (popUpDictionary.ContainsKey(currentpopUpID))
        {
            popUpDisplayUI.gameObject.SetActive(true); 
            popUpDisplayUI.sprite = popUpDictionary[currentpopUpID];
            
        }
        else
        {
            popUpDisplayUI.gameObject.SetActive(false);
            Debug.Log("No more popups to display."); 
        }
    }  

    

    public void CompletepopUp()
    {
        Debug.Log($"Task {currentpopUpID} completed.");
        currentpopUpID++; 

        if (currentpopUpID < popUpDictionary.Count)
        {
            UpdatepopUpUI(); 
        }
        else
        {
            Debug.Log("All tasks completed!");
            popUpDisplayUI.gameObject.SetActive(false); 
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (startDoor != null && other.CompareTag("Player")) 
    //     {
    //         Debug.Log("Player entered the start door trigger.");
            
            
    //         popUpDisplayUI.gameObject.SetActive(true);
    //         UpdatepopUpUI();

    //         // Optional: Trigger other effects for the door
    //         Debug.Log("Start door logic executed.");
    //     }
    // }

}