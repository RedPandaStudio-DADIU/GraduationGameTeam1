using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;
    // Level we are in
    [SerializeField] private AK.Wwise.State levelState;
    // State we are in - combat  non-combat
    [SerializeField] private AK.Wwise.State playerState;

    // combat intensity - only when in combat first - combat high
    [SerializeField] private AK.Wwise.State intensityState;
    private string playerStateStateGroup = "Player_state";
    private string playerLevelStateGroup = "Player_Level";
    private string combatIntensityStateGroup = "Combat_intensity_Lv1";


    // [SerializeField] private AK.Wwise.State combatState;
    // [SerializeField] private AK.Wwise.State nonCombatState;
    // [SerializeField] private AK.Wwise.State intensityLevelLowState;
    // [SerializeField] private AK.Wwise.State intensityLevelHighState;
    // [SerializeField] private AK.Wwise.State intensityNoneState;
    private string currentState = "No combat";

    void Start()
    {

        AkSoundEngine.SetState(playerLevelStateGroup, "Lv1");
        AkSoundEngine.SetState(playerStateStateGroup, "No_combat");


        // levelState.SetValue();
        // playerState.SetValue();

        // nonCombatState.SetValue();
        // intensityNoneState.SetValue();

        musicEvent.Post(gameObject);

    }

    void Update()
    {
        Debug.LogWarning("Player posiiton: " + GameObject.FindWithTag("Player").transform.position);
    }

    public void CheckIfSameState(string state){
        if (currentState == state) return;
        else {
            SetStateOfCombat(state);
        }
    }

    public string GetCurrentState(){
        return this.currentState;
    }

    public void SetStateOfCombat(string state){
        currentState = state;
        switch(state){
            case "Combat":
                AkSoundEngine.SetState(playerStateStateGroup, "In_combat");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 1 -  combat");
                break;
            case "CombatIntense":
                AkSoundEngine.SetState(playerStateStateGroup, "In_combat");
                AkSoundEngine.SetState(combatIntensityStateGroup, "Combat_high_Lv1");
                Debug.Log("Playing music level 2 - high combat");
                break;
            case "Control tower":
                AkSoundEngine.SetState(playerStateStateGroup, "Control_tower");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 3 - Control_tower");
                break;
            case "Elevator":
                AkSoundEngine.SetState(playerStateStateGroup, "Elevator");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 4 - Elevator");
                break;
            default:
                AkSoundEngine.SetState(playerStateStateGroup, "No_combat");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 0 - no combat");
                break;
        }
    }
}


/* In level 1:

*/


/* In level 2:
    2 stages:

    
    When you start - combat hall (from the very beginning) - no no-combat state
    Combat Xaga - it needs to be activated once we get to Xaga
*/

/* Level 2 ending:
    Loose vs. Win
*/

// 