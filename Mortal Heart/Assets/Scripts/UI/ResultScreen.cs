using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResultScreen : MonoBehaviour
{
    private const string WIN = "You Win";
    private const string LOSE = "You Lose";

    public PlayerData playerData;

    //public Image result;
    //public Image rank;
    public TMP_Text resultText;
    public TMP_Text timeText;
    public TMP_Text moneyText;
    public TMP_Text killedText;
    public TMP_Text scoreText;

    private void Awake()
    {
        if (playerData.Hp > 0f)
        {
            resultText.text = WIN;
        }
        else
        {
            resultText.text = LOSE;
        }

        TimeSpan time = TimeSpan.FromSeconds(playerData.PlayTime);
        timeText.text = time.ToString(@"mm\:ss");
        moneyText.text = InventorySystem.Instance.money.ToString();
        killedText.text = playerData.EnemyKilled.ToString();

        scoreText.text = ScoreCalculation(playerData.PlayTime, InventorySystem.Instance.money, playerData.EnemyKilled);
    }

    private string ScoreCalculation(float time, int money, int enemyKilled)
    {
        int score = 0;

        score += money * 10;
        score += enemyKilled * 20;
        if (playerData.Hp > 0f)
            score += 1800 - Mathf.FloorToInt(time);

        return score.ToString();
    }


    public void OnBack()
    {
        SceneManager.LoadScene(GameUtils.SceneName.MAIN_MENU, LoadSceneMode.Single);
    }
}