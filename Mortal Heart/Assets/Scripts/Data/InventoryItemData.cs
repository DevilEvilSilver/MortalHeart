using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "ScriptableObjects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;

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
