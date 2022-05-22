using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : BaseDoor
{
    [SerializeField] protected LayerMask _effectLayers;
    [SerializeField] protected bool _isCombatRoom;

    private void OnTriggerEnter(Collider collision)
    {
        if ((_effectLayers.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        if (_isCombatRoom)
            _room.SetInCombatState();
        else
            _room.SetNormalState();
    }
}
