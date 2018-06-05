using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    public static int numberOfPickedScrolls = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            try
            {
                
                LevelGenerator levGen =
                GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
                levGen.scrollsPositions.Remove(gameObject.transform.position);
                string english = LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Value;
			
				AudioClip clip = Resources.Load<AudioClip>(english);
				if(clip!=null)
					AudioSource.PlayClipAtPoint(clip,transform.position);
				var textObj = GameObject.FindObjectOfType<Text>();
                textObj.text = english + " - " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Key;
				StartCoroutine(func(textObj));
                print("zebrane slowko: " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Value.ToUpper() + " means: " + LevelGenerator.vocabularyToLearn[numberOfPickedScrolls].Key.ToUpper());
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
	private IEnumerator func(Text textObj)
	{
		yield return new WaitForSeconds(0.5f);
		textObj.text = "";
	}
}
