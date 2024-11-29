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

        [SerializeField] private GameObject controlImageUI; // The image to show when the button is pressed
        private bool isShowing = false; 

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
            // DontDestroyOnLoad(gameObject);
            // DontDestroyOnLoad(playerUI);
            // DontDestroyOnLoad(pauseMenuUI);


        }

        private void Update()
        {
            HandlePauseInput();
             Debug.Log("UPDATE OF PAUSEMENU ");
            // if (isPaused)
            // {
            //     HandlePause();
            //     Debug.Log("pause ui 1");
            // }
            // else
            // {
            //     HandleUnpause();
            // }
            if (isShowing && InputManager.pausing)
            {
                isShowing = false;
                controlImageUI.SetActive(false); // Hide the info image
                pauseMenuUI.SetActive(true); // Show the pause menu
                isPaused=true;
            
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f; 
                
             
                Debug.Log("Suspending All Audio");
                AkSoundEngine.Suspend();
                Debug.Log("pause ui ");
                stats.LoseControl();

                if (disablePlayerUIWhilePaused && !stats.isDead)
                {
                    playerUI.SetActive(false);
                    
                    Debug.Log("Wake up All Audio");
                    AkSoundEngine.WakeupFromSuspend();
                    Debug.Log("hide user ui ");
                }


                //OnPause?.Invoke();
             }
        }

        private void HandlePauseInput()
        {
            Debug.Log("HandlePauseInput() ");
            if (InputManager.pausing)
            {
                 Debug.Log("HandlePauseInput() InputManager.pausing");
                TogglePause();
            }
        }
        public void ShowCreImage()
        {
            if (controlImageUI == null)
            {
                Debug.LogError("Info Image UI is not assigned.");
                return;
            }

            isShowing = true;
            controlImageUI.SetActive(true); // Show the image
            //pauseMenuUI.SetActive(false); // Hide the pause menu
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
             Debug.Log("in togglepause");

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f; 
                
                pauseMenuUI.SetActive(true); 
                Debug.Log("Suspending All Audio");
                AkSoundEngine.Suspend();
                Debug.Log("pause ui ");
                stats.LoseControl();

                if (disablePlayerUIWhilePaused && !stats.isDead)
                {
                    playerUI.SetActive(false);
                    
                    Debug.Log("Wake up All Audio");
                    AkSoundEngine.WakeupFromSuspend();
                    Debug.Log("hide user ui ");
                }


                OnPause?.Invoke();
            }
            else
            {
                stats.CheckIfCanGrantControl();

                Time.timeScale = 1f; 
                pauseMenuUI.SetActive(false); 
                
                Debug.Log("Wake up All Audio");
                AkSoundEngine.WakeupFromSuspend();
                Debug.Log("un pause ui ");

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playerUI.SetActive(true);

                OnUnpause?.Invoke();
            }
        }

         public void QuitToMainMenu()
        {
            isPaused = false;
            Time.timeScale = 1f; 
            // AkSoundEngine.StopAll();
            SceneManager.LoadScene(0); 
        }

        public void RestartGame()
        {
            Time.timeScale = 1f; 
            isPaused = false;
            AkSoundEngine.WakeupFromSuspend();
            // AkSoundEngine.StopAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        }



    }
}
