using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cowsins;
using AK.Wwise;


public class HandleButtons : Interactable
{
    private bool interactionOccured = false;
    private GameObject elevatorDoor;
    [SerializeField] private AK.Wwise.Event disabledButtonEvent;
    [SerializeField] private AK.Wwise.Event goodButtonEvent;

    // Start is called before the first frame update
    void Start()
    {
        elevatorDoor = GameObject.FindWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Interact(Transform player)
    {
        interactableEvents.OnInteract?.Invoke();
        Debug.Log("Interacted with" + this.gameObject.name); 


        if (this.gameObject.CompareTag("ElevatorButton")){

            if (GameObject.FindWithTag("MainframeButton").GetComponent<HandleButtons>().GetInteractionOccured()){
                goodButtonEvent.Post(elevatorDoor);
                DestroyDoor();
            } else {
                disabledButtonEvent.Post(elevatorDoor);

            }


        }else if (this.gameObject.CompareTag("MainframeButton")){
            interactionOccured = true;
            goodButtonEvent.Post(elevatorDoor);

        }
    }

    public void DestroyDoor(){
        Destroy(elevatorDoor);
    }

    public bool GetInteractionOccured(){
        return this.interactionOccured;
    }

}
