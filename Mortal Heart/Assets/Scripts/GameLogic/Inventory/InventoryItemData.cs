using UnityEngine;
using System.Collections;

public class InventoryItemData
{
    public string id;
    public string displayName;
    public Sprite icon;
    public int maxCapacity;

    [Header("Usable")]
    public AnimationClip anim;
    public float delay;

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
