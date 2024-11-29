using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using UnityEngine.SceneManagement;


public class MusicManager : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;

    private string playerStateStateGroup = "Player_state";
    private string playerLevelStateGroup = "Player_Level";
    private string combatIntensityStateGroup = "Combat_intensity_Lv1";
    private string level2CombatStateGroup = "Combat_Lv2";
    private string level2EndingStateGroup = "Lv2_ending";

    private string currentState = "No combat";

    void Start()
    {


        if(SceneManager.GetActiveScene().buildIndex == 1){
            AkSoundEngine.SetState(playerLevelStateGroup, "Lv1");
            AkSoundEngine.SetState(playerStateStateGroup, "No_combat");
        } else if(SceneManager.GetActiveScene().buildIndex == 2){
            AkSoundEngine.SetState(playerLevelStateGroup, "Lv2");
            AkSoundEngine.SetState(level2CombatStateGroup, "Combat_hall_lv2");
        }
        

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
            case "Combat hall":
                AkSoundEngine.SetState(playerLevelStateGroup, "Lv2");
                AkSoundEngine.SetState(level2CombatStateGroup, "Combat_hall_lv2");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 5 - leval 2 hall");
                break;
            case "Xaga":
                AkSoundEngine.SetState(playerLevelStateGroup, "Lv2");
                AkSoundEngine.SetState(level2CombatStateGroup, "Combat_Xaga_Lv2");
                AkSoundEngine.SetState(combatIntensityStateGroup, "None");
                Debug.Log("Playing music level 6 - lvl 2 Xaga");
                break;
            case "Win":
                AkSoundEngine.SetState(level2EndingStateGroup, "Win");
                Debug.Log("Playing music level 7 - Win");
                break;
            case "Loose":
                AkSoundEngine.SetState(level2EndingStateGroup, "Loose");
                Debug.Log("Playing music level 8 - Loose");
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