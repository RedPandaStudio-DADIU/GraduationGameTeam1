using UnityEngine;
using System.Collections;
using AK.Wwise;

namespace cowsins
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;


        private AudioSource src;
        public AK.Wwise.Event defaultSoundEvent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.parent = null;
            }
            else Destroy(this.gameObject);

            src = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip, float delay, float pitchAdded, bool randomPitch, float spatialBlend)
        {
            if (clip == null) return;
            StartCoroutine(Play(clip, delay, pitchAdded, randomPitch, spatialBlend));
        }

        private IEnumerator Play(AudioClip clip, float delay, float pitch, bool randomPitch, float spatialBlend)
        {
            if (!clip) yield return null;
            yield return new WaitForSeconds(delay);
            src.spatialBlend = spatialBlend;
            float pitchAdded = randomPitch ? Random.Range(-pitch, pitch) : pitch;
            src.pitch = 1 + pitchAdded;
            src.PlayOneShot(clip);
            yield return null;
        }


        public void PlaySound(AK.Wwise.Event eventToPlay, float delay = 0f)
        {
            if (eventToPlay == null)
            {
                Debug.LogWarning("No Wwise event assigned.");
                return;
            }

            // if (delay > 0)
            // {
            //     StartCoroutine(PlayWithDelay(eventToPlay, delay));
            // }
            // else
            // {
                eventToPlay.Post(gameObject);
            //}
        }

        public void PlayDefaultSound(float delay = 0f)
        {
            PlaySound(defaultSoundEvent, delay);
        }

        // private IEnumerator PlayWwiseEventWithDelay(AK.Wwise.Event eventToPlay, float delay)
        // {
        //     yield return new WaitForSeconds(delay);
        //     eventToPlay.Post(gameObject);
        // }


    }
}

