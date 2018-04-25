using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightScreenController : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
        MySceneManager.Instance.Load("SwordScene");

        SceneManager.GetSceneByName("Fight screen").GetRootGameObjects()[4].SetActive(false);
    }
	
    public void Escape()
    {
        SceneManager.GetSceneByName("Adventure").GetRootGameObjects()[0].SetActive(true);
        SceneManager.GetSceneByName("Player").GetRootGameObjects()[1].SetActive(true);
        SceneManager.GetSceneByName("Player").GetRootGameObjects()[2].SetActive(true);
        MySceneManager.Instance.Unload("SwordScene");
        MySceneManager.Instance.Unload("Fight screen");
    }
}
