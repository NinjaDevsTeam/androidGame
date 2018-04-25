using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AppearingMonstersController : MonoBehaviour {
    //Rigidbody2D player;
    System.Random r;
    // Use this for initialization
    void Start () {
        Debug.Log("adventure!!!");
        r = new System.Random();
    }
	
	// Update is called once per frame
	void Update () {
        if (r.Next(0, 100) < 3)
            Debug.Log("monsters!!!");
        if (r.Next(0, 1000) < 3)
        {
            gameObject.SetActive(false);
            SceneManager.GetSceneByName("Player").GetRootGameObjects()[0].transform.position = new Vector3(0, -6, -10);
            SceneManager.GetSceneByName("Player").GetRootGameObjects()[1].SetActive(false);
            SceneManager.GetSceneByName("Player").GetRootGameObjects()[2].SetActive(false);
            MySceneManager.Instance.Load("Fight screen");
                Debug.Log("loadscene");
        }
    }
}
