using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public Vector2 position;

    public int type;

    public bool containsScroll, containsFight;

    public DoorType doorType;

    public RoomInfo(Vector2 position, int type, bool containsScroll)
    {
        this.position = position;
        this.type = type;
        this.containsScroll = containsScroll;
    }
}

public enum DoorType
{
    NoDoors = 0,
    Top = 1,
    Bottom = 2,
    Left = 4,
    Right = 8,
    TopBottom = 3,
    TopLeft = 5,
    BottomLeft = 6,
    TopBottomLeft = 7,
    TopRight = 9,
    BottomRight = 10,
    TopBottomRight = 11,
    LeftRight = 12,
    TopLeftRight = 13,
    BottomLeftRight = 14,
    TopBottomLeftRight = 15
}
