using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class SaveFileUI : MonoBehaviour
{
    private const string NEW_DATA = "New Game";
    private const string OLD_DATA = "Continue";

    public SaveData saveData;
    public Button selectButton;
    public Button deleteButton;
    public TMP_Text selectButtonText;

    public GameObject statsGO;
    public TMP_Text timeText;
    public TMP_Text moneyText;
    public TMP_Text killedText;

    public void UpdateData()
    {
        selectButton.onClick.RemoveAllListeners();

        if (saveData.isNewSaveData)
        {
            selectButtonText.text = NEW_DATA;
            selectButton.onClick.AddListener(CreateNewData);
            deleteButton.gameObject.SetActive(false);

            statsGO.gameObject.SetActive(false);
        }
        else
        {
            selectButtonText.text = OLD_DATA;
            selectButton.onClick.AddListener(StartGame);
            deleteButton.gameObject.SetActive(true);

            TimeSpan time = TimeSpan.FromSeconds(saveData.playTime);
            timeText.text = time.ToString(@"mm\:ss");
            moneyText.text = saveData.experience.ToString();
            killedText.text = saveData.enemyKilled.ToString();
            statsGO.gameObject.SetActive(true);
        }
    }

    public void CreateNewData()
    {
        saveData.isNewSaveData = false;
        UpdateData();
    }

    public void DeleteData()
    {
        saveData.ResetData();
        UpdateData();
    }

    public void StartGame()
    {
        saveData.LoadData();
        GameController.Instance.LoadSaveData(saveData); 
        SceneManager.LoadScene(GameUtils.SceneName.UPGRADE_CHARACTER, LoadSceneMode.Single);
    }
}
