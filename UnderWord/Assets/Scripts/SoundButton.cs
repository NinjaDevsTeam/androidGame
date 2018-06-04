using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

    public GameObject audioSource;
    bool soundToggle = false;

    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(ToggleSoundOnOff);
    }
    void ToggleSoundOnOff()
    {
        soundToggle = !soundToggle;
        if (soundToggle)
        {
            AudioListener.pause = true;
            gameObject.GetComponentInChildren<Text>().text = "Sound: Off";
        }
        else
        {
            AudioListener.pause = false;
            gameObject.GetComponentInChildren<Text>().text = "Sound: On";
        }
    }
}
