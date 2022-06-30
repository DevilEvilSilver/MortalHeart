using System.Collections;
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
    [SerializeField] protected BaseEnemyController[] enemies;
    [SerializeField] protected Vector2Int enemiesCount;

    protected EntranceDoor _currentEntrance;
    protected ExitDoor[] _currenExits;
    protected int _enemiesCount;

    protected RoomState _roomState;
    private CinemachineVirtualCamera _camera;

    protected void Awake()
    {
        _enemiesCount = 0;
        _roomState = RoomState.Spawned;
        _camera = Camera.main.GetComponent<CinemachineVirtualCamera>();

        foreach (var door in entranceDoors)
        {
            door.Init(this);
        }
        foreach (var door in exitDoors)
        {
            door.Init(this);
        }
    }

    public void Init(GameObject player, RoomProperties currentRoom, RoomProperties previousRoom)
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
        SpawnPlayer(player, spawnPos);

        // enemies
        int count = Random.Range(enemiesCount.x, enemiesCount.y + 1);
        _enemiesCount = count;
        for (int j = 0; j < count; j++)
        {
            var zone = enemyZones[Random.Range(0, enemyZones.Length)];
            var e = SimplePool.Spawn(
                GetRandomEnemy().gameObject,
                zone.position,
                zone.rotation);

            e.GetComponent<BaseEnemyController>().Init(OnEnemyDeath);
        }
    }

    private BaseEnemyController GetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Length)];
    }

    private void OnEnemyDeath()
    {
        _enemiesCount--;
        InventorySystem.Instance.UpdatePlayerMoney(10);
        if (_enemiesCount <= 0)
        {
            SetNormalState();
        }
    }

    [Button("Combat")]
    public void SetInCombatState()
    {
        _roomState = RoomState.InCombat;

        _currentEntrance.CloseDoor();
        foreach (var door in _currenExits)
        {
            door.CloseDoor();
        }
    }

    [Button("Normal")]
    public void SetNormalState()
    {
        _roomState = RoomState.InCombat;

        _currentEntrance.CloseDoor();
        foreach (var door in _currenExits)
        {
            door.OpenDoor();
        }
    }

    public void ExitRoom(RoomProperties nextRoom)
    {
        DungeonController.Instance.GoToNextRoom(nextRoom);

        switch (nextRoom.type)
        {
            default:
            case RoomType.Normal:
                SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
                break;
        }
    }

    public void SpawnPlayer(GameObject player, int spawnPos = 0)
    {
        var index = Mathf.Clamp(spawnPos, 0, spawnZones.Length);
        var target = SimplePool.Spawn(player, spawnZones[index].position, spawnZones[index].transform.rotation);
        _camera.Follow = target.transform;
    }
}
