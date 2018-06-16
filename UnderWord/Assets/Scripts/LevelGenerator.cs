using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System;
using System.IO;

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
	public static List<KeyValuePair<string, string>> knownVocabulary = new List<KeyValuePair<string, string>>();
    public static List<KeyValuePair<string, string>> vocabularyToLearn = new List<KeyValuePair<string, string>>();
	private int lvl;
    private void Start()
    {
        GameState gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
		lvl = gameState.levelCounter;

		//seedVocabulary ();

        if (gameState.isNewLevel)
        {
            generator = null;
            if(lvl == 0)
            {
                numberOfRooms = 4;
                numberOfScrolls = 3;
                numberOfEnemies = 0;
            }
            else
            {
                numberOfRooms = lvl / 5 + 7;
                numberOfEnemies = lvl / 5 + 2;
                numberOfScrolls = 5;
            }
            gameState.isNewLevel = false;
			gameState.levelCounter++;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector2(0, 0);
        }
        if (generator == null)
        {
            DontDestroyOnLoad(gameObject);
            generator = this;
            try
            {
                generator.Generate();
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
			getToDB(gameState);
        }
        else if (generator != this)
        {
            print("Log: Number of rooms: " + generator.takenPositions.Count);
            Destroy(gameObject);
        }
        print("Log: Drawing map...");
        generator.DrawMap();
        print("Log: Drawing map finished.");
        
    }

	private void seedVocabulary()
	{
		vocabularyToLearn.Add(new KeyValuePair<String, String>("biec" , "run"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("iść", "go"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("atakować", "attack"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("pies", "dog"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("kot", "cat"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("papuga", "parrot"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("koń", "horse"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("mysz", "mouse"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("homar", "lobster"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("masło", "butter"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("ser", "cheese"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("szynka", "ham"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("drewno", "wood"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("trawnik", "lawn"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("sok", "juice"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("miecz", "sword"));
		vocabularyToLearn.Add(new KeyValuePair<String, String>("biurko", "desk"));
	}
	


    private void getToDB(GameState gameState)
    {
        string filepath = Application.persistentDataPath + "/Vocabulary.db"; //Path to database
        print(filepath);
        if(!File.Exists(filepath))
        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/Vocabulary.db");
            while (!loadDB.isDone) { }

            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        string conn = "URI=file:" + filepath;
        using (var dbconn = new SqliteConnection(conn))
        {

            dbconn.Open(); //Open connection to the database.
            
            using (var dbcmd = dbconn.CreateCommand())
            {
                int level = gameState.levelCounter;
                string sqlQuery = "SELECT id, polish, english " + "FROM Vocabulary where id <= " + level + ";";
                print(sqlQuery);
                dbcmd.CommandText = sqlQuery;
                using (var reader = dbcmd.ExecuteReader())
                {
                    if (vocabularyToLearn.Count > 0)
                    {
                        knownVocabulary.AddRange(vocabularyToLearn.GetRange(0, numberOfScrolls));
                        vocabularyToLearn.Clear();
                    }
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string polish = reader.GetString(1);
                        string english = reader.GetString(2);

                        Debug.Log("id = " + id + "  polish =" + polish + " english =" + english);
                        if (id == level)
                            vocabularyToLearn.Add(new KeyValuePair<string, string>(polish, english));
                    }
                }
            }
        }
        
        
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

            if(NumberOfNeighbours(checkPos, takenPositions) > 1 && UnityEngine.Random.value > randomCompare)
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
            int index = Mathf.RoundToInt(UnityEngine.Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool upDown = (UnityEngine.Random.value < 0.5f);
            bool positive = (UnityEngine.Random.value < 0.5f);
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
                index = Mathf.RoundToInt(UnityEngine.Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);
            
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool upDown = (UnityEngine.Random.value < 0.5f);
            bool positive = (UnityEngine.Random.value < 0.5f);
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
		Debug.Log("Debug: " + takenPositions.Count);
        foreach (var position in takenPositions)
            permutatedPositions.Add(UnityEngine.Random.value, position);
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
        SetItemPositionsList(ref generator.scrollsPositions, numberOfScrolls);
    }

    void SetFightsPositions()
    {
		//if(lvl > 0)
    		SetItemPositionsList(ref generator.fightsPositions, numberOfEnemies);
    }


    private void DrawMap()
    {
        GameObject obj;
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
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Bottom")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(Bottom, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomLeft:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("BottomLeft")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(BottomLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomLeftRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("BottomLeftRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(BottomLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.BottomRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("BottomRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(BottomRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.Left:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Left")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(Left, drawPos, Quaternion.identity);
                    break;
                case DoorType.LeftRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("LeftRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(LeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.Right:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Right")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(Right, drawPos, Quaternion.identity);
                    break;
                case DoorType.Top:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("Top")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(Top, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottom:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopBottom")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopBottom, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomLeft:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopBottomLeft")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopBottomLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomLeftRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopBottomLeftRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopBottomLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopBottomRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopBottomRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopBottomRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopLeft:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopLeft")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopLeft, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopLeftRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopLeftRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopLeftRight, drawPos, Quaternion.identity);
                    break;
                case DoorType.TopRight:
                    obj = GameObject.Instantiate(Resources.Load<GameObject>("TopRight")) as GameObject;
                    obj.transform.position = drawPos;
                    //Instantiate(TopRight, drawPos, Quaternion.identity);
                    break;
            }
            InstantiateSurroundings(x, y);
        }
        foreach (var position in generator.scrollsPositions)
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Scroll")) as GameObject;
            obj.transform.position = position;
            //Instantiate(Scroll, position, Quaternion.identity);
        }
        foreach (var position in generator.fightsPositions)
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>("FightTrigger")) as GameObject;
            obj.transform.position = position;
            print("Instantiated fight trigger at position: (" + position.x + "," + position.y + ")");
            //Instantiate(Fight, position, Quaternion.identity);
        }
    }

    private void InstantiateSurroundings(int x, int y)
    {
        GameObject obj;
        if (x - 1 >= 0 && rooms[x - 1, y] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x - 1) * 11, rooms[x, y].position.y * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (x - 1 >= 0 && y - 1 >= 0 && rooms[x - 1, y - 1] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x - 1) * 11, (rooms[x, y].position.y - 1) * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (y - 1 >= 0 && rooms[x, y - 1] == null)
        {
            Vector2 tmp = new Vector2(rooms[x, y].position.x * 11, (rooms[x, y].position.y - 1) * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (x + 1 < gridSizeX * 2 && y - 1 >= 0 && rooms[x + 1, y - 1] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x + 1) * 11, (rooms[x, y].position.y - 1) * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (x + 1 < gridSizeX * 2 && rooms[x + 1, y] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x + 1) * 11, rooms[x, y].position.y * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (x + 1 < gridSizeX * 2 && y + 1 < gridSizeY * 2 && rooms[x + 1, y + 1] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x + 1) * 11, (rooms[x, y].position.y + 1) * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (y + 1 < gridSizeY * 2 && rooms[x, y + 1] == null)
        {
            Vector2 tmp = new Vector2(rooms[x, y].position.x * 11, rooms[x, y].position.y * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
        if (x - 1 >= 0 && y + 1 < gridSizeY * 2 && rooms[x - 1, y + 1] == null)
        {
            Vector2 tmp = new Vector2((rooms[x, y].position.x - 1) * 11, (rooms[x, y].position.y + 1) * 11);
            obj = GameObject.Instantiate(Resources.Load<GameObject>("Surroundings")) as GameObject;
            obj.transform.position = tmp;
        }
    }

}
