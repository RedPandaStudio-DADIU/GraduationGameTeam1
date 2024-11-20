using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        string videoPath;

        #if UNITY_EDITOR || UNITY_STANDALONE
            // Correct the file path for VideoPlayer
            videoPath = "file:///" + Application.streamingAssetsPath + "/Cutscene/Dream_6.mov";
        #elif UNITY_ANDROID
            videoPath = Application.streamingAssetsPath + "/Cutscene/Dream_6.mov"; // Android handles StreamingAssets differently
        #elif UNITY_IOS
            videoPath = Application.streamingAssetsPath + "/Cutscene/Dream_6.mov";
        #endif

        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}
