using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class Startup : MonoBehaviour
{
    public bool EnableMultiTouch = false;
    public bool loadScene = true;
    public SceneReference StartupScene;

    void Start()
    {
        Input.multiTouchEnabled = EnableMultiTouch;

        Scene _scene = SceneManager.GetSceneByName(StartupScene);
        if (_scene.isLoaded == false && loadScene == true)
        {
            Debug.Log("Loading Startup Scene: " + StartupScene);
            SceneLoader.Instance.LoadLevel(StartupScene, true);
        }
    }
}