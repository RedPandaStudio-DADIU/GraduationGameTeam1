using UnityEngine;
using UnityEngine.Playables;

public class TriggerTimeline : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector; 
    [SerializeField] private PlayableAsset timelineAsset;  

    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            playableDirector.Play(timelineAsset);
            hasTriggered = true; 
        }
    }
}
