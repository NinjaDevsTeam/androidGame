using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextDisappeared : MonoBehaviour {
	public Text wordText;
	private int timeToExpose = 1000;
	private int remainTime = 0;
	public string currText;
	// Use this for initialization
	void Start () {
		wordText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (wordText.text != "") {
			if (currText != wordText.text) {
				remainTime = 0;
			}
			currText = wordText.text;
			remainTime += 1;
			if (remainTime == timeToExpose) {
				wordText.text = "";
				remainTime = 0;
			}
		}
	}
}
