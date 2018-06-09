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
			if (loadLevel == "Level Title") {
				ScrollScript.numberOfPickedScrolls = 0;
				SceneManager.LoadScene (loadLevel);
				return;
			}
			LevelGenerator levGen =
				GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
			if (loadLevel == "Base room" && ScrollScript.numberOfPickedScrolls == levGen.numberOfScrolls)
            {
				SceneManager.LoadScene(loadLevel);
                PlayerPrefs.SetInt("healthpoints", 100);
                return;
            }

        }
    }
}
