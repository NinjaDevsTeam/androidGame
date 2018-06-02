using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator generator;

    private Vector2 levelSize = new Vector2(4, 4);
    private RoomInfo[,] rooms;
    private List<Vector2> takenPositions = new List<Vector2>();
    public List<Vector2> scrollsPositions = new List<Vector2>();
    public List<Vector2> fightsPositions = new List<Vector2>();
    private int gridSizeX, gridSizeY;
    [Header("Parameters:")]
    public int numberOfRooms = 20;
    public int numberOfScrolls = 8;
    public int numberOfEnemies = 12;
    public GameObject Top, Bottom, Left, Right, TopBottom, TopLeft, TopRight,
        BottomLeft, BottomRight, LeftRight, TopBottomLeft, TopBottomRight, TopLeftRight,
        BottomLeftRight, TopBottomLeftRight, Scroll, Fight;

    private void Awake()
    {
        if (generator == null)
        {
            DontDestroyOnLoad(gameObject);
            generator = this;
            generator.Generate();
        }
        else if(generator != this)
        {
            print("Log: Number of rooms: " + generator.takenPositions.Count);
            Destroy(gameObject);
        }        
        print("Log: Drawing map...");
        generator.DrawMap();
        print("Log: Drawing map finished.");
    }

    private void Generate()
    {
        print("Log: Level generation started...");

        if (numberOfRooms >= (levelSize.x * 2) * (levelSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((levelSize.x * 2) * (levelSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(levelSize.x);
        gridSizeY = Mathf.RoundToInt(levelSize.y);
        CreateRooms();
        SetRoomDoors();
        SetScrollsPositions();
        SetFightsPositions();

        print("Log: Level generation finished.");
        
    }

    private void CreateRooms()
    {
        rooms = new RoomInfo[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new RoomInfo(Vector2.zero, 1, false);
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
                    checkPos = SelectivePosition();
                    it++;
                } while (NumberOfNeighbours(checkPos, takenPositions) > 1 && it < 100);
            }
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new RoomInfo(checkPos, 0, false);
            takenPositions.Insert(0, checkPos);
        }
        
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool upDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (upDown)
            {
                if (positive) y++;
                else y--;
            }
            else
            {
                if (positive) x++;
                else x--;
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    Vector2 SelectivePosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);
            
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool upDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (upDown)
            {
                if (positive) y++;
                else y--;
            }
            else
            {
                if (positive) x++;
                else x--;
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
            print("Error: could not find position with only one neighbour");
        return checkingPos;
    }

    int NumberOfNeighbours(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right)) ret++;
        if (usedPositions.Contains(checkingPos + Vector2.left)) ret++;
        if (usedPositions.Contains(checkingPos + Vector2.up)) ret++;
        if (usedPositions.Contains(checkingPos + Vector2.down)) ret++;
        return ret;
    }

    void SetRoomDoors()
    {
        for(int x = 0; x < gridSizeX*2; x++)
            for(int y=0; y < gridSizeY*2;y++)
            {
                if (rooms[x, y] == null) continue;

                Vector2 gridPosition = new Vector2(x, y);
                rooms[x, y].doorType = 0;
                if ((y - 1 >= 0) && (rooms[x, y - 1] != null))
                    rooms[x, y].doorType += 2;
                if ((y + 1 < gridSizeY * 2) && (rooms[x, y + 1] != null))
                    rooms[x, y].doorType += 1;
                if ((x - 1 >= 0) && (rooms[x - 1, y] != null))
                    rooms[x, y].doorType += 4;
                if ((x + 1 < gridSizeX * 2) && (rooms[x + 1, y] != null))
                    rooms[x, y].doorType += 8;
            }
    }

    void SetItemPositionsList(ref List<Vector2> positions, int limit)
    {
        SortedList<float, Vector2> permutatedPositions = new SortedList<float, Vector2>();
        print("Debug: " + takenPositions.Count);
        foreach (var position in takenPositions)
            permutatedPositions.Add(Random.value, position);
        print("Debug: " + permutatedPositions.Count);
        for (int i = 0; i < limit; i++)
        {
            int x = (int)permutatedPositions.Values[i].x;
            int y = (int)permutatedPositions.Values[i].y;
            if (x == 0 && y == 0)
            {
                x = (int)permutatedPositions.Values[limit].x;
                y = (int)permutatedPositions.Values[limit].y;
            }
            Vector2 drawPos = new Vector2(x * 11, y * 11);
            positions.Add(drawPos);
        }
    }

    void SetScrollsPositions()
    {
        SetItemPositionsList(ref scrollsPositions, numberOfScrolls);
    }

    void SetFightsPositions()
    {
        SetItemPositionsList(ref fightsPositions, numberOfEnemies);
    }


    private void DrawMap()
    {
        print("Log: Number of rooms: "+takenPositions.Count);
        foreach(Vector2 position in takenPositions)
        {
            int x = (int)position.x + gridSizeX;
            int y = (int)position.y + gridSizeY;
            RoomInfo room = rooms[x, y];
            Vector2 drawPos = new Vector2(room.position.x * 11, room.position.y * 11);
            print("Log: Drawing room at position " + "(" + drawPos.x + "," + drawPos.y + ")");
            switch (room.doorType)
            {
                case DoorType.Bottom:
                    Instantiate(Bottom, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomLeft:
                    Instantiate(BottomLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomLeftRight:
                    Instantiate(BottomLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomRight:
                    Instantiate(BottomRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.Left:
                    Instantiate(Left, drawPos, Quaternion.identity);
                    break;
                case DoorType.LeftRight:
                    Instantiate(LeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.Right:
                    Instantiate(Right, drawPos, Quaternion.identity);
                    break;
                case DoorType.Top:
                    Instantiate(Top, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottom:
                    Instantiate(TopBottom, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomLeft:
                    Instantiate(TopBottomLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomLeftRight:
                    Instantiate(TopBottomLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomRight:
                    Instantiate(TopBottomRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopLeft:
                    Instantiate(TopLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopLeftRight:
                    Instantiate(TopLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopRight:
                    Instantiate(TopRight, drawPos, Quaternion.identity);
                    break;
            }
        }
        foreach (var position in scrollsPositions)
            Instantiate(Scroll, position, Quaternion.identity);
        foreach (var position in fightsPositions)
            Instantiate(Fight, position, Quaternion.identity);
    }
}
