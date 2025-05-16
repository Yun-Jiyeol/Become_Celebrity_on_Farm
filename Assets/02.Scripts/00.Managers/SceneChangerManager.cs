using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerManager : MonoBehaviour
{
    private static SceneChangerManager instance = null;

    [Header("Scene Settings")]
    public string[] sceneNamesInBuild;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static SceneChangerManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
    public void OnClick_LoadScene(string targetSceneName)
    {
        LoadSceneByName(targetSceneName);
    }

    public void OnClick_QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
