using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    private Dictionary<string, InventoryItemStack> _itemDictionary;
    public List<InventoryItemStack> inventory { get; private set; }
    public int money { get; private set; }

    public int currentItemIndex { get; private set; }

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        InitState();
    }

    public void InitState()
    {
        inventory = new List<InventoryItemStack>();
        _itemDictionary = new Dictionary<string, InventoryItemStack>();
        money = 0;
        currentItemIndex = 0;
        GameplayScreen.instance.OnMoneyChange(money);
        InputManager.Instance.leftItemAction.performed += ctx =>
        {
            ChangeItem(-1);
        };
        InputManager.Instance.rightItemAction.performed += ctx =>
        {
            ChangeItem(1);
        };
        UpdatePlayerMoney(0);
        UpdateItemData();
    }

    private void ChangeItem(int offset)
    {
        if (inventory == null) return;

        currentItemIndex = Mathf.Clamp(currentItemIndex + offset, 0, inventory.Count - 1);
        UpdateItemData();
    }

    public InventoryItemStack Get(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData.id, out InventoryItemStack value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData.id, out InventoryItemStack value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItemStack newItem = new InventoryItemStack(itemData);
            inventory.Add(newItem);
            _itemDictionary.Add(itemData.id, newItem);
        }
        itemData.OnAdd();
        UpdateItemData();
    }

    public void Remove(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData.id, out InventoryItemStack value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                _itemDictionary.Remove(itemData.id);
            }
        }
        itemData.OnRemoved();
        UpdateItemData();
    }

    [Button("UseItem")]
    public void UseItem()
    {
        if (inventory[currentItemIndex] == null) return;

        var item = inventory[currentItemIndex];
        item.RemoveFromStack();

        if (item.stackSize == 0)
        {
            inventory.Remove(item);
            _itemDictionary.Remove(item.data.id);
        }
        item.data.OnUsed();
        UpdateItemData();
    }

    public void UpdatePlayerMoney(int change)
    {
        money += change;
        GameplayScreen.instance.OnMoneyChange(money);
    }

    private void UpdateItemData()
    {
        if (inventory[currentItemIndex] != null)
            GameplayScreen.instance.
                OnItemChange(inventory[currentItemIndex].data.icon, inventory[currentItemIndex].stackSize);
    }
    
}
