using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private Dictionary<int, Sprite> taskDictionary = new Dictionary<int, Sprite>();

    [SerializeField] private Image taskDisplayUI; 
    private int currentTaskID = 0; 

    void Start()
    {
        
        taskDictionary.Add(0, Resources.Load<Sprite>("TaskImages/1")); 
        taskDictionary.Add(1, Resources.Load<Sprite>("TaskImages/2")); 
        taskDictionary.Add(2, Resources.Load<Sprite>("TaskImages/3")); 
        taskDictionary.Add(3, Resources.Load<Sprite>("TaskImages/4")); 

        // 显示初始任务
        UpdateTaskUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTaskUI()
    {
        if (taskDictionary.ContainsKey(currentTaskID))
        {
            taskDisplayUI.gameObject.SetActive(true); 
        }
        else
        {
            taskDisplayUI.gameObject.SetActive(false); 
        }
    }

    public void CompleteTask()
    {
        Debug.Log($"Task {currentTaskID} completed.");
        currentTaskID++; /

        if (currentTaskID < taskDictionary.Count)
        {
            UpdateTaskUI(); 
        }
        else
        {
            Debug.Log("All tasks completed!");
            taskDisplayUI.gameObject.SetActive(false); 
        }
    }
}
