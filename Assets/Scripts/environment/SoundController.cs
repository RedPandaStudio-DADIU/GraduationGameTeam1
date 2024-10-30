using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AK.Wwise;

public class SoundController : MonoBehaviour
{
    [Header("SoundBank and Event")]
    [SerializeField] private string soundBank = "soundbank_MAIN";  // Name of the SoundBank to load
    [SerializeField] private AK.Wwise.Event ambienceEvent;         // Ambient event to play (set in the Inspector)

    private uint bankID;

    void Start()
    {
        LoadSoundBank();
        PostAmbienceEvent();
    }

    // Method to load the specified SoundBank
    private void LoadSoundBank()
    {
        var result = AkSoundEngine.LoadBank(soundBank, AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
        if (result == AKRESULT.AK_Success)
        {
            Debug.Log("SoundBank loaded successfully: " + soundBank);
        }
        else
        {
            Debug.LogWarning("Failed to load SoundBank: " + soundBank);
        }
    }

    // Method to post the ambience event
    private void PostAmbienceEvent()
    {
        if (ambienceEvent != null)
        {
            ambienceEvent.Post(gameObject);
            Debug.Log("Ambience event posted.");
        }
        else
        {
            Debug.LogWarning("No ambience event assigned.");
        }
    }

    }
