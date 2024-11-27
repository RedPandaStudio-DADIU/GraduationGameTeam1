using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("Instantiate this when the barrel explodes"), SerializeField]
    private GameObject destroyedObject; //, explosionVFX;
    [SerializeField] private AK.Wwise.Event doorKicked;

    public void KickTheDoor(){
        doorKicked.Post(gameObject);
        // GameObject doorBroken = Instantiate(destroyedObject, transform.position, Quaternion.identity);

        // Instantiate(explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
