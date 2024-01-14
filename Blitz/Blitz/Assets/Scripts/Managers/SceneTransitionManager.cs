using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    public Scenes currentScene;

    [SerializeField] private string[] sceneNames;

    //private Coroutine currentAction;
    public bool isLoading { get; private set; } = false;

    [SerializeField] private float FadeOutDuration;
    [SerializeField] private float FadeInDuration;

    private void Awake()
    {
        if (instance == null) instance = this;

        loadScene(Scenes.MainMenu);
    }

    ///// <summary>
    ///// attemps to switch from the current scene to the given scene, returns true if process was able to start false otherwise
    ///// </summary>
    ///// <param name="scene"></param>
    ///// <returns></returns>
    //public bool switchScene(Scenes scene)
    //{
    //    if (isLoading) return false;

    //    isLoading = true;
    //    StartCoroutine(switchSceneCoro(scene));
    //    return true;
    //}

    ///// <summary>
    ///// attemps to unload the current, returns true if process was able to start false otherwise
    ///// </summary>
    ///// <param name="scene"></param>
    ///// <returns></returns>
    //public bool unloadScene(Scenes scene)
    //{
    //    if (isLoading) return false;

    //    isLoading = true;
    //    StartCoroutine(UnloadScene(scene));
    //    return true;
    //}

    ///// <summary>
    ///// attemps to load the given scene, returns true if process was able to start false otherwise
    ///// </summary>
    ///// <param name="scene"></param>
    ///// <returns></returns>
    //public bool loadScene(Scenes scene)
    //{
    //    if (isLoading) return false;

    //    isLoading = true;
    //    StartCoroutine(LoadScene(scene));
    //    return true;
    //}

    /// <summary>
    /// attemps to switch from the current scene to the given scene, returns created coroutine
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public Coroutine switchScene(Scenes scene)
    {
        if (isLoading) return null;

        isLoading = true;
        return StartCoroutine(switchSceneCoro(scene));
    }

    /// <summary>
    /// attemps to unload the current, returns true if process was able to start false otherwise, also fills out a Coroutine reference
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public Coroutine unloadScene(Scenes scene)
    {
        if (isLoading) return null;

        isLoading = true;
        return StartCoroutine(UnloadScene(scene));
    }

    public Coroutine unloadScene()
    {
        if (isLoading) return null;

        isLoading = true;
        return StartCoroutine(UnloadScene(currentScene));
    }

    /// <summary>
    /// attemps to load the given scene, returns true if process was able to start false otherwise, also fills out a Coroutine reference
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public Coroutine loadScene(Scenes scene)
    {
        if (isLoading) return null;

        isLoading = true;
        return StartCoroutine(LoadScene(scene));
    }

    /// <summary>
    /// coroutine for loading a scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private IEnumerator LoadScene(Scenes scene)
    {

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneNames[(int)scene], LoadSceneMode.Additive);

        while (!load.isDone)
        {
            yield return null;
        }

        currentScene = scene;
        isLoading = false;
    }

    /// <summary>
    /// coroutine for unloading a scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private IEnumerator UnloadScene(Scenes scene)
    {
        Debug.Log("unloading scene " + sceneNames[(int)scene]);
        AsyncOperation unload = SceneManager.UnloadSceneAsync(sceneNames[(int)scene]);

        while (!unload.isDone)
        {
            yield return null;
        }

        isLoading = false;
    }

    /// <summary>
    /// coroutine for switching from the current scene to the given scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private IEnumerator switchSceneCoro(Scenes scene)
    {
        yield return GameUIManager.instance.FadeIn(FadeInDuration);
        //yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.FadeInCoroutine(FadeInDuration));

        yield return StartCoroutine(UnloadScene(currentScene));

        yield return StartCoroutine(LoadScene(scene));

        yield return GameUIManager.instance.FadeOut(FadeOutDuration);
        //yield return GameUIManager.instance.StartCoroutine(GameUIManager.instance.FadeOutCoroutine(FadeOutDuration));

        isLoading = false;
    }

}

public enum Scenes { 
    MainMenu, LockerRoom, Arena, Podium
}

