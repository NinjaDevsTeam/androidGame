using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public Vector2 position;

    public int type;

    public bool doorTop, doorBot, doorLeft, doorRight;

    public RoomInfo(Vector2 position, int type)
    {
        this.position = position;
        this.type = type;
    }
}
