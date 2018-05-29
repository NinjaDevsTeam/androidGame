using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotScript : MonoBehaviour {

	public int damage = 1;
	public bool isPlayerShot = false;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 5);
	}
}
