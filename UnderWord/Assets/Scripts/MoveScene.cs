using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    //[SerializeField]
    private string loadLevel = "Base room";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            int l = PlayerPrefs.GetInt("level", 1);
            if (loadLevel == "Dungeon" && ScrollScript.numberOfPickedScrolls == 2)
            {
                SceneManager.LoadScene(loadLevel);
                PlayerPrefs.SetInt("healthpoints", 100);
                if (l > 3)
                    PlayerPrefs.SetInt("level", 1);
                else
                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
                return;
            }
            if (loadLevel == "Base room")
            {
                if (l > 3)
                    PlayerPrefs.SetInt("level", 1);
                SceneManager.LoadScene("Dungeon");
                loadLevel = "Dungeon";  
            }

        }
    }
}
