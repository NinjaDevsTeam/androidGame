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
		SceneManager.LoadScene ("Level Title");
	}
}
