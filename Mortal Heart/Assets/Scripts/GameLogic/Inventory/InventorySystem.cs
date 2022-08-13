using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class InventorySystem : SingletonMonoBehaviour<InventorySystem>
{
    private readonly int maxUsableCapacity = 5;

    public AudioClip item;
    public AudioClip coin;

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
        InputManager.Instance.leftItemAction.performed += ctx =>
        {
            ChangeItem(-1);
        };
        InputManager.Instance.rightItemAction.performed += ctx =>
        {
            ChangeItem(1);
        };
        OnChangeScene();
    }

    public void OnChangeScene()
    {
        if (SceneManager.GetActiveScene().name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            UpdatePlayerMoney(0);
            UpdateItemData();
        }
    }

    private void ChangeItem(int offset)
    {
        if (inventory == null || inventory.Count <= 0) return;

        currentItemIndex = Mathf.Clamp(currentItemIndex + offset, 0, inventory.Count - 1);
        Debug.Log(currentItemIndex);
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

    public bool CheckAvailbleSlot(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData.id, out InventoryItemStack value))
        {
            if (value.stackSize < itemData.maxCapacity)
                return true;
        }
        else if (inventory.Count < maxUsableCapacity)
        {
            return true;
        }
        return false;
    }

    public InventoryItemData GetCurrentItem()
    {
        if (inventory.Count <= 0 || inventory[currentItemIndex] == null) return null;

        return inventory[currentItemIndex].data;
    }

    public bool Add(InventoryItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData.id, out InventoryItemStack value))
        {
            if (value.AddToStack())
            {
                AudioManager.Instance.PlaySoundEffect(item);

                itemData.OnAdd();
                UpdateItemData();
                return true;
            }
        }
        else if (inventory.Count < maxUsableCapacity)
        {
            InventoryItemStack newItem = new InventoryItemStack(itemData);
            inventory.Add(newItem);
            _itemDictionary.Add(itemData.id, newItem);

            AudioManager.Instance.PlaySoundEffect(item);

            itemData.OnAdd();
            UpdateItemData();
            return true;
        }
        return false;
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
        if (inventory.Count <= 0 || inventory[currentItemIndex] == null) return;

        var item = inventory[currentItemIndex];
        item.RemoveFromStack();

        if (item.stackSize == 0)
        {
            currentItemIndex = 0;
            inventory.Remove(item);
            _itemDictionary.Remove(item.data.id);
        }
        AudioManager.Instance.PlaySoundEffect(this.item);

        item.data.OnUsed();
        UpdateItemData();
    }

    public void UpdatePlayerMoney(int change)
    {
        money += change;
        if (change != 0)
            AudioManager.Instance.PlaySoundEffect(coin);

        if (SceneManager.GetActiveScene().name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            GameplayScreen.Instance.OnMoneyChange(money);
        }  
    }

    private void UpdateItemData()
    {
        if (SceneManager.GetActiveScene().name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            if (inventory.Count > 0 && inventory[currentItemIndex] != null)
                GameplayScreen.Instance.
                    OnItemChange(inventory[currentItemIndex].data.icon, inventory[currentItemIndex].stackSize);
            else
                GameplayScreen.Instance.OnNoneItem();
        }
    }
    
}
