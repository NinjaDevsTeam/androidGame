using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFader : MonoBehaviour {

	public CanvasGroup fadeGroup;
	public float fadeInTime = 0.33f;

	// Use this for initialization
	void Start () {
		fadeGroup = FindObjectOfType<CanvasGroup> ();
		fadeGroup.alpha = 1;
	}

	// Update is called once per frame
	void Update () {
		//Fade-in
		fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInTime;
	}
}
