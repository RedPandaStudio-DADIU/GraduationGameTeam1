using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startCanvas;    
    public GameObject mainMenuCanvas; 
    public GameObject pauseCanvas;    

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
    }

    public void ShowStartCanvas()
    {
        startCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    public void ShowMainMenuCanvas()
    {
        startCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
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
}
