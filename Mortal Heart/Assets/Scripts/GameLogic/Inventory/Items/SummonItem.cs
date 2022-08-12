using UnityEngine;
using System.Collections;

public class SummonItem : InventoryItemData
{
    [SerializeField] private Sparkling summon;

    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        var pos = Object.FindObjectOfType<MainCharacterController>().transform.position;
        SimplePool.Spawn(summon.gameObject, pos, Quaternion.identity);
    }

    public override void OnRemoved()
    {

    }
}
