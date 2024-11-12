using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void Attack(){

    }

    public void Die(){
        gameObject.GetComponent<EnemyRagdollController>().SetRagdollActive(true);
    }

}
