﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            try
            {
                LevelGenerator levGen =
                GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
                levGen.scrollsPositions.Remove(gameObject.transform.position);
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
}
