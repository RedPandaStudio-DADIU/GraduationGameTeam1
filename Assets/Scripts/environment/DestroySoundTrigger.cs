using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySoundTrigger : MonoBehaviour
{
    [SerializeField] private float delay = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitToDisappear(delay));
        }

    }


    IEnumerator WaitToDisappear(float delay)
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);

    }
}