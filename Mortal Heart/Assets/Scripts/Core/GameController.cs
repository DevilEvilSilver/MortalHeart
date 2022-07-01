using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    UINavigation = 0, InCombat, Interact
}

public class GameController : SingletonMonoBehaviour<GameController>
{
    public GameState currentGameState;
    public SaveData currSaveData;
    public PlayerData playerData;

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;
        InputManager.Instance.Init();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            InventorySystem.Instance.OnChangeScene();
            DungeonController.Instance.SpawnRoom();
        }
    }

    public void LoadSaveData(SaveData data)
    {
        currSaveData = data;
        playerData.Hp = data.baseMaxHealth;
        playerData.Speed = data.baseSpeed;
    }
}
