using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightTrigger : MonoBehaviour {

    [SerializeField]
    private string loadLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameState gameState =
                GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            gameState.playerCameraPosition = new Vector2(player.transform.position.x,
                player.transform.position.y);
            gameState.isSet = true;
            LevelGenerator.generator.fightsPositions.Remove(gameObject.transform.position);
            Destroy(gameObject);
            SceneManager.LoadScene(loadLevel);            
        }
    }
}
