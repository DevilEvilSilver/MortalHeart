using UnityEngine;
using System;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "AllItemData", menuName = "ScriptableObjects/AllItemData")]
public class AllItemData : ScriptableObject
{
    [TableList]
    public ItemData[] ItemList;
}

[Serializable]
public class ItemData
{
    public ItemObject item;
    public ItemType type;
    public int floor;
}

public enum ItemType
{
    Normal = 0, Rare, Boss
}