using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player2DController : MonoBehaviour
{
    public List<KeyValuePair<string, string>> LearnedWords;
    public float velocity = 1;
    Rigidbody2D myBody;
    public Vector2 tmpPosition;

	// Use this for initialization
	void Start ()
    {
        LearnedWords = new List<KeyValuePair<string, string>>();
        LearnedWords.Add(new KeyValuePair<string, string>("gra", "game"));
        myBody = this.GetComponent<Rigidbody2D>();
        Debug.Log("zmiana sceny");
        //if (SceneManager.GetActiveScene().name == "Adventure")
        //{
        //    myBody.position = new Vector2(8, 0);
        //    Debug.Log("playerAdv");
        //}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 myVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
            CrossPlatformInputManager.GetAxis("Vertical")) * velocity * Time.deltaTime;
        //myBody.AddForce(myVec);
        tmpPosition = myBody.position + myVec;
        myBody.position = tmpPosition;
	}
	
}
