using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay;
    [SerializeField] float slowmoTimeBeforeNextScene = 0.2f;
    bool GotToThePortal;
    private void Start()
    {
        GotToThePortal = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GotToThePortal)
        { 
            StartCoroutine(LoadNextSceneCoru());
            GotToThePortal = false;
        }
        
   
        
    }
    

    private IEnumerator LoadNextSceneCoru()
    {
        GetComponent<AudioSource>().Play();
        Time.timeScale = slowmoTimeBeforeNextScene;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
         var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
        Time.timeScale = 1;
    }
}
