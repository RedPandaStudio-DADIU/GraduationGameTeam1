using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]  private GameObject destroyedObject; //, explosionVFX;
    [SerializeField] private GameObject originalDoor;
    [SerializeField] private AK.Wwise.Event doorKicked;

    public void KickTheDoor(){
        doorKicked.Post(gameObject);

        originalDoor.SetActive(false);
        // GameObject broken = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        // GameObject doorBroken = Instantiate(destroyedObject, transform.position, Quaternion.identity);
        destroyedObject.SetActive(true);
        AddExplosionForceToPieces(destroyedObject.transform);
    }

    private void AddExplosionForceToPieces(Transform brokenDoor)
    {
        float explosionForce = 500f;
        float explosionRadius = 5f;
        Vector3 explosionPosition = brokenDoor.position;

        foreach (Rigidbody piece in brokenDoor.GetComponentsInChildren<Rigidbody>())
        {
            piece.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }
        
        StartCoroutine(DestroyPieces());

    }

    
    IEnumerator DestroyPieces()
    {
        
        yield return new WaitForSeconds(1f);
        Destroy(destroyedObject);

    }


}
