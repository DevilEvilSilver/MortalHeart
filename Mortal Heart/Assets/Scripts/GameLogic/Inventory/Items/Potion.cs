using UnityEngine;
using System.Collections;

public class Potion : InventoryItemData
{
    public override void OnAdd()
    {

    }

    public override void OnUsed()
    {
        Object.FindObjectOfType<MainCharacterController>().ChangeHealth(25f);
    }

    public override void OnRemoved()
    {

    }
}
