using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseDeathState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string deathAnim;
    public float delayTillResult;

    private IDisposable _disposable;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        actorController.GetComponent<Collider>().enabled = false;
        actorController.Agent.enabled = false;
        actorController.animator.CrossFadeInFixedTime(deathAnim, 0.2f);

        _disposable = Observable.Timer(TimeSpan.FromSeconds(delayTillResult)).Subscribe(_ =>
        {
            SceneManager.LoadScene(GameUtils.SceneName.RESULT, LoadSceneMode.Single);
        });
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
    }
}