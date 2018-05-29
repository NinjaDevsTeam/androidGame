using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			SceneManager.LoadScene ("Stage2");
		}
	}
}
