using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalFadeIn : MonoBehaviour
{
    [SerializeField] private Image blackScreenImage; 

    [SerializeField] private float fadeDuration = 33f; 

    [SerializeField] private  GameObject blackScreen;
    private bool startFading = false;
    private bool finished = false;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        Color color = blackScreenImage.color;
        color.a = 0f;
        blackScreenImage.color = color;


       // Time.timeScale = 0; 
       
    }

    private IEnumerator FadeInAndEnd()
    {
        Debug.Log("call IEnumerator FadeINLEVEL2");
            
        float elapsedTime = 0f;
        Color color = blackScreenImage.color;

   
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 0f + Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha; 
            // Debug.Log("change color alpha2");
            blackScreenImage.color = color;
            yield return null;
        }

        Debug.Log("Is FINISHED");
        SceneManager.LoadScene("Credits");
    }   

    void Update()
    {
        if(startFading && !started){
            started = true;
            StartCoroutine(FadeInAndEnd());

        }
    }

    public void StartFading(){
        startFading = true;
    }

    public bool GetFinished(){
        return this.finished;
    }
}
