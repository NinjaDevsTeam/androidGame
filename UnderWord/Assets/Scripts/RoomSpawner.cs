using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public OpeningDirection openingDirection;

    private RoomTemplates templates;
    private static BoardState boardState;
    private int rand;
    private bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        boardState = GameObject.FindGameObjectWithTag("BoardState").GetComponent<BoardState>();
        Invoke("Spawn", 5f);
    }

    void Spawn ()
    {

        if (spawned)
            return;

        if (openingDirection == OpeningDirection.BottomDoor)
        {
            if (templates.counter == 0)
                rand = 0;
            else
            {
                rand = Random.Range(1, templates.bottomRooms.Length);
                templates.counter--;
            }
            Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);

            //boardState.roomInfos.Add(RoomInfo.Create(openingDirection, rand, new Vector3(transform.position.x, transform.position.y, transform.position.z)));
        }
        else if (openingDirection == OpeningDirection.TopDoor)
        {
            if (templates.counter == 0)
                rand = 0;
            else
            {
                rand = Random.Range(1, templates.topRooms.Length);
                templates.counter--;
            }
            Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            //boardState.roomInfos.Add(RoomInfo.Create(openingDirection, rand, new Vector3(transform.position.x, transform.position.y, transform.position.z)));
        }
        else if (openingDirection == OpeningDirection.LeftDoor)
        {
            if (templates.counter == 0)
                rand = 0;
            else
            {
                rand = Random.Range(1, templates.leftRooms.Length);
                templates.counter--;
            }
            Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            //boardState.roomInfos.Add(RoomInfo.Create(openingDirection, rand, new Vector3(transform.position.x, transform.position.y, transform.position.z)));
        }
        else if (openingDirection == OpeningDirection.RightDoor)
        {
            if (templates.counter == 0)
                rand = 0;
            else
            {
                rand = Random.Range(1, templates.rightRooms.Length);
                templates.counter--;
            }
            Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            //boardState.roomInfos.Add(RoomInfo.Create(openingDirection, rand, new Vector3(transform.position.x, transform.position.y, transform.position.z)));
        }
        spawned = true;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            if (!other.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}

public enum OpeningDirection
{
    BottomDoor,
    TopDoor,
    LeftDoor,
    RightDoor
}
