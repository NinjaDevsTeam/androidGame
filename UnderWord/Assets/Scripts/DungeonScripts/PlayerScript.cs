using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour {
	
	public Vector2 speed = new Vector2(3,3);
	//private float maxSpeed = 3;

	private Rigidbody2D rg2d;
	private Animator anim;

	void Start () {
		rg2d = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");

		float inputY = CrossPlatformInputManager.GetAxis ("Vertical");

		anim.SetFloat ("SpeedX", Mathf.Abs(inputX));
		anim.SetFloat ("SpeedY", Mathf.Abs(inputY));


		/*Vector3 movement = new Vector3 (speed.x * inputX, speed.y * inputY, 0);
		movement *= Time.deltaTime;


		if (inputX < -0.1f)
			transform.localScale = new Vector3 (
			-transform.localScale.x,
			transform.localScale.y,
			transform.localScale.z);

		if (inputX > 0.1f)
			transform.localScale = new Vector3 (
				transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z);
		
		transform.Translate (movement);
*/
		if (inputX < -0.1f && transform.localScale.x > 0)
			transform.localScale = new Vector3 (
				-transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z);

		if (inputX > 0.1f && transform.localScale.x < 0)
			transform.localScale = new Vector3 (
				-transform.localScale.x,
				transform.localScale.y,
				transform.localScale.z);

		Vector3 movement = new Vector3 (speed.x * inputX, speed.y * inputY, 0);
		movement *= Time.deltaTime;
		transform.Translate (movement);
		
		/*
		 * if (rg2d.velocity.x > maxSpeed)
			rg2d.velocity = new Vector2(maxSpeed, rg2d.velocity.y);

		if (rg2d.velocity.x < -maxSpeed)
			rg2d.velocity = new Vector2 (-maxSpeed, rg2d.velocity.y);
			*/
	}
}