using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SkillUI : MonoBehaviour, ISelectHandler
{
    public UpgradeData data;

    public Image icon;
    public Image lockSkillImage;
    public TMP_Text level;

    private void Awake()
    {
        InitSkillData();
    }

    public void OnSelect(BaseEventData eventData)
    {
        UpgradeCharacterScreen.Instance.OnSelectSkill(data);
    }

    public void InitSkillData()
    {
        icon.sprite = data.icon;
        if (data.level <= 0)
            lockSkillImage.gameObject.SetActive(true);
        else
            lockSkillImage.gameObject.SetActive(false);

        if (data.maxLevel > 1 && data.level != 0)
        {
            level.text = data.level.ToString();
            level.gameObject.SetActive(true);
        }
        else
            level.gameObject.SetActive(false);
    }
}
