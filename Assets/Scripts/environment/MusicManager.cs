using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;
    [SerializeField] private AK.Wwise.State combatState;
    [SerializeField] private AK.Wwise.State nonCombatState;
    [SerializeField] private AK.Wwise.State levelState;
    [SerializeField] private AK.Wwise.State intensityLevelLowState;
    [SerializeField] private AK.Wwise.State intensityLevelHighState;
    [SerializeField] private AK.Wwise.State intensityNoneState;
    private int currentLevel = 0;

    void Start()
    {
        nonCombatState.SetValue();
        intensityNoneState.SetValue();
        levelState.SetValue();

        musicEvent.Post(gameObject);

    }

    void Update()
    {
        Debug.LogWarning("Player posiiton: " + GameObject.FindWithTag("Player").transform.position);
    }

    public void CheckIfSameState(int level){
        if (currentLevel == level) return;
        else {
            SetStateOfCombat(level);
        }
    }

    public void SetStateOfCombat(int level){
        currentLevel = level;
        switch(level){
            case 1:
                combatState.SetValue();
                intensityNoneState.SetValue();
                // intensityLevelLowState.SetValue();
                Debug.Log("Playing music level 1 - low combat");
                break;
            case 2:
                combatState.SetValue();
                intensityNoneState.SetValue();
                Debug.Log("Playing music level 2 - high combat");
                // intensityLevelHighState.SetValue();
                break;
            default:
                nonCombatState.SetValue();
                intensityNoneState.SetValue();
                Debug.Log("Playing music level 0 - no combat");

                break;

        }
    }
}
