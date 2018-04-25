using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordChallengeController : MonoBehaviour {
    public Text EnglishText;
    public Text PolishText;
    bool guessedEnglish = false;
    bool guessedPolish = false;
	// Use this for initialization
	void Start ()
    {
        EnglishText.text = "game";
        PolishText.text = "gra";
    }
	public void GuessPolish()
    {
        guessedPolish = true;
        if (guessedEnglish)
            MySceneManager.Instance.Unload("SwordScene");
    }
    public void GuessEnglish()
    {
        guessedEnglish = true;
        if(guessedPolish)
            MySceneManager.Instance.Unload("SwordScene");
    }
	// Update is called once per frame
}
