using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerButton : MonoBehaviour {

	public bool hasGoodAnswer = false;
	Animation selfAnim;
	// Use this for initialization
	void Start () {
		selfAnim = gameObject.GetComponent<Animation> ();
		selfAnim.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
