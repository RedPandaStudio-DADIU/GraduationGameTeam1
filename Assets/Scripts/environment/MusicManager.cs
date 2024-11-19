using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;
    void Start()
    {
        AkSoundEngine.SetSwitch("MainMusicSW", "Lv1_hangard", gameObject);
        AkSoundEngine.SetSwitch("Lv1_Combat", "Lv1_combat_intensity", gameObject);
        AkSoundEngine.SetSwitch("Lv1_combat_intensity", "Lv1_combat_low", gameObject);

        musicEvent.Post(gameObject);

    }

    void Update()
    {
        
    }
}
