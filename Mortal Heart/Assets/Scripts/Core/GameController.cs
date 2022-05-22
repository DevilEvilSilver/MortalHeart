using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : SingletonMonoBehaviour<GameController>
{
    public enum GameState
    {
        UINavigation = 0, InCombat, OutCombat
    }

    public GameState currentGameState;

    public override void Awake()
    {
        if (instance == null)
        {
            instance = this as GameController;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;
        }
        else
        {
            Destroy(gameObject);
        }
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
