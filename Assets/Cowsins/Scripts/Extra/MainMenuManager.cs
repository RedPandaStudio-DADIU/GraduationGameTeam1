using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace cowsins
{
    public class MainMenuManager : MonoBehaviour
    {
        [System.Serializable]
        public class MainMenuSection
        {
            public string sectionName;
            public CanvasGroup section;
        }
        public static MainMenuManager Instance { get; private set; }

        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private string videoFileName ;


        [SerializeField] private AK.Wwise.Event cutsceneEvent; 

        [SerializeField] private AK.Wwise.Event musicEvent; 

        [SerializeField, Header("Sections")] private MainMenuSection[] mainMenuSections;

        private CanvasGroup objectToLerp;
        private int currentSectionIndex = 0;
        private uint musicEventId;

        //private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            mainMenuSections[0].section.gameObject.SetActive(true);
            //mainMenuSections[0].section.alpha = 1;
            mainMenuSections[2].section.gameObject.SetActive(false);


            // We want to skip the first item
            for (int i = 1; i < mainMenuSections.Length; i++)
            {
                mainMenuSections[i].section.gameObject.SetActive(false);
                //mainMenuSections[i].section.alpha = 0;
            }

            AkSoundEngine.SetState("Player_Level", "Menu");
            musicEventId = musicEvent.Post(this.gameObject);

            //audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // if (!objectToLerp || objectToLerp?.alpha >= 1) return;
            // objectToLerp.gameObject.SetActive(true);
            // objectToLerp.alpha += Time.deltaTime * 3;

            if (currentSectionIndex == 0 && Input.GetKeyDown(KeyCode.Space))
            {
                mainMenuSections[1].section.gameObject.SetActive(true);
                //mainMenuSections[1].section.alpha = 1;
                mainMenuSections[0].section.gameObject.SetActive(false);
                //mainMenuSections[0].section.alpha = 0;
                currentSectionIndex = 1;

            }

             if (mainMenuSections[3].section.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenuSections[3].section.gameObject.SetActive(false);
                Debug.Log("Back to main menu");
                currentSectionIndex = 1;
                mainMenuSections[1].section.gameObject.SetActive(true);
               

            }

            if (mainMenuSections[4].section.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenuSections[4].section.gameObject.SetActive(false);
                Debug.Log("Back to main menu");
                currentSectionIndex = 1;
                mainMenuSections[1].section.gameObject.SetActive(true);
               

            }
        }


        public void SetObjectToLerp(CanvasGroup To) => objectToLerp = To;

       public void ChangeScene(int scene) => SceneManager.LoadScene(scene);

        // public void PlaySound(AudioClip clickSFX)
        // {
        //     if (audioSource)
        //     {
        //         audioSource.clip = clickSFX;
        //         audioSource.Play();
        //     }
        // }

         public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
            DynamicGI.UpdateEnvironment();

        }

        public void LoadGameScene()
        {
            Debug.Log("LoadGameScene method called.");

            if (videoPlayer != null)
            {

                // videoPlayer.loopPointReached += OnVideoEnd;
                //  mainMenuSections[1].section.gameObject.SetActive(false);

                // videoPlayer.Play();
                // AkSoundEngine.StopPlayingID(musicEventId);
                AkSoundEngine.ExecuteActionOnPlayingID(AkActionOnEventType.AkActionOnEventType_Pause, musicEventId);

                mainMenuSections[2].section.gameObject.SetActive(true);

                // string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName).Replace("\\", "/");
                // string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

                #if UNITY_EDITOR || UNITY_STANDALONE
                    // Correct the file path for VideoPlayer
                    string videoPath = "file:///" + Application.streamingAssetsPath + "/Cutscene/Finished_Cutscene.mp4";
                    videoPlayer.url = videoPath;
                #elif UNITY_ANDROID
                    string videoPath = Application.streamingAssetsPath + "/Cutscene/Finished_Cutscene.mp4"; // Android handles StreamingAssets differently
                    videoPlayer.url = videoPath;
                #elif UNITY_IOS
                    string videoPath = Application.streamingAssetsPath + "/Cutscene/Finished_Cutscene.mp4";
                    videoPlayer.url = videoPath;
                #endif


                Debug.Log("Video path set to: " + videoPlayer.url);
                videoPlayer.loopPointReached += OnVideoEnd;
                 mainMenuSections[1].section.gameObject.SetActive(false);
                cutsceneEvent.Post(this.gameObject);
                videoPlayer.Play();
                Debug.Log("Is playing: " + videoPlayer.isPlaying);


                Debug.Log("Playing video before scene load.");
            }
            else
            {
                Debug.Log("LoadGameScene and we dont have video.");
                AkSoundEngine.ExecuteActionOnPlayingID( AkActionOnEventType.AkActionOnEventType_Resume, musicEventId);
                // musicEventId = musicEvent.Post(this.gameObject);
                SceneManager.LoadScene("Level 1 Design 1.1");
                DynamicGI.UpdateEnvironment();

            }


        }
         public void ShowCredits()
    {
        mainMenuSections[1].section.gameObject.SetActive(false);
        if (mainMenuSections[3].section.gameObject.activeSelf)
        {
            mainMenuSections[3].section.gameObject.SetActive(false);
        }
        else
        {
            mainMenuSections[3].section.gameObject.SetActive(true);
        }
    }

         public void ShowControl()
    {
        mainMenuSections[1].section.gameObject.SetActive(false);
        if (mainMenuSections[4].section.gameObject.activeSelf)
        {
            mainMenuSections[4].section.gameObject.SetActive(false);
        }
        else
        {
            mainMenuSections[4].section.gameObject.SetActive(true);
        }
    }



        private void OnVideoEnd(VideoPlayer vp)
        {
            Debug.Log("Video playback finished. Loading game scene.");
            DynamicGI.UpdateEnvironment();
            SceneManager.LoadScene("Level 1 Design 1.1");

            videoPlayer.loopPointReached -= OnVideoEnd;
        }

         public void QuitGame()
        {
            Debug.Log("QuitGame method called.");


#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else

            Application.Quit();
#endif
        }

    }
}