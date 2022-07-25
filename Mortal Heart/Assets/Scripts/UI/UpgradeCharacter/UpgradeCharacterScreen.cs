using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UpgradeCharacterScreen : SingletonMonoBehaviour<UpgradeCharacterScreen>
{
    //public PlayerData playerData;

    public TMP_Text moneyText;

    public GameObject descriptionPanel;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public Button upgradeBtn;

    protected override void Init()
    {
        ResetUI();
    }

    private void ResetUI(SkillUI currentSelectSkill = null)
    {
        moneyText.text = GameController.Instance.currSaveData.experience.ToString();

        if (currentSelectSkill == null)
            descriptionPanel.SetActive(false);
        else
        {
            descriptionText.text = currentSelectSkill.data.description;
            priceText.text = currentSelectSkill.data.GetNextLevelPrice().ToString();
            upgradeBtn.interactable = UpgradeSystem.Instance.IsSkillUpgradeable(currentSelectSkill.data);
            upgradeBtn.onClick.RemoveAllListeners();
            upgradeBtn.onClick.AddListener(() => OnUpgradeSkill(currentSelectSkill));

            descriptionPanel.SetActive(true);
        }
    }

    public void OnSelectSkill(SkillUI skill)
    {
        ResetUI(skill);
    }

    private void OnUpgradeSkill(SkillUI skill)
    {
        UpgradeSystem.Instance.UpgradeSkill(skill.data);
        ResetUI();
        skill.InitState();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(GameUtils.SceneName.MAIN_MENU, LoadSceneMode.Single);
    }

    public void OnPlay()
    {
        GameController.Instance.ResetPlayerData();
        InventorySystem.Instance.InitState();
        SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}
