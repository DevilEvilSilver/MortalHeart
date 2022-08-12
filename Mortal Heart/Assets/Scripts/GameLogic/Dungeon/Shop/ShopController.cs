using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class ShopController : SingletonMonoBehaviour<ShopController>
{
    [SerializeField] protected AllItemData allNormalItems;
    [SerializeField] protected AllItemData allRareItems;
    [SerializeField] protected ItemUI[] normalItems;
    [SerializeField] protected ItemUI[] rareItems;

    protected override void Init()
    {
        foreach (var ui in normalItems)
        {
            ui.InitItemData(allNormalItems.ItemList[Random.Range(0, allNormalItems.ItemList.Length)].item);
        }
        foreach (var ui in rareItems)
        {
            ui.InitItemData(allRareItems.ItemList[Random.Range(0, allRareItems.ItemList.Length)].item);
        }
    }

    public bool IsAffordable(InventoryItemData data)
    {
        return InventorySystem.Instance.CheckAvailbleSlot(data)
            && InventorySystem.Instance.money >= data.price;
    }

    public void BuyItem(InventoryItemData data)
    {
        if (InventorySystem.Instance.Add(data))
            InventorySystem.Instance.UpdatePlayerMoney(-data.price);
    }
}
