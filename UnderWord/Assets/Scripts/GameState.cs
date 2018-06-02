using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public Vector2 playerCameraPosition;
    public bool isSet;

    public static GameState gameState;

    private void Awake()
    {
        if (gameState == null)
        {
            DontDestroyOnLoad(gameObject);
            gameState = this;
        }
        else if (gameState != this)
        {
            Destroy(gameObject);
        }
    }
}
