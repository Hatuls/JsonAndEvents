using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{

    int startingSceneIndex;
  

    private void Awake()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;

        ScenePersist[] scenePersists = FindObjectsOfType<ScenePersist>();


        if (scenePersists.Length <= 1)
        {
            // I am alone
            DontDestroyOnLoad(gameObject);
            Debug.Log("Scene Persist is Alone");
        }
        else
        {
            bool destroyMe = false; 
            Debug.Log("Scene Persist In The Scene alread");
            // We are Two
            foreach (ScenePersist scenePersist in scenePersists)
            {
                if (scenePersist != this)
                {
                    // It's not me
                    if (scenePersist.startingSceneIndex != startingSceneIndex)
                    {
                        // You have nothing to do here
                        Destroy(scenePersist.gameObject);
                    }
                    else
                    {
                        // I have nothing to do here
                        destroyMe = true;
                    }
                }
            }

            if (destroyMe)
            {
                // Seppuku!
                Destroy(gameObject);
                Debug.Log("Scenepersist destroyed");
            }
            else
            {
                // They are all dead, I will survive!
                DontDestroyOnLoad(gameObject);
                Debug.Log("Scenepersist survived");
            }
           
        }

    }
}
