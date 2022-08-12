using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameplayScreen : SingletonMonoBehaviour<GameplayScreen>
{
    [Header("HUD")]
    public Image hpProgress;
    public TMP_Text hpText;

    public Image manaProgress;

    public Image itemIcon;
    public TMP_Text itemAmount;

    public TMP_Text moneyAmount;

    [Header("PauseScreen")]
    public GameObject pausePanel;
    public GameObject optionPanel;

    protected override void Init()
    {
        InputManager.Instance.pauseAction.performed += OnPause;
    }

    private void OnDestroy()
    {
        InputManager.Instance.pauseAction.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
    }

    public void OnPlay()
    {
        pausePanel.SetActive(false);
        GameController.Instance.PauseGame(false);
    }

    public void OnHPChange(float value, float max, bool isAnim)
    {
        hpText.text = Mathf.CeilToInt(value) + "/" + Mathf.CeilToInt(max);
        hpProgress.transform.DOKill();
        hpProgress.transform.DOScaleX(value / max, isAnim ? 0.5f : 0f);
    }

    public void OnManaChange(float value, float max, bool isAnim)
    {
        manaProgress.transform.DOKill();
        manaProgress.transform.DOScaleX(value / max, isAnim ? 0.5f : 0f);
    }

    public void OnItemChange(Sprite icon, int amount)
    {
        itemIcon.sprite = icon;
        itemAmount.text = amount.ToString();
        itemAmount.gameObject.SetActive(true);
    }

    public void OnNoneItem()
    {
        itemIcon.sprite = null;
        itemAmount.gameObject.SetActive(false);
    }

    public void OnMoneyChange(int amount)
    {
        moneyAmount.text = amount.ToString();
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
