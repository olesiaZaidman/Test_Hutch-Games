using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    int currentSceneIndex;
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

}
