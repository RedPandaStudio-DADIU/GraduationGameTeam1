using UnityEngine;
using UnityEngine.SceneManagement;
namespace cowsins
{
    public class DeathRestart : MonoBehaviour
    {
        private GameObject player;
        private void Start(){
            player = GameObject.FindWithTag("Player");
        }
        private void Update()
        {

            if (InputManager.reloading) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}