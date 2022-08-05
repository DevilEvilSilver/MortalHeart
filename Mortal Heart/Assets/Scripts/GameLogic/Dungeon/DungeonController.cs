using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class DungeonController : SingletonMonoBehaviour<DungeonController>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private BaseRoom _roomPrefab;
    [SerializeField] private int iterateTimes;
    [SerializeField] private float eliteRate;
    [SerializeField] private int width, height;

    private int _currentFloor;
    private RoomProperties _previousRoom;
    private RoomProperties _currentRoom;
    public RoomProperties[,] Map { get; private set; }

    private bool _isInit = false;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        Map = new RoomProperties[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Map[i, j] = new RoomProperties(i, j, RoomType.Normal);
            }
        }
    }

    public void InitDungeon()
    {
        _currentFloor = 1;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Map[i, j].isActive = false;
                Map[i, j].type = RoomType.Normal;
            }
        }
        _isInit = true;
        _previousRoom = new RoomProperties(width / 2, -2, RoomType.Normal);
        _currentRoom = new RoomProperties(width / 2, -1, RoomType.Normal);
        _currentRoom.isActive = true;
        GenerateDungeon(_currentRoom);
    }

    public void GoToNextRoom(RoomProperties nextRoom)
    {
        _previousRoom = _currentRoom;
        _currentRoom = nextRoom;
    }

    public bool GoToNextFloor()
    {
        _isInit = false;
        _currentFloor++;

        if (_currentFloor > 1)
        {
            SceneManager.LoadScene(GameUtils.SceneName.RESULT, LoadSceneMode.Single);
            return true;
        }
        else
            return false;

    }

    public RoomProperties GetRoomProperties(Vector2Int index)
    {
        if (index.y > -1 && index.y < height)
            return Map[index.x, index.y];
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
            Map[index, 0].isActive = true;
            startRoom.nextRooms.Add(new Vector2Int(index, 0));

            IterateNextRoom(Map[index, 0]);
        }
    }

    public void IterateNextRoom(RoomProperties previous)
    {
        //Debug.Log(previous.index);

        var xPrevious = previous.index.x;
        var yPrevious = previous.index.y;
        if (yPrevious > height - 2)
        {
            previous.nextRooms.Add(new Vector2Int(xPrevious, yPrevious + 1));
            previous.type = RoomType.Boss;

            return;
        }
        if (yPrevious == height - 2)
        {
            Map[width / 2, yPrevious + 1].isActive = true;
            previous.nextRooms.Add(new Vector2Int(width / 2, yPrevious + 1));
            IterateNextRoom(Map[width / 2, yPrevious + 1]);
            previous.type = RoomType.Shop;
            return;
        }

        int xLeft = xPrevious - 1 > 0 ? xPrevious - 1 : 0;
        int xRight = xPrevious + 1 < width - 1 ? xPrevious + 1 : width - 1;
        for (int i = 0; i < width; i++)
        {
            if (Map[i, yPrevious].isActive)
            {
                for (int j = 0; j < Map[i, yPrevious].nextRooms.Count; j++)
                {
                    if (xPrevious > i)
                        xLeft = xLeft > Map[i, yPrevious].nextRooms[j].x ? xLeft : Map[i, yPrevious].nextRooms[j].x;
                    else if (xPrevious < i)
                        xRight = xRight < Map[i, yPrevious].nextRooms[j].x ? xRight : Map[i, yPrevious].nextRooms[j].x;
                }
            }
        }

        var index = Random.Range(xLeft, xRight + 1);
        Map[index, yPrevious + 1].isActive = true;
        Map[index, yPrevious + 1].type = Random.Range(0f, 100f) < eliteRate * 100f ? RoomType.Elite : RoomType.Normal;
        previous.nextRooms.Add(new Vector2Int(index, yPrevious + 1));
        IterateNextRoom(Map[index, yPrevious + 1]);
    }

    [Button("SpawnRoom")]
    public void SpawnRoom()
    {
        if (!_isInit)
            InitDungeon();

        var room = SimplePool.Spawn(_roomPrefab.gameObject, Vector3.zero, Quaternion.identity);
        room.GetComponent<BaseRoom>().Init(_player, _currentFloor, _currentRoom, _previousRoom);
    }

    public void SpawnShop()
    {
        if (!_isInit)
            InitDungeon();

        ShopScreen.Instance.InitState(_currentRoom);
    }
}
