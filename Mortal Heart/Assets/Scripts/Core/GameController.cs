using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum GameState
{
    UINavigation = 0, InCombat, Interact
}

public class GameController : SingletonMonoBehaviour<GameController>
{
    public AllBGMData allBGMData;
    public GameState currentGameState;
    public SaveData currSaveData;
    public PlayerData playerData;

    public bool IsPlaying { get; private set; }

    protected override void Init()
    {
        DontDestroyOnLoad(this.gameObject);

        IsPlaying = false;
        SceneManager.sceneLoaded += OnSceneLoad;
        InputManager.Instance.Init();
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        PauseGame(IsPlaying);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void Update()
    {
        if (IsPlaying)
        {
            playerData.PlayTime += Time.deltaTime;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        InputManager.Instance.pauseAction.performed -= OnPausePerformed;
        allBGMData.PlayBGM(scene.name);

        Time.timeScale = 1f;
        if (scene.name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            IsPlaying = true;
            InputManager.Instance.pauseAction.performed += OnPausePerformed;
            ChangeGameState(GameState.InCombat);
            InventorySystem.Instance.OnChangeScene();
            DungeonController.Instance.SpawnRoom();
        }
        else if (scene.name.Equals(GameUtils.SceneName.SHOP))
        {
            IsPlaying = true;
            InputManager.Instance.pauseAction.performed += OnPausePerformed;
            ChangeGameState(GameState.UINavigation);
            DungeonController.Instance.SpawnShop();
        }
        else if (scene.name.Equals(GameUtils.SceneName.MAIN_MENU))
        {
            IsPlaying = false;
            ChangeGameState(GameState.UINavigation);
        }
        else if (scene.name.Equals(GameUtils.SceneName.UPGRADE_CHARACTER))
        {
            IsPlaying = false;
            ChangeGameState(GameState.UINavigation);
        }
        else if (scene.name.Equals(GameUtils.SceneName.RESULT))
        {
            IsPlaying = false;
            ChangeGameState(GameState.UINavigation);
        }
    }

    public void ChangeGameState(GameState state)
    {
        if (currentGameState == state) return;
        InputManager.Instance.ActiveMap(state);
        switch (state)
        {
            default:
            case GameState.UINavigation:
                break;
            case GameState.Interact:
                break;
            case GameState.InCombat:
                break;
        }
    }

    public void LoadSaveData(SaveData data)
    {
        currSaveData = data;
    }

    public void SaveData(int experience = 0)
    {
        currSaveData.SavePlayData(playerData.PlayTime, playerData.EnemyKilled, experience);
    }

    public void ResetPlayerData()
    {
        playerData.Hp = GlobalData.GetMaxHealth();
        playerData.Mana = 0f;
        playerData.Speed = GlobalData.GetPlayerSpeed();

        playerData.PlayTime = 0f;
        playerData.EnemyKilled = 0;
    }

    public void PauseGame(bool isPause)
    {
        if (SceneManager.GetActiveScene().name.Equals(GameUtils.SceneName.GAMEPLAY))
        {
            IsPlaying = !isPause;
            Time.timeScale = isPause ? 0f : 1f;

            if (IsPlaying)
            {
                if (FindObjectOfType<BaseRoom>().CurrentRoomState == BaseRoom.RoomState.Normal)
                    ChangeGameState(GameState.Interact);
                else
                    ChangeGameState(GameState.InCombat);
            }
            else
                ChangeGameState(GameState.UINavigation);
        }
        else if (SceneManager.GetActiveScene().name.Equals(GameUtils.SceneName.SHOP))
        {
            IsPlaying = !isPause;
            Time.timeScale = isPause ? 0f : 1f;
        }
    }

}
