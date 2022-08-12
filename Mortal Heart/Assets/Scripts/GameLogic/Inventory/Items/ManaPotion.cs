using UnityEngine;
using System.Collections;

public class ManaPotion : InventoryItemData
{
    [SerializeField] private float healAmount;

    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        Object.FindObjectOfType<MainCharacterController>().ChangeMana(healAmount);
    }

    public override void OnRemoved()
    {

    }
}
