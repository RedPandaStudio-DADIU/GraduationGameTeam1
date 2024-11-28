using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    private Dictionary<int, Sprite> taskDictionary = new Dictionary<int, Sprite>();

    [SerializeField] private Image taskDisplayUI; 
     //[SerializeField] private List<Sprite> taskSprites;
    private int currentTaskID = 0; 

    void Start()
    {
        taskDictionary.Add(0, Resources.Load<Sprite>("Assets/TaskImages/1.png"));
       taskDictionary.Add(0, Resources.Load<Sprite>("Assets/TaskImages/2.png"));
       taskDictionary.Add(0, Resources.Load<Sprite>("Assets/TaskImages/3.png"));
       taskDictionary.Add(0, Resources.Load<Sprite>("Assets/TaskImages/4.png"));
       
    
        
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
            taskDisplayUI.sprite = taskDictionary[currentTaskID];
        }
        else
        {
            taskDisplayUI.gameObject.SetActive(false); 
        }
    }  // void UpdateTaskUI()
    // {
    //     if (taskDictionary.ContainsKey(currentTaskID))
    //     {
    //         taskDisplayUI.gameObject.SetActive(true); 
    //         taskDisplayUI.sprite = taskDictionary[currentTaskID];
    //     }
    //     else
    //     {
    //         taskDisplayUI.gameObject.SetActive(false); 
    //     }
    // }

    //  void UpdateTaskUI()
    // {
    //     if (currentTaskID < taskSprites.Count && taskSprites[currentTaskID] != null)
    //     {
    //         taskDisplayUI.gameObject.SetActive(true); // 显示 UI
    //         taskDisplayUI.sprite = taskSprites[currentTaskID]; // 更新任务图像
    //     }
    //     else
    //     {
    //         taskDisplayUI.gameObject.SetActive(false); // 隐藏 UI
    //     }
    // }

    public void CompleteTask()
    {
        Debug.Log($"Task {currentTaskID} completed.");
        currentTaskID++; 

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

    public int GetCurrentTaskID()
{
    return currentTaskID;
}
}
