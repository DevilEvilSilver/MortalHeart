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
            DungeonController.Instance.SpawnRoom();
        }
    }
}
