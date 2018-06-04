using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrealoaderFading : MonoBehaviour {

	public CanvasGroup fadeGroup;
	public float loadTime;
	public float minLogoTime = 3.0f;
    public string scene;
	// Use this for initialization
	void Start () {
		fadeGroup = FindObjectOfType<CanvasGroup> ();
		fadeGroup.alpha = 1;

		if (Time.time < minLogoTime)
			loadTime = minLogoTime;
		else
			loadTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		//Fade-in
		if (Time.time < minLogoTime)
			fadeGroup.alpha = 1 - Time.time;
		
		//Fade-out
		if (Time.time > minLogoTime && loadTime != 0) {
			fadeGroup.alpha = Time.time - minLogoTime;

			if (fadeGroup.alpha >= 1) {
				SceneManager.LoadScene (scene);
			}
		}
	}
}
