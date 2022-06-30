using UnityEngine;
using System.Collections;

public class InventoryItemData
{
    public string id;
    public string displayName;
    public Sprite icon;

    public virtual void OnAdd()
    {

    }

    public virtual void OnUsed()
    {

    }

    public virtual void OnRemoved()
    {

    }
}
