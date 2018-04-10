using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player2DController : MonoBehaviour
{
    public float moveForce = 5;
    Rigidbody2D myBody;

	// Use this for initialization
	void Start ()
    {
        myBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector2 myVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
            CrossPlatformInputManager.GetAxis("Vertical")) * moveForce;
        myBody.AddForce(myVec);
	}
}
