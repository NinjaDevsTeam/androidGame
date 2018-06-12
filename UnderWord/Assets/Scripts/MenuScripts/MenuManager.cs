using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void QuitApp()
	{
		Application.Quit ();
	}

	public void PlayGame()
	{
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        gameState.isNewLevel = true;
        gameState.levelCounter = 0;
		SceneManager.LoadScene ("Level Title");
	}

    public void GoToMenu()
    {
        GameState gameState =
                     GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        gameState.sceneToGo = "Dungeon";
        if (SceneManager.GetActiveScene().name == "Dungeon")
        {
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            gameState.playerCameraPosition = new Vector2(player.transform.position.x,
                player.transform.position.y);
            gameState.isSet = true;
        }
        if(SceneManager.GetActiveScene().name == "Base room")
        {
            gameState.sceneToGo = "Base room";
        }
        SceneManager.LoadScene("Menu");
    }

    public void Resume()
    {
        GameState gameState =
                     GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        SceneManager.LoadScene(gameState.sceneToGo);
    }
}
