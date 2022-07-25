using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [ReadOnly]
    public InventoryItemData data;

    public RectTransform demoObjectTransform;
    public Image icon;
    public TMP_Text amount;
    public Image lockItemImage;

    private int _amount;
    private ItemObject _prefab;
    private GameObject _currentDemoObject;

    public void OnSelect(BaseEventData eventData)
    {
        if (_amount > 0)
        {
            ShopScreen.Instance.OnSelectSkill(this);
            _currentDemoObject = SimplePool.Spawn(_prefab.gameObject, 
                Helpers.GetScreenPointToWorldPointInRectangle(demoObjectTransform), 
                Quaternion.identity);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_currentDemoObject != null)
        {
            SimplePool.Despawn(_currentDemoObject);
        }
    }

    public void InitItemData(ItemObject obj)
    {
        _prefab = obj;
        this.data = obj.Data;
        _amount = this.data.maxCapacity;
        ResetUI();
    }

    public void OnBuyItem()
    {
        _amount--;
        ResetUI();
    }

    public void ResetUI()
    {
        icon.sprite = data.icon;
        amount.text = _amount.ToString();
        lockItemImage.gameObject.SetActive(_amount > 0 ? false : true);
    }
}
