using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    private static PreserveCharacter instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
