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

    protected EntranceDoor _currentEntrance;
    protected ExitDoor[] _currenExits;

    protected RoomState _roomState;
    private CinemachineVirtualCamera _camera;

    protected void Awake()
    {
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
        SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
    }

    public void SpawnPlayer(GameObject player, int spawnPos = 0)
    {
        var index = Mathf.Clamp(spawnPos, 0, spawnZones.Length);
        var target = SimplePool.Spawn(player, spawnZones[index].position, spawnZones[index].transform.rotation);
        _camera.Follow = target.transform;
    }
}
