using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    public static MySceneManager Instance { get; set; }
	// Use this for initialization
    private void Awake()
    {
        //gamemanager
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
        Load("Player");
        Load("Base room");
        Load("Bridge");
    }
	public void Load(string sceneName)
    {
        Debug.Log("try to show " + sceneName);
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            Debug.Log(sceneName + " on");
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
    public void Unload(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            //SceneManager.UnloadScene(sceneName);
            Debug.Log(sceneName + " off");
        }
    }
}
