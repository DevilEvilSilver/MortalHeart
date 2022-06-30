using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DungeonController : SingletonMonoBehaviour<DungeonController>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private BaseRoom _roomPrefab;
    [SerializeField] private int iterateTimes;
    [SerializeField] private int width, height;

    //private int _currentFloor;
    private RoomProperties _previousRoom;
    private RoomProperties _currentRoom;
    private RoomProperties[,] _map;

    private bool _isInit = false;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        _map = new RoomProperties[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _map[i, j] = new RoomProperties(i, j);
            }
        }
    }

    public void InitDungeon()
    {
        _isInit = true;
        _previousRoom = new RoomProperties(width / 2, -2);
        _currentRoom = new RoomProperties(width / 2, -1);
        _currentRoom.isActive = true;
        GenerateDungeon(_currentRoom);
    }

    public void GoToNextRoom(RoomProperties nextRoom)
    {
        _previousRoom = _currentRoom;
        _currentRoom = nextRoom;
    }

    public RoomProperties GetRoomProperties(Vector2Int index)
    {
        if (index.y > -1 && index.y < height)
            return _map[index.x, index.y];
        return null;
    }

    public void GenerateDungeon(RoomProperties startRoom)
    {
        int[] secondRoomXs = new int[3];
        secondRoomXs[0] = width / 2;
        secondRoomXs[1] = Random.Range(0, width / 2);
        secondRoomXs[2] = Random.Range(width / 2 + 1, width);

        for (int i = 0; i < iterateTimes; i++)
        {
            var index = secondRoomXs[Random.Range(0, secondRoomXs.Length)];
            _map[index, 0].isActive = true;
            startRoom.nextRooms.Add(new Vector2Int(index, 0));

            IterateNextRoom(_map[index, 0]);
        }
    }

    public void IterateNextRoom(RoomProperties previous)
    {
        //Debug.Log(previous.index);

        var xPrevious = previous.index.x;
        var yPrevious = previous.index.y;
        if (yPrevious > height - 2)
        {
            previous.nextRooms.Add(new Vector2Int(width / 2, height));
            return;
        }

        int xLeft = xPrevious - 1 > 0 ? xPrevious - 1 : 0;
        int xRight = xPrevious + 1 < width - 1 ? xPrevious + 1 : width - 1;
        for (int i = 0; i < width; i++)
        {
            if (_map[i, yPrevious].isActive)
            {
                for (int j = 0; j < _map[i, yPrevious].nextRooms.Count; j++)
                {
                    if (xPrevious > i)
                        xLeft = xLeft > _map[i, yPrevious].nextRooms[j].x ? xLeft : _map[i, yPrevious].nextRooms[j].x;
                    else if (xPrevious < i)
                        xRight = xRight < _map[i, yPrevious].nextRooms[j].x ? xRight : _map[i, yPrevious].nextRooms[j].x;
                }
            }
        }

        var index = Random.Range(xLeft, xRight + 1);
        _map[index, yPrevious + 1].isActive = true;
        previous.nextRooms.Add(new Vector2Int(index, yPrevious + 1));
        IterateNextRoom(_map[index, yPrevious + 1]);
    }

    [Button("SpawnRoom")]
    public void SpawnRoom()
    {
        if (!_isInit)
            InitDungeon();

        var room = SimplePool.Spawn(_roomPrefab.gameObject, Vector3.zero, Quaternion.identity);
        room.GetComponent<BaseRoom>().Init(_player, _currentRoom, _previousRoom);
    }
}
