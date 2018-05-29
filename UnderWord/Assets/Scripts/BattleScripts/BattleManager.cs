using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class BattleManager : MonoBehaviour
{
	// Static Refs //
	private static BattleManager instance;
	public static BattleManager Instance{ get { return instance; } }

	private const int STARTING_INDEX = 0;

	// Prefabs //
	[Header( "ShotsPrefabs" )]
	public GameObject BigFireballPrefab;
	public GameObject FireballPrefab;
	public GameObject DragonballPrefab;

	[Header( "ButtonBallPrefabs" )]
	public GameObject AnswerButtonPrefab;
	public GameObject QuestionBlueBall;
	public GameObject FABall;
	public GameObject SABall;
	public GameObject TABall;

	private GameObject questionBall;
	private GameObject firstBall;
	private GameObject secondBall;
	private GameObject thirdBall;

	[Header( "ButtonPrefabs" )]
	public GameObject QuestionButton;
	public GameObject FAButton;
	public GameObject SAButton;
	public GameObject TAButton;

	private GameObject QButton;
	private GameObject FButton;
	private GameObject SButton;
	private GameObject TButton;

	[Header( "BattleOver" )]
	bool hasLost = false;

	void Awake ()
	{
		// If the instance variable is already assigned...
		if( instance != null )
		{
			// If the instance is currently active...
			if( instance.gameObject.activeInHierarchy == true )
			{
				// Warn the user that there are multiple Game Managers within the scene and destroy the old manager.
				Debug.LogWarning( "There are multiple instances of the Game Manager script. Removing the old manager from the scene." );
				Destroy( instance.gameObject );
			}

			// Remove the old manager.
			instance = null;
		}

		// Assign the instance variable as the Game Manager script on this object.
		instance = GetComponent<BattleManager>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NormalAttack(){
		GameObject fireball = Instantiate (FireballPrefab);
		fireball.name = FireballPrefab.name;
		Destroy (fireball, 5.0f);

		GameObject ninja = GameObject.Find ("Ninja_Animator");
		Animator ninjaAnimator = ninja.GetComponent<Animator> ();
		ninjaAnimator.Play ("Ninja_Attack");

		Button simpleAttackButton = GameObject.Find ("BasicAttackButton").GetComponent<UnityEngine.UI.Button>();
		simpleAttackButton.enabled = false;

		Button specialAttackButton = GameObject.Find ("SpecialAttackButton").GetComponent<UnityEngine.UI.Button>();
		specialAttackButton.enabled = false;

		StartCoroutine (WaitAndAttack (1.0f));
	}

	public void FireballAttack(){
		Button simpleAttackButton = GameObject.Find ("BasicAttackButton").GetComponent<UnityEngine.UI.Button>();
		simpleAttackButton.enabled = false;

		Button specialAttackButton = GameObject.Find ("SpecialAttackButton").GetComponent<UnityEngine.UI.Button>();
		specialAttackButton.enabled = false;

		CreateAnswer (STARTING_INDEX);
	}

	public void DragonAttack(){
		GameObject fireball = Instantiate (DragonballPrefab);
		fireball.name = DragonballPrefab.name;
		Destroy (fireball, 5.0f);

		GameObject dragon = GameObject.Find ("Dragon_Animator");
		Animator dragonAnimator = dragon.GetComponent<Animator> ();
		dragonAnimator.Play ("Dragon_Attack");

		Button simpleAttackButton = GameObject.Find ("BasicAttackButton").GetComponent<UnityEngine.UI.Button>();
		simpleAttackButton.enabled = true;

		Button specialAttackButton = GameObject.Find ("SpecialAttackButton").GetComponent<UnityEngine.UI.Button>();
		specialAttackButton.enabled = true;
	}

	private void CreateAnswer(int index){
		if (index == 4)
			return;
		
		//jakikolwiek obiekt, który ma Animator
		GameObject obj = GameObject.Find ("Ninja_Animator");
		Animator ballAnimator = obj.GetComponent<Animator> ();
		string [] animationNames = { "Blue_Question_Ball", "Blue_Ans1_Ball", "Blue_Ans2_Ball", "Blue_Ans3_Ball" };
		switch(index)
		{
		case 0:
			questionBall = Instantiate (QuestionBlueBall);
			ballAnimator = questionBall.GetComponent<Animator> ();
			break;
		case 1:
			firstBall = Instantiate (FABall);
			ballAnimator = firstBall.GetComponent<Animator> ();
			break;
		case 2:
			secondBall = Instantiate (SABall);
			ballAnimator = secondBall.GetComponent<Animator> ();
			break;
		case 3:
			thirdBall = Instantiate (TABall);
			ballAnimator = thirdBall.GetComponent<Animator> ();
			break;
		}
		ballAnimator.Play (animationNames[index]);
		StartCoroutine(WaitForAnimationEnd(ballAnimator,animationNames[index], index));
	}

	private void AnimateAnswerButton(int index){
		Vector3 position;
		GameObject canvas = GameObject.Find("Canvas");
		GameObject answerButton = GameObject.Find("Canvas");
		Text buttonText;
		switch(index)
		{
		case 0:
//			QButton = Instantiate (QuestionButton);
			position = new Vector3 (650, 650, 0);
//			buttonText = QButton.gameObject.GetComponent<Text> ();
//			buttonText.text = "run";
//			Destroy (questionBall);
			InstantiateAnswerButton(out QButton, QuestionButton, position, "run",  ref questionBall, ref canvas);
			break;
		case 1:
//			FButton = Instantiate (FAButton);
			position = new Vector3 (650, 430, 0);
//			buttonText = FButton.gameObject.GetComponent<Text> ();
//			AnswerButton buttonScript = FButton.gameObject.GetComponent<AnswerButton> ();
//			buttonScript.hasGoodAnswer = true;
//			buttonText.text = "biec";
//			Destroy (firstBall);			
			InstantiateAnswerButton(out FButton, FAButton, position, "biec", ref firstBall, ref canvas);
			AnswerButton buttonScript = FButton.gameObject.GetComponent<AnswerButton> ();
			buttonScript.hasGoodAnswer = true;
			break;
		case 2:
//			SButton = Instantiate (SAButton);
			position = new Vector3 (650, 280, 0);
//			buttonText = SButton.gameObject.GetComponent<Text> ();
//			buttonText.text = "kopać";
			//			Destroy (secondBall);			
			InstantiateAnswerButton(out SButton, SAButton, position, "upaść", ref secondBall, ref canvas);
			break;
		case 3:
//			TButton = Instantiate (TAButton);
			position = new Vector3 (650, 130, 0);
//			buttonText = TButton.gameObject.GetComponent<Text> ();
//			buttonText.text = "pokazywać";
			//			Destroy (thirdBall);	
			InstantiateAnswerButton(out TButton, TAButton, position, "myć", ref thirdBall, ref canvas);		
			break;
		}
		/*answerButton.transform.SetParent(canvas.transform);
		Animator ABAnimator = answerButton.GetComponent<Animator> ();
		Animation ABAnimation = answerButton.GetComponent<Animation> ();
		ABAnimation.enabled = true;
		ABAnimator.Play ("Answer_Button_Show");*/
	}

	private void InstantiateAnswerButton(out GameObject button, GameObject submittedButton, Vector3 position, string text,  ref GameObject ball, ref GameObject canvas)
	{
		button = Instantiate (submittedButton);
		button.transform.position = position;
		Button btn = button.gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (CheckIfGoodButtonClicked);
		Text buttonText = button.gameObject.GetComponentInChildren<Text> ();
		buttonText.text = text;
		Destroy (ball);
		button.transform.SetParent (canvas.transform);
		Animator ABAnimator = button.GetComponent<Animator> ();
		Animation ABAnimation = button.GetComponent<Animation> ();
		ABAnimation.enabled = true;
		ABAnimator.Play ("Answer_Button_Show");
	}

	private IEnumerator WaitForAnimationEnd(Animator animator, string animationName, int index)
	{
		animator.Play (animationName);
		var animController = animator.runtimeAnimatorController;
		float animationLength = animController.animationClips.First (a => a.name == animationName).length;
		yield return new WaitForSeconds (animationLength - 0.2f);

		AnimateAnswerButton (index);
		CreateAnswer (index + 1);
	}

	private IEnumerator WaitAndAttack(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		GameObject dragon = GameObject.Find ("Dragon_Animator");
		HealthScript hs = dragon.gameObject.GetComponent<HealthScript> ();
		if(hs.currentHp >0)
			DragonAttack ();
	}

	public void CheckIfGoodButtonClicked()
	{
		GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
		AnswerButton buttonScript = buttonClicked.GetComponent<AnswerButton> ();
		if (buttonScript.hasGoodAnswer) {
			GameObject fireball = Instantiate (BigFireballPrefab);
			fireball.name = BigFireballPrefab.name;
			Destroy (fireball, 5.0f);

			GameObject ninja = GameObject.Find ("Ninja_Animator");
			Animator ninjaAnimator = ninja.GetComponent<Animator> ();
			ninjaAnimator.Play ("Ninja_Special_Attack");
		} 

		DestroyButtonsAfterAnswer ();
		StartCoroutine (WaitAndAttack (1.0f));
	}

	private void DestroyButtonsAfterAnswer()
	{
		Destroy (QButton);
		Destroy (FButton);
		Destroy (SButton);
		Destroy (TButton);
	}

}
