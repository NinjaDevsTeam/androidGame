using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public OpeningDirection openingDirection;

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn ()
    {
        if (spawned)
            return;

		if(openingDirection == OpeningDirection.BottomDoor)
        {
            rand = Random.Range(0, templates.bottomRooms.Length);
            Instantiate(templates.bottomRooms[rand], new Vector3(transform.position.x+7, transform.position.y+2), templates.bottomRooms[rand].transform.rotation);
        }
        else if(openingDirection == OpeningDirection.TopDoor)
        {
            rand = Random.Range(0, templates.topRooms.Length);
            Instantiate(templates.topRooms[rand], new Vector3(transform.position.x + 7, transform.position.y + 2), templates.topRooms[rand].transform.rotation);
        }
        else if(openingDirection == OpeningDirection.LeftDoor)
        {
            rand = Random.Range(0, templates.leftRooms.Length);
            Instantiate(templates.leftRooms[rand], new Vector3(transform.position.x + 7, transform.position.y + 2), templates.leftRooms[rand].transform.rotation);
        }
        else if(openingDirection == OpeningDirection.RightDoor)
        {
            rand = Random.Range(0, templates.rightRooms.Length);
            Instantiate(templates.rightRooms[rand], new Vector3(transform.position.x + 7, transform.position.y + 2), templates.rightRooms[rand].transform.rotation);
        }
        spawned = true;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            if(!other.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
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
