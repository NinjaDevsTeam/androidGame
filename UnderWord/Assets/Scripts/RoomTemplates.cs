using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public int chance;
    public int counter;

    private int chanceCounter = 0;


    private void FixedUpdate()
    {
        if (chanceCounter == chance)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Battle Stage");
        chanceCounter++;
    }
}
