using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    [SerializeField] private int taskID; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            TaskManager taskManager = FindObjectOfType<TaskManager>();
             Debug.Log("Task at here.");
             if (taskManager = null ) // 确保任务顺序正确
            {
                
                Debug.Log("no Task manager.");
            }

            // if (taskManager != null && taskManager.GetCurrentTaskID() == taskID) // 确保任务顺序正确
             if (taskManager != null ) // 确保任务顺序正确
          
             {
                taskManager.CompleteTask(); 
                gameObject.SetActive(false); 
                Debug.Log("Task " + taskID + " completed.");
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
