using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    public static int numberOfPickedScrolls = 0;
	GameObject wordButton;
	Text textObj;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            try
			{
				LevelGenerator.generator.scrollsPositions.Remove(gameObject.transform.position);
				wordButton = GameObject.Find("WordButton");
				textObj = (Text)wordButton.gameObject.GetComponentInChildren(typeof(Text));
				string english = null, polish = null;
				bool sqlDoesntWork = false;
				try
				{
					english = LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Value;
					polish = LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Key;
				}
				catch
				{
					sqlDoesntWork = true;
				}
				if(sqlDoesntWork)
				{
					textObj.text = "Your last collected word:\n" + english + " \n-\n" + polish;
					LevelGenerator.knownVocabulary.Add(new KeyValuePair<string, string>(polish, english));
					//StartCoroutine(func(textObj));
					//Invoke("Dissappear",2f);
				}
               	else
				{
					AudioClip clip = Resources.Load<AudioClip>(english);
					if(clip!=null)
						AudioSource.PlayClipAtPoint(clip,transform.position);
					textObj.text = "Your last collected word:\n" + english + " \n-\n" + polish;
					LevelGenerator.knownVocabulary.Add(new KeyValuePair<string, string>(polish, english));
					StartCoroutine(func(textObj));
	                print("zebrane slowko: " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Value.ToUpper() + " means: " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Key.ToUpper());
				}
				numberOfPickedScrolls++;
                print("liczba zebranych slow : " + numberOfPickedScrolls);
            }
            catch
            {
                print("Error: No level generator found");
            }
            finally
            {
                Destroy(gameObject);
            }
        }
    }
	private void Dissappear()
	{
		textObj.text = "kaszanka";
	}

	private IEnumerator func(Text textObj)
	{
		yield return new WaitForSeconds(0.5f);
		textObj.text = "zawsze moge";
	}
}
