using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCredits : MonoBehaviour
{

    
    void Update()
    {
        if(GameObject.FindWithTag("EndScreen").GetComponent<FinalFadeIn>().GetFinished()){
            this.GetComponent<FinalFadeIn>().StartFading();
        }
    }
}
