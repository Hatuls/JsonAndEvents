using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void StartFirstLevel(int WhatScene)
    {
        SceneManager.LoadScene(WhatScene);
    }
}
