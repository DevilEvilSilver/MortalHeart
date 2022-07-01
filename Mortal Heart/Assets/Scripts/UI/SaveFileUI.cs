using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SaveFileUI : MonoBehaviour
{
    private const string NEW_DATA = "New Game";
    private const string OLD_DATA = "Continue";

    public SaveData saveData;
    public Button selectButton;
    public TMP_Text selectButtonText;

    public void UpdateData()
    {
        selectButton.onClick.RemoveAllListeners();

        if (saveData.isNewSaveData)
        {
            selectButtonText.text = NEW_DATA;
            selectButton.onClick.AddListener(CreateNewData);
        }
        else
        {
            selectButtonText.text = OLD_DATA;
            selectButton.onClick.AddListener(StartGame);
        }
    }

    public void CreateNewData()
    {
        saveData.isNewSaveData = false;
        UpdateData();
    }

    public void StartGame()
    {
        GameController.Instance.LoadSaveData(saveData);
        InventorySystem.Instance.InitState();
        SceneManager.LoadScene(GameUtils.SceneName.GAMEPLAY, LoadSceneMode.Single);
    }
}
