using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMusicControlRoom : MonoBehaviour
{
    private MusicManager musicManager;
    [SerializeField] private string stateName;

    void Start(){
        musicManager = GameObject.Find("GeneralSoundAmbience").GetComponent<MusicManager>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            musicManager.CheckIfSameState(stateName);
        }   
    }
}
