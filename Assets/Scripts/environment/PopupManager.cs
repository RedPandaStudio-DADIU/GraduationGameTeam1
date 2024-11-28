using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupManager : MonoBehaviour
{
   private Dictionary<int, Sprite> popUpDictionary = new Dictionary<int, Sprite>();

    [SerializeField] private Image popUpDisplayUI; 
     [SerializeField] private List<Sprite> popUpSprites;
      [SerializeField] private GameObject pop4; // Represents task for popUp ID 4
    [SerializeField] private GameObject pop5;
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
        UpdatepopUpUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPopConditions();
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

    void CheckPopConditions()
    {
        if (currentpopUpID == 3 &&  !pop4.activeSelf)
        {
            Debug.Log("Task for Pop4 completed.");
            CompletepopUp();
        }
        else if (currentpopUpID == 4 &&  (pop5 == null || !pop5.activeSelf))
        {
            Debug.Log("Task for Pop5 completed.");
            CompletepopUp();
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