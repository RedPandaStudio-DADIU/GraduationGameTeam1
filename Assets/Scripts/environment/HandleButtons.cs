using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using AK.Wwise;


public class HandleButtons : Interactable
{
    private bool interactionOccured = false;
    private GameObject elevatorDoor;

    [SerializeField] private AK.Wwise.Event elevatorDoorOpen;
    [SerializeField] private AK.Wwise.Event elevatorButtonPress;
    [SerializeField] private AK.Wwise.State myState;


    void Start()
    {
        elevatorDoor = GameObject.FindWithTag("ElevDoor");
        AkSoundEngine.SetSwitch("ElevatorPower", "PowerOff", gameObject);
    
    }

    void Update()
    {
        
    }


    public override void Interact(Transform player)
    {
        interactableEvents.OnInteract?.Invoke();
        Debug.Log("Interacted with" + this.gameObject.name); 


        if (this.gameObject.CompareTag("ElevatorButton")){

            if (GameObject.FindWithTag("MainframeButton").GetComponent<HandleButtons>().GetInteractionOccured()){
                AkSoundEngine.SetSwitch("ElevatorPower", "PowerOn", gameObject);
                elevatorButtonPress.Post(gameObject);

                DestroyDoor();
            } else {
                elevatorButtonPress.Post(gameObject);
                myState.SetValue();
            }


        }else if (this.gameObject.CompareTag("MainframeButton")){
            interactionOccured = true;
            AkSoundEngine.SetSwitch("ElevatorPower", "PowerOn", gameObject);
            elevatorButtonPress.Post(gameObject);
            myState.SetValue();

        }
    }

    public void DestroyDoor(){
        elevatorDoorOpen.Post(elevatorDoor);
        Destroy(elevatorDoor);
    }

    public bool GetInteractionOccured(){
        return this.interactionOccured;
    }

}
