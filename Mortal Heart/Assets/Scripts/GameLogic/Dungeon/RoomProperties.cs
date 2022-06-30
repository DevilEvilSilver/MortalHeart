using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RoomProperties
{
    public Vector2Int index;
    public bool isActive;
    public List<Vector2Int> nextRooms;
    public RoomType type;

    public RoomProperties(int x, int y)
    {
        type = RoomType.Normal;
        index = new Vector2Int(x, y);
        isActive = false;

        nextRooms = new List<Vector2Int>();
    }
}

public enum RoomType
{
    Normal = 0, Shop
}