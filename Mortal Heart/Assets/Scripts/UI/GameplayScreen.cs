using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameplayScreen : SingletonMonoBehaviour<GameplayScreen>
{
    [Header("Health")]
    public Image hpProgress;
    public TextMeshPro hpText;

    [Header("Item")]
    public Image itemIcon;
    public TextMeshPro itemAmount;

    [Header("Money")]
    public TextMeshPro moneyAmount;

    public void OnHPChange(int value, int max)
    {
        hpText.text = value + "/" + max;
        hpProgress.transform.DOKill();
        hpProgress.transform.DOScaleX(value / max, 0.5f);
    }

    public void OnItemChange(Sprite icon, int amount)
    {
        itemIcon.sprite = icon;
        itemAmount.text = amount.ToString();
    }

    public void OnMoneyChange(int amount)
    {
        moneyAmount.text = amount.ToString();
    }
}
