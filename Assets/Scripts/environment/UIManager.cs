using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startCanvas;    
    public GameObject mainMenuCanvas; 
    public GameObject pauseCanvas;   
     public GameObject creditsCanvas; 

    private bool isPaused = false; 

    void Start()
    {
        
        ShowStartCanvas();
    }

    void Update()
    {
        
        if (startCanvas.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ShowMainMenuCanvas();
        }

      
        if (Input.GetKeyDown(KeyCode.Escape) && !startCanvas.activeSelf && !mainMenuCanvas.activeSelf)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) &&creditsCanvas.activeSelf)
        {
             HideCredits(); 
        }

    }

    public void ShowStartCanvas()
    {
        startCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
         creditsCanvas.SetActive(false);
    }

    public void ShowMainMenuCanvas()
    {
        startCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void StartGame()
    {
        
        mainMenuCanvas.SetActive(false);
        ResumeGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsCanvas.SetActive(true); // 显示 Credits
        mainMenuCanvas.SetActive(false); // 确保主菜单被隐藏
        pauseCanvas.SetActive(false); 
        startCanvas.SetActive(false); 
    }

    public void HideCredits()
    {
        creditsCanvas.SetActive(false); // 隐藏 Credits
        mainMenuCanvas.SetActive(true); // 显示主菜单
    }

}
