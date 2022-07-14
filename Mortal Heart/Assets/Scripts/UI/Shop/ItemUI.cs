using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemUI : MonoBehaviour, ISelectHandler
{
    public InventoryItemData data;

    public Image icon;
    public TMP_Text amount;
    public Image lockItemImage;

    private Action<InventoryItemData> _onSelect;

    public void OnSelect(BaseEventData eventData)
    {
        _onSelect?.Invoke(data);
    }

    public void InitItemData(Action<InventoryItemData> onSelect)
    {
        _onSelect = onSelect;
        icon.sprite = data.icon;
        amount.text = data.maxCapacity.ToString();
        lockItemImage.gameObject.SetActive(true);
    }
}
