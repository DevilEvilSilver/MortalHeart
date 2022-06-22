using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    private Dictionary<InventoryItemData, InventoryItem> _itemDictionary;
    public List<InventoryItem> inventory { get; private set; }
    public int money { get; private set; }

    public int currentItemIndex { get; private set; }

    private InputAction leftAction;
    private InputAction rightAction;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        InitState();
    }

    public void InitState()
    {
        inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        money = 0;
        currentItemIndex = 0;
        GameplayScreen.instance.OnMoneyChange(money);
        leftAction.performed += ctx =>
        {
            ChangeItem(-1);
        };
        rightAction.performed += ctx =>
        {
            ChangeItem(1);
        };
    }

    private void ChangeItem(int offset)
    {
        if (inventory == null) return;

        currentItemIndex = Mathf.Clamp(currentItemIndex + offset, 0, inventory.Count - 1);
        if (inventory[currentItemIndex] != null)
        {
            GameplayScreen.instance.
                OnItemChange(inventory[currentItemIndex].data.icon, inventory[currentItemIndex].stackSize);
        }
    }

    public InventoryItem Get(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            _itemDictionary.Add(itemData, newItem);
        }
        itemData.OnAdd();
    }

    public void Remove(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                _itemDictionary.Remove(itemData);
            }
        }
        itemData.OnRemoved();
    }

    public void UseItem()
    {
        if (inventory[currentItemIndex] == null) return;

        inventory[currentItemIndex].RemoveFromStack();

        if (inventory[currentItemIndex].stackSize == 0)
        {
            inventory.Remove(inventory[currentItemIndex]);
            _itemDictionary.Remove(inventory[currentItemIndex].data);
        }
        inventory[currentItemIndex].data.OnUsed();
    }

    public void UpdatePlayerMoney(int change)
    {
        money += change;
        GameplayScreen.instance.OnMoneyChange(money);
    }

}
