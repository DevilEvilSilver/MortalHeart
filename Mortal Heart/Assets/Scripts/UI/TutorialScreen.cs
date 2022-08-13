using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TutorialScreen : MonoBehaviour
{
    public void OnContinue()
    {
        GameController.Instance.ResetPlayerData();
        InventorySystem.Instance.InitState();
        SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}
