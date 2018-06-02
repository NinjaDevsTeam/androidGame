using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour {

	void Awake () {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        if (gameState.isSet) gameObject.transform.position = gameState.playerCameraPosition;
    }

	
}
