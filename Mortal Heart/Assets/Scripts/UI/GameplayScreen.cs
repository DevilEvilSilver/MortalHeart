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
    public TMP_Text hpText;

    [Header("Item")]
    public Image itemIcon;
    public TMP_Text itemAmount;

    [Header("Money")]
    public TMP_Text moneyAmount;

    public void OnHPChange(float value, float max, bool isAnim)
    {
        hpText.text = Mathf.CeilToInt(value) + "/" + Mathf.CeilToInt(max);
        hpProgress.transform.DOKill();
        hpProgress.transform.DOScaleX(value / max, isAnim ? 0.5f : 0f);
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
