using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour {

	static PlayerController instance;
	public static PlayerController Instance { get { return instance; } }

	public int maxHealth = 100;
	public int currentHealth = 0;
	public bool isPlayer = true;

	public SimpleHealthBar healthBar;

	[Header("Prefabs")]
	public GameObject BattleLost;

	void Awake ()
	{
		// If the instance variable is already assigned, then there are multiple player health scripts in the scene. Inform the user.
		if( instance != null )
			Debug.LogError( "There are multiple instances of the Player Health script. Assigning the most recent one to Instance." );




        currentHealth = maxHealth;
        // Assign the instance variable as the Player Health script on this object.
        instance = GetComponent<PlayerController>();
	}

	void Start()
	{
        currentHealth = PlayerPrefs.GetInt("healthpoints");
        healthBar.UpdateBar( currentHealth, maxHealth );
	}

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D collider){
		EnemyShotScript shot = collider.gameObject.GetComponent<EnemyShotScript> ();

		if (shot != null) {
			if (shot.isPlayerShot != isPlayer) {
				currentHealth -= shot.damage;

				Destroy (shot.gameObject);
				//StartCoroutine (ShakeCamera());
				Animator ninjaAnimator = gameObject.GetComponent<Animator> ();

				if (currentHealth <= 0) {
					currentHealth = 0;
					healthBar.UpdateBar( currentHealth, maxHealth );
					StartCoroutine (Die (ninjaAnimator));
				}
				else {
					healthBar.UpdateBar( currentHealth, maxHealth );
					ninjaAnimator.Play ("Ninja_Hurt");

                    PlayerPrefs.SetInt("healthpoints", currentHealth);			
                }
            }
		}
	}
	private IEnumerator Die(Animator ninjaAnimator)
	{
		ninjaAnimator.Play ("Ninja_Crouch");
		var animController = ninjaAnimator.runtimeAnimatorController;
		float animationLength = animController.animationClips.First (a => a.name == "Ninja_Crouch").length;
		yield return new WaitForSeconds(animationLength-0.2f);

		ShowBattleResult ();
	}

	IEnumerator ShakeCamera ()
	{
		// Store the original position of the camera.
		Vector2 origPos = Camera.main.transform.position;
		for( float t = 0.0f; t < 1.0f; t += Time.deltaTime * 2.0f )
		{
			// Create a temporary vector2 with the camera's original position modified by a random distance from the origin.
			Vector2 tempVec = origPos + Random.insideUnitCircle;

			// Apply the temporary vector.
			Camera.main.transform.position = tempVec;

			// Yield until next frame.
			yield return null;
		}

		// Return back to the original position.
		Camera.main.transform.position = origPos;
	}

	private void ShowBattleResult()
	{
		GameObject BattleLostObject = Instantiate (BattleLost);
		Animator winningAnimator = BattleLostObject.gameObject.GetComponent<Animator> ();
		winningAnimator.Play ("Battle_Lost");
	}

	public void HealPlayer ()
	{
		// Increase the current health by 25%.
		currentHealth += ( maxHealth / 4 );

		// If the current health is greater than max, then set it to max.
		if( currentHealth > maxHealth )
			currentHealth = maxHealth;

		// Update the Simple Health Bar with the new Health values.
		healthBar.UpdateBar( currentHealth, maxHealth );
	}
}
