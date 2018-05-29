using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthScript : MonoBehaviour {

	public int maxHp = 5;
	public int currentHp = 0;
	public bool isEnemy = true;

	[Header("Prefabs")]
	public GameObject BattleWon;
	public GameObject BattleLost;

	public SimpleHealthBar enemyHealthBar;

	void Start()
	{
		currentHp = maxHp;

		enemyHealthBar.UpdateBar( currentHp, maxHp );
	}
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D collider){
		ShotScript shot = collider.gameObject.GetComponent<ShotScript> ();

		if (shot != null) {
			if (shot.isEnemyShot != isEnemy) {
				currentHp -= shot.damage;

				Destroy (shot.gameObject);
				//StartCoroutine (ShakeCamera());
				Animator dragon_animator = gameObject.GetComponent<Animator> ();

				if (currentHp <= 0) {
					currentHp = 0;
					enemyHealthBar.UpdateBar( currentHp, maxHp );
					StartCoroutine (Die (dragon_animator));
				}
				else {
					enemyHealthBar.UpdateBar( currentHp, maxHp );
					dragon_animator.Play ("Dragon_Hurt");					
				}
			}
		}
	}
	private IEnumerator Die(Animator dragonAnimator)
	{
		dragonAnimator.Play ("Dragon_Death");
		var animController = dragonAnimator.runtimeAnimatorController;
		float animationLength = animController.animationClips.First (a => a.name == "Dragon_Death").length;
		yield return new WaitForSeconds(animationLength-0.2f);
		Destroy (gameObject);

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
		GameObject BattleWonObject = Instantiate (BattleWon);
		Animator winningAnimator = BattleWonObject.gameObject.GetComponent<Animator> ();
		winningAnimator.Play ("Battle_Won");
	}
}
