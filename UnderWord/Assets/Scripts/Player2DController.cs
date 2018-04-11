using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player2DController : MonoBehaviour
{
    public float velocity = 1;
    Rigidbody2D myBody;
    public Vector2 tmpPosition;

	// Use this for initialization
	void Start ()
    {
        myBody = this.GetComponent<Rigidbody2D>();
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
