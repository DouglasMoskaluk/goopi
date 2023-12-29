using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [SerializeField] private string[] sceneNames;

    //private Coroutine currentAction;
    public bool isLoading { get; private set; } = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool unloadScene(string name)
    {
        if (isLoading) return false;

        isLoading = true;
        StartCoroutine(UnloadScene(name));
        return true;
    }

    public bool loadScene(string name)
    {
        if (isLoading) return false;

        isLoading = true;
        StartCoroutine(LoadScene(name));
        return true;
    }

    private IEnumerator LoadScene(string sceneName)
    {

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);

        while (!load.isDone)
        {
            yield return null;
        }
        isLoading = false;
    }

    private IEnumerator UnloadScene(string sceneName)
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(sceneName);

        while (!unload.isDone)
        {
            yield return null;
        }
        isLoading = false;
    }

}
