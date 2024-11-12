using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AK.Wwise;

public class SoundController : MonoBehaviour
{
    [Header("SoundBanks ")]
    //[SerializeField] private string mainBank = "Main";  // Name of the SoundBank to load
    //[SerializeField] private string dialogueBank = "MainDialogueBank"; 
   // [SerializeField] private string musicBank = "MainMusicBank"; 
    [SerializeField] private string soundFXBank = "MainSoundFXBank";        // Ambient event to play (set in the Inspector)

    [SerializeField] private AK.Wwise.Event backgroundMusicEvent; // Background music event to play

   /* private uint mainBankID;
    private uint dialogueBankID;
    private uint musicBankID;*/

    private uint soundFXBankID;

   
    void Awake()
    {
        /*AkSoundEngine.LoadBank(mainBank, out mainBankID);
        Debug.Log("Loaded Main Bank: " + mainBankID);

        AkSoundEngine.LoadBank(dialogueBank, out dialogueBankID);
        Debug.Log("Loaded Dialogue Bank: " + dialogueBankID);

        AkSoundEngine.LoadBank(musicBank, out musicBankID);
        Debug.Log("Loaded Music Bank: " + musicBankID);
*/
        AkSoundEngine.LoadBank(soundFXBank, out soundFXBankID);
        Debug.Log("Loaded Sound FX Bank: " + soundFXBankID);

        backgroundMusicEvent.Post(gameObject); // Post the music event to start playing
        Debug.Log("Background music started.");
        //ambienceEvent.Post(gameObject);

    }

    

    }
