using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private Vector2 levelSize = new Vector2(4, 4);
    private RoomInfo[,] rooms;
    private List<Vector2> takenPositions = new List<Vector2>();
    private int gridSizeX, gridSizeY, numberOfRooms = 20;
    public GameObject roomWhiteObj;

    private void Start()
    {
        if(numberOfRooms >= (levelSize.x * 2) * (levelSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((levelSize.x * 2) * (levelSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(levelSize.x);
        gridSizeY = Mathf.RoundToInt(levelSize.y);
        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    private void CreateRooms()
    {
        rooms = new RoomInfo[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new RoomInfo(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        for(int i=0;i<numberOfRooms-1;i++)
        {
            float randomPerc = ((float)i / ((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            checkPos = NewPosition();

            if(NumberOfNeighbours(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int it = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbours(checkPos, takenPositions) > 1 && iterations < 100);


                rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new RoomInfo(checkPos, 0);
                takenPositions.Insert(0, checkPos);
            }
        }
    }
}
