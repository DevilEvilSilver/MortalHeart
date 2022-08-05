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
    [SerializeField] protected AllEnemyData allNormalEnemies;
    [SerializeField] protected AllEnemyData allRareEnemies;
    [SerializeField] protected AllEnemyData allBossEnemies;
    [SerializeField] protected AllItemData allNormalItems;
    [SerializeField] protected AllItemData allRareItems;
    [SerializeField] protected AllItemData allBossItems;
    [SerializeField] protected int difficultValue;

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
        //Debug.Log("Spawn room:" + currentRoom.index);
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

        // enemies & reward
        if (currentRoom.type == RoomType.Normal)
        {
            var enemies = GetEnemiesBaseOnDiffValue(difficultValue, allNormalEnemies, currentFloor);
            _enemyList = new BaseEnemyController[enemies.Count];
            _aliveEnemyCount = enemies.Count;
            for (int j = 0; j < enemies.Count; j++)
            {
                var zone = enemyZones[Random.Range(0, enemyZones.Length)];
                var e = SimplePool.Spawn(
                    enemies[j].gameObject,
                    zone.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                    zone.rotation);

                _enemyList[j] = e.GetComponent<BaseEnemyController>();
                _enemyList[j].Init(OnEnemyDeath);
            }
            _reward = GetRandomItemFromList(allNormalItems, currentFloor);
        }
        else if (currentRoom.type == RoomType.Elite)
        {
            var eliteData = GetRandomEnemyFromList(allRareEnemies, currentFloor);
            var elite = SimplePool.Spawn(
                eliteData.enemy.gameObject,
                enemyZones[0].position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                enemyZones[0].rotation);

            var enemies = GetEnemiesBaseOnDiffValue(difficultValue - eliteData.diffValue
                , allNormalEnemies, currentFloor);
            _enemyList = new BaseEnemyController[enemies.Count + 1];
            _aliveEnemyCount = enemies.Count + 1;
            for (int j = 0; j < enemies.Count; j++)
            {
                var zone = enemyZones[Random.Range(0, enemyZones.Length)];
                var e = SimplePool.Spawn(
                    enemies[i].gameObject,
                    zone.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                    zone.rotation);

                _enemyList[j] = e.GetComponent<BaseEnemyController>();
                _enemyList[j].Init(OnEnemyDeath);
            }

            _enemyList[enemies.Count] = elite.GetComponent<BaseEnemyController>();
            _enemyList[enemies.Count].Init(OnEnemyDeath);

            _reward = GetRandomItemFromList(allRareItems, currentFloor);
        }
        else if (currentRoom.type == RoomType.Boss)
        {
            _enemyList = new BaseEnemyController[1];
            _aliveEnemyCount = 1;
            var elite = SimplePool.Spawn(
                GetRandomEnemyFromList(allBossEnemies, currentFloor).enemy.gameObject,
                enemyZones[0].position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                enemyZones[0].rotation);

            _enemyList[0] = elite.GetComponent<BaseEnemyController>();
            _enemyList[0].Init(OnEnemyDeath);

            _reward = GetRandomItemFromList(allBossItems, currentFloor);
        }
    }

    private List<BaseEnemyController> GetEnemiesBaseOnDiffValue(int diffvalue, AllEnemyData enemies, int floor)
    {
        var remaindiff = diffvalue;
        List<BaseEnemyController> result = new List<BaseEnemyController>();

        while (remaindiff > 0)
        {
            var e = GetRandomEnemyFromList(enemies, floor);
            remaindiff -= e.diffValue;
            result.Add(e.enemy);
        }

        return result;
    }

    private EnemyData GetRandomEnemyFromList(AllEnemyData enemies, int floor)
    {
        var list = enemies.EnemyList;
        List<EnemyData> normals = new List<EnemyData>();
        foreach (var enemydata in list)
        {
            if (enemydata.floor == floor)
                normals.Add(enemydata);
        }
        return normals[Random.Range(0, normals.Count)];
    }

    private ItemObject GetRandomItemFromList(AllItemData items, int floor)
    {
        var list = items.ItemList;
        List<ItemObject> normals = new List<ItemObject>();
        foreach (var itemdata in list)
        {
            if (itemdata.floor == floor)
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
            case RoomType.Elite:
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
