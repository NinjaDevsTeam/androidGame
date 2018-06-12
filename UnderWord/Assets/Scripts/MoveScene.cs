using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField]
    private string loadLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("healthpoints", 5);
            if (loadLevel == "Level Title")
            {
                ScrollScript.numberOfPickedScrolls = 0;
                SceneManager.LoadScene(loadLevel);
                return;
            }
			if (loadLevel == "Base room" && ScrollScript.numberOfPickedScrolls == LevelGenerator.generator.numberOfScrolls)
            {
				SceneManager.LoadScene(loadLevel);
                PlayerPrefs.SetInt("healthpoints", 5);
                return;
            }

        }
    }
}
