using UnityEngine;
using System.Collections;

public class Potion : InventoryItemData
{
    [SerializeField] private float healAmount;

    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        Object.FindObjectOfType<MainCharacterController>().ChangeHealth(healAmount);
    }

    public override void OnRemoved()
    {

    }
}
