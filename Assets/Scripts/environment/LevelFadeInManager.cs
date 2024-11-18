using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFadeInManager : MonoBehaviour
{
    [SerializeField] private Image blackScreenImage; 
    [SerializeField] private float fadeDuration = 3f; 
    [SerializeField] private  GameObject blackScreen;

    // Start is called before the first frame update
    void Start()
    {
        Color color = blackScreenImage.color;
        color.a = 1f;
        blackScreenImage.color = color;

       // Time.timeScale = 0;

        
        StartCoroutine(FadeInAndResumeGame());

    }

     private IEnumerator FadeInAndResumeGame()
    {
         Debug.Log("call IEnumerator FadeINLEVEL2");
            
        float elapsedTime = 0f;
        Color color = blackScreenImage.color;

   
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = alpha; 
            Debug.Log("change color alpha2");
            blackScreenImage.color = color;
            yield return null;
        }

  
        //Time.timeScale = 1;

        blackScreen.SetActive(false);
        blackScreenImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
