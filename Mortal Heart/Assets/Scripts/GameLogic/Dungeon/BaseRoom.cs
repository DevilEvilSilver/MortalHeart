using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;
using UnityEngine.SceneManagement;

public class BaseRoom : MonoBehaviour
{
    public enum RoomState
    {
        Spawned = 0, InCombat, Normal
    }

    [SerializeField] protected EntranceDoor[] entranceDoors;
    [SerializeField] protected ExitDoor[] exitDoors;
    [SerializeField] protected Transform[] spawnZones;
    [SerializeField] protected Transform[] enemyZones;
    [SerializeField] protected AllEnemyData enemies; 
    [SerializeField] protected AllItemData items; 
    [SerializeField] protected Vector2Int enemiesCount;

    protected EntranceDoor _currentEntrance;
    protected ExitDoor[] _currenExits;
    protected BaseEnemyController[] _enemyList;
    protected int _aliveEnemyCount;

    public RoomState CurrentRoomState { get; protected set; }
    protected ItemObject _reward;
    private CinemachineVirtualCamera _camera;
    private MainCharacterController _player;

    protected void Awake()
    {
        _aliveEnemyCount = 0;
        CurrentRoomState = RoomState.Spawned;
        _camera = Helpers.Camera.GetComponent<CinemachineVirtualCamera>();

        foreach (var door in entranceDoors)
        {
            door.Init(this);
        }
        foreach (var door in exitDoors)
        {
            door.Init(this);
        }
    }

    public void Init(GameObject player, int currentFloor, RoomProperties currentRoom, RoomProperties previousRoom)
    {

#if UNITY_EDITOR
        Debug.Log("Spawn room:" + currentRoom.index);
#endif

        // spawn pos
        int spawnPos = 0;
        if (currentRoom.index.x == previousRoom.index.x)
            spawnPos = 1;
        else if (currentRoom.index.x < previousRoom.index.x)
            spawnPos = 2;

        // doors
        foreach (var door in entranceDoors)
        {
            door.CloseDoor();
        }
        foreach (var door in exitDoors)
        {
            door.CloseDoor();
        }
        _currentEntrance = entranceDoors[Mathf.Clamp(spawnPos, 0, entranceDoors.Length)];
        _currentEntrance.OpenDoor();
        _currenExits = new ExitDoor[currentRoom.nextRooms.Count];
        int i = 0;
        foreach (var nextRoom in currentRoom.nextRooms)
        {
            _currenExits[i] = exitDoors[0];
            if (currentRoom.index.x == nextRoom.x)
                _currenExits[i] = exitDoors[1];
            else if (currentRoom.index.x < nextRoom.x)
                _currenExits[i] = exitDoors[2];

            _currenExits[i].Init(DungeonController.Instance.GetRoomProperties(nextRoom));
            i++;
        }

        // player
        _player = SpawnPlayer(player, spawnPos);

        // enemies
        if (currentRoom.type == RoomType.Normal)
        {
            int count = Random.Range(enemiesCount.x, enemiesCount.y + 1);
            _enemyList = new BaseEnemyController[count];
            _aliveEnemyCount = count;
            for (int j = 0; j < count; j++)
            {
                var zone = enemyZones[Random.Range(0, enemyZones.Length)];
                var e = SimplePool.Spawn(
                    GetRandomNormalEnemy(currentFloor).gameObject,
                    zone.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                    zone.rotation);

                _enemyList[j] = e.GetComponent<BaseEnemyController>();
                _enemyList[j].Init(OnEnemyDeath);
            }
            _reward = GetRandomNormalItem(currentFloor);
        }
    }

    private BaseEnemyController GetRandomNormalEnemy(int floor)
    {
        var list = enemies.EnemyList;
        List<BaseEnemyController> normals = new List<BaseEnemyController>();
        foreach (var enemydata in list)
        {
            if (enemydata.floor == floor && enemydata.type == EnemyType.Normal)
                normals.Add(enemydata.enemy);
        }
        return normals[Random.Range(0, normals.Count)];
    }

    private ItemObject GetRandomNormalItem(int floor)
    {
        var list = items.ItemList;
        List<ItemObject> normals = new List<ItemObject>();
        foreach (var itemdata in list)
        {
            if (itemdata.floor == floor && itemdata.type == ItemType.Normal)
                normals.Add(itemdata.item);
        }
        return normals[Random.Range(0, normals.Count)];
    }

    private void OnEnemyDeath()
    {
        _aliveEnemyCount--;
        GameController.Instance.playerData.EnemyKilled++;
        InventorySystem.Instance.UpdatePlayerMoney(10);
        if (_aliveEnemyCount <= 0)
        {
            SetNormalState();
            SimplePool.Spawn(_reward.gameObject, transform.position, Quaternion.identity);
            _player.Agent.enabled = false;
        }
    }

    [Button("Combat")]
    public void SetInCombatState()
    {
        CurrentRoomState = RoomState.InCombat;
        GameController.Instance.ChangeGameState(GameState.InCombat);

        _currentEntrance.CloseDoor();
        foreach (var door in _currenExits)
        {
            door.CloseDoor();
        }

        for (int i = 0; i < _enemyList.Length; i++)
        {
            _enemyList[i].isActive = true;
        }
    }

    [Button("Normal")]
    public void SetNormalState()
    {
        CurrentRoomState = RoomState.Normal;
        GameController.Instance.ChangeGameState(GameState.Interact);

        _currentEntrance.CloseDoor();
        foreach (var door in _currenExits)
        {
            door.OpenDoor();
        }
    }

    public void ExitRoom(RoomProperties nextRoom)
    {
        DungeonController.Instance.GoToNextRoom(nextRoom);
        if (nextRoom == null)
        {
            if (DungeonController.Instance.GoToNextFloor()) // check if reach final floor
                return;
            else
                SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
        }

        switch (nextRoom.type)
        {
            default:
            case RoomType.Normal:
            case RoomType.Boss:
                SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
                break;
            case RoomType.Shop:
                SceneManager.LoadScene(GameUtils.SceneName.SHOP, LoadSceneMode.Single);
                break;
        }
    }

    public MainCharacterController SpawnPlayer(GameObject player, int spawnPos = 0)
    {
        var index = Mathf.Clamp(spawnPos, 0, spawnZones.Length);
        var target = SimplePool.Spawn(player, spawnZones[index].position, spawnZones[index].transform.rotation);
        _camera.Follow = target.transform;

        return target.GetComponent<MainCharacterController>();
    }
}
