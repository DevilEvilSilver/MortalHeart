using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : BaseDoor
{
    [SerializeField] protected LayerMask effectLayers;

    private RoomProperties _nextRoom;

    public void Init(RoomProperties nextRoom)
    {
        _nextRoom = nextRoom;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((effectLayers.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        _room.ExitRoom(_nextRoom);
    }
}
