using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : Singleton<SceneLoader>
{
    public class SceneLoadedEvent : EventManager.Event
    {
        public Scene scene;
    }

    public void OnSceneLoadedCallBack(SceneLoadedEvent _e)
    {
    }

    private void Start()
    {
        EventManager.Instance.AddListener<SceneLoadedEvent>(OnSceneLoadedCallBack);
    }

    public MenuClassifier loadingMenuClasifier;
    private float delayTimer = 1.0f;
    private string currentLoadedScene = "";

    public void UnLoadCurrentScene()
    {
        StartCoroutine(UnLoadScene(currentLoadedScene));
    }

    IEnumerator UnLoadScene(string _unloadScene)
    {
        AsyncOperation _sync = null;
        _sync = SceneManager.UnloadSceneAsync(_unloadScene);
        while (_sync.isDone == false) { yield return null; }
        _sync = Resources.UnloadUnusedAssets();
        while (_sync.isDone == false) { yield return null; }
    }

    public void LoadLevel(string _loadScene, bool _showLoadingScreen = true, string _unloadScene = "")
    {
        StartCoroutine(LoadScene(_loadScene, _showLoadingScreen, _unloadScene));
    }

    IEnumerator LoadScene(string _scene, bool _showLoadingScreen, string _unloadScene)
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        if (_showLoadingScreen == true && loadingMenuClasifier != null)
        {
            MenuManager.Instance.showMenu(loadingMenuClasifier);
        }

        yield return new WaitForSeconds(delayTimer);
        AsyncOperation _sync = null;

        if (_unloadScene != "")
        {
            _sync = SceneManager.UnloadSceneAsync(_unloadScene);
            while (_sync.isDone == false) { yield return null; }
            _sync = Resources.UnloadUnusedAssets();
            while (_sync.isDone == false) { yield return null; }
        }

        if (currentLoadedScene != "" && SceneManager.GetSceneByName(currentLoadedScene).isLoaded)
        {
            _sync = SceneManager.UnloadSceneAsync(currentLoadedScene);
            while (_sync.isDone == false) { yield return null; }
            _sync = Resources.UnloadUnusedAssets();
            while (_sync.isDone == false) { yield return null; }
        }

        yield return new WaitForSeconds(delayTimer);

        _sync = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        while (_sync.isDone == false) { yield return null; }

        yield return new WaitForSeconds(delayTimer);

        Scene loadedScene = SceneManager.GetSceneByName(_scene);
        if (loadedScene != null && loadedScene.buildIndex != -1)
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(loadedScene);
        }
        //else
        //{
        //    throw new System.Exception("Scene not found in Scene Manager: " + _scene);
        //}

        currentLoadedScene = _scene;

        SceneLoadedEvent _event = new SceneLoadedEvent();
        EventManager.Instance.Raise(_event);

        if (_showLoadingScreen == true && loadingMenuClasifier != null)
        {
            MenuManager.Instance.hideMenu(loadingMenuClasifier);
        }

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }
}