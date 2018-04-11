using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("c"))
        {
            SceneManager.LoadScene("Fight screen", LoadSceneMode.Single);
            Debug.Log("loadscene");
        }
    }
}
