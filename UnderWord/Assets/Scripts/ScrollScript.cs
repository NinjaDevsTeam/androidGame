using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    public static int numberOfPickedScrolls = 0;
	Text textObj;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            try
			{
				LevelGenerator.generator.scrollsPositions.Remove(gameObject.transform.position);
				textObj = GameObject.FindObjectOfType<Text>();
				string english = null;
				bool sqlDoesntWork = false;
				try
				{
					english = LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Value;
				}
				catch
				{
					sqlDoesntWork = true;
				}
				if(sqlDoesntWork)
				{
					textObj.text = "bardzo dlugi tekst sprawdzajacy zawijanie";
					//StartCoroutine(func(textObj));
					//Invoke("Dissappear",2f);
				}
               	else
				{
					AudioClip clip = Resources.Load<AudioClip>(english);
					if(clip!=null)
						AudioSource.PlayClipAtPoint(clip,transform.position);
	                textObj.text = english + " - " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Key;
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
