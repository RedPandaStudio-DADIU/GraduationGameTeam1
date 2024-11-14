using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace cowsins
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject playerUI;
        [SerializeField] private bool disablePlayerUIWhilePaused;
        [SerializeField] private GameObject pauseMenuUI;
        //[SerializeField] private float fadeSpeed;

        public static PauseMenu Instance { get; private set; }

        /// <summary>
        /// Returns the Pause State of the game
        /// </summary>
        public static bool isPaused= false;

        [HideInInspector] public PlayerStats stats;

        public event Action OnPause;
        public event Action OnUnpause;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Initially, the game is not paused
            isPaused = false;
            pauseMenuUI.SetActive(false);
        }

        private void Update()
        {
            HandlePauseInput();
            // if (isPaused)
            // {
            //     HandlePause();
            //     Debug.Log("pause ui 1");
            // }
            // else
            // {
            //     HandleUnpause();
            // }
        }

        private void HandlePauseInput()
        {
            if (InputManager.pausing)
            {
                TogglePause();
            }
        }

        // private void HandlePause()
        // {
        //     // if (!menu.gameObject.activeSelf)
        //     // {
        //     //     menu.gameObject.SetActive(true);
        //     //     menu.alpha = 0;
        //     // }

        //     // menu.alpha = Mathf.Min(menu.alpha + Time.deltaTime * fadeSpeed, 1);

        //     // if (disablePlayerUIWhilePaused && !stats.isDead)
        //     // {
        //     //     playerUI.SetActive(false);
        //     // }

        //    // isPaused = true;
        //     Time.timeScale = 0f; 
        //     pauseMenuUI.SetActive(true); 
        //     Debug.Log("pause ui ");
       
        //     // Cursor.lockState = CursorLockMode.None;
        //     // Cursor.visible = true;

        //     // if (disablePlayerUIWhilePaused)
        //     // {
        //     //     playerUI.SetActive(false); 
        //     // }
        // }

        // private void HandleUnpause()
        // {
        //     // menu.alpha = Mathf.Max(menu.alpha - Time.deltaTime * fadeSpeed, 0);

        //     // if (menu.alpha <= 0)
        //     // {
        //     //     menu.gameObject.SetActive(false);
        //     // }

        //     // playerUI.SetActive(true);
        //    // isPaused = false;
        //     Time.timeScale = 1f; 
        //     pauseMenuUI.SetActive(false); 
        //     Debug.Log("un pause ui ");
        //     // Cursor.lockState = CursorLockMode.Locked;
        //     // Cursor.visible = false;

        //     // if (disablePlayerUIWhilePaused)
        //     // {
        //     //     playerUI.SetActive(true); // 显示玩家 UI
        //     // }

        // }

        // public void UnPause()
        // {
        //     isPaused = false;
        //     stats.CheckIfCanGrantControl();
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Cursor.visible = false;
        //     playerUI.SetActive(true);

        //     OnUnpause?.Invoke();
        // }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f; 
                
                pauseMenuUI.SetActive(true); 
                Debug.Log("pause ui ");
                stats.LoseControl();

                if (disablePlayerUIWhilePaused && !stats.isDead)
                {
                    playerUI.SetActive(false);
                }

                OnPause?.Invoke();
            }
            else
            {
                stats.CheckIfCanGrantControl();

                Time.timeScale = 1f; 
                pauseMenuUI.SetActive(false); 
                Debug.Log("un pause ui ");

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerUI.SetActive(true);

                OnUnpause?.Invoke();
            }
        }

         public void QuitToMainMenu()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(0); 
        }

        public void RestartGame()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }



    }
}
