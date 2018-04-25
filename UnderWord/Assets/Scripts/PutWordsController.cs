using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PutWordsController : MonoBehaviour {
    System.Random r;
	// Use this for initialization
	void Start () {
        r = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
        
            //GUI.Button(new Rect(r.Next(-5, 5), r.Next(5, 5), 2, 2),"message");
        //}
    }
    public void OnGUI()
    {
        if (r.Next(0, 1000) < 3)
        {
            GUI.Button(new Rect(r.Next(-5, 5), r.Next(5, 5), 2, 2), "message");
        }
    }
}
