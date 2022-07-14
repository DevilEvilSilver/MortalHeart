using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShopScreen : MonoBehaviour
{
    //public 
    public TMP_Text moneyText;

    public ItemUI[] normalItems;
    public ItemUI[] rareItems;

    public GameObject descriptionPanel;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public Button buyBtn;

    private ItemData currentSelectItem;
    
    private void OnEnable()
    {


        currentSelectItem = null;
        ResetUI();
    }

    private void ResetUI()
    {
        //moneyText.text = GameController.Instance.currSaveData.money.ToString();

        //if (currentSelectSkill == null)
        //    descriptionPanel.SetActive(false);
        //else
        //{
        //    descriptionText.text = currentSelectSkill.description;
        //    priceText.text = currentSelectSkill.GetNextLevelPrice().ToString();

        //    bool isUpgradeAble = true;
        //    foreach (var require in currentSelectSkill.requirements)
        //    {
        //        if (require.level <= 0)
        //            isUpgradeAble = false;
        //    }
        //    if (isUpgradeAble && GameController.Instance.currSaveData.money > currentSelectSkill.GetNextLevelPrice())
        //        upgradeBtn.interactable = true;
        //    else
        //        upgradeBtn.interactable = false;

        //    upgradeBtn.onClick.RemoveAllListeners();
        //    upgradeBtn.onClick.AddListener(() => OnUpgradeSkill(currentSelectSkill));

        //    descriptionPanel.SetActive(true);
        //}
    }

    public void OnSelectSkill(UpgradeData data)
    {
        //currentSelectSkill = data;
        //ResetUI();
    }

    private void OnUpgradeSkill(UpgradeData data)
    {
        GameController.Instance.currSaveData.money -= data.GetNextLevelPrice();
        GameController.Instance.currSaveData.SaveUpgrade(data);
        data.UpgradeNextLevel();
        ResetUI();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(GameUtils.SceneName.MAIN_MENU, LoadSceneMode.Single);
    }

    public void OnPlay()
    {
        InventorySystem.Instance.InitState();
        SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}
