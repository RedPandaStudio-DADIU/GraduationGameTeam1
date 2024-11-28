using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupManager : MonoBehaviour
{
   private Dictionary<int, Sprite> popUpDictionary = new Dictionary<int, Sprite>();

    [SerializeField] private Image popUpDisplayUI; 
     [SerializeField] private List<Sprite> popUpSprites;
    private int currentpopUpID = 0; 

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
        UpdatepopUpUI();
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
            Debug.Log("Task update.");
        }
        else
        {
            popUpDisplayUI.gameObject.SetActive(false); 
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

        public int GetCurrentpopUpID()
    {
        return currentpopUpID;
    }
}