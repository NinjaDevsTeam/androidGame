using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState : MonoBehaviour {

    public List<RoomInfo> roomInfos;
    public int width;
    public int height;
    public RoomType[,] board;
    public Vector2Int startPosition;
    // Use this for initialization
    private void Awake()
    {       
        DontDestroyOnLoad(gameObject);
    }

    private void GenerateBoard()
    {
        board = new RoomType[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                board[i, j] = RoomType.Unninitialized;

        startPosition = new Vector2Int(Random.Range(1, width-1), Random.Range(1, height-1));
        board[startPosition.x, startPosition.y] = RoomType.TopBottomLeftRight;

    }

    private void GenerateBoardRec(ref RoomType[,] board, RoomType roomType, Vector2Int currentPosition)
    {
        switch(roomType)
        {
            case RoomType.Top:
                if(board[currentPosition.x, currentPosition.y + 1] == RoomType.Unninitialized)
                {
                    RoomType newType = Random.Range(0,)
                }
        }
    }
}

public enum RoomType
{
    Top = 1,
    Right = 2,
    Bottom = 4,
    Left = 8,    
    TopBottom = 5,
    TopLeft = 9,
    TopRight = 3,
    BottomLeft = 12,
    BottomRight = 7,
    LeftRight = 10,
    TopBottomLeftRight = 15,
    Unninitialized = 0
}
