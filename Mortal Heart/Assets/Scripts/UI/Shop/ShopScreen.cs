using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShopScreen : SingletonMonoBehaviour<ShopScreen>
{
    [Header("ShopScreen")]
    public TMP_Text moneyText;

    public GameObject descriptionPanel;
    public TMP_Text descriptionText;
    public TMP_Text priceText;
    public Button buyBtn;

    [Header("PauseScreen")]
    public GameObject pausePanel;
    public GameObject optionPanel;

    protected RoomProperties roomProperties;

    protected override void Init()
    {
        base.Init();
        ResetUI();
    }

    public void InitState(RoomProperties properties)
    {
        roomProperties = properties;
    }

    private void ResetUI(ItemUI currentSelectItem = null)
    {
        moneyText.text = InventorySystem.Instance.money.ToString();

        if (currentSelectItem == null)
            descriptionPanel.SetActive(false);
        else
        {
            descriptionText.text = currentSelectItem.data.description;
            priceText.text = currentSelectItem.data.price.ToString();
            buyBtn.interactable = ShopController.Instance.IsAffordable(currentSelectItem.data);
            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() => OnBuy(currentSelectItem));

            descriptionPanel.SetActive(true);
        }
    }

    public void OnSelectSkill(ItemUI item)
    {
        ResetUI(item);
    }

    private void OnBuy(ItemUI item)
    {
        ShopController.Instance.BuyItem(item.data);
        ResetUI();
        item.OnBuyItem();
    }

    public void OnContinue()
    {
        var nextRoom = DungeonController.Instance.GetRoomProperties(roomProperties.nextRooms[0]);
        DungeonController.Instance.GoToNextRoom(nextRoom);
        if (nextRoom == null)
        {
            if (DungeonController.Instance.GoToNextFloor()) // check if reach final floor
                return;
            else
                SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
        }

        switch (nextRoom.type)
        {
            default:
            case RoomType.Normal:
            case RoomType.Elite:
            case RoomType.Boss:
                SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
                break;
            case RoomType.Shop:
                SceneManager.LoadScene(GameUtils.SceneName.SHOP, LoadSceneMode.Single);
                break;
        }
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
    }

    public void OnPlay()
    {
        pausePanel.SetActive(false);
        GameController.Instance.PauseGame(true);
    }

    public void OnOptionSelect()
    {
        optionPanel.SetActive(true);
    }

    public void OnOptionQuit()
    {
        optionPanel.SetActive(false);
    }

    public void OnQuit()
    {
        GameController.Instance.SaveData();
        SceneManager.LoadScene(GameUtils.SceneName.MAIN_MENU, LoadSceneMode.Single);
    }
}
