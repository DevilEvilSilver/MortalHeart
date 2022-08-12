using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UniRx;

public class BaseEnemyDeathState : BaseEnemyState
{
    [ValueDropdown("AllAnimations")]
    public string deathAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        actorController.GetComponent<Collider>().enabled = false;
        actorController.Agent.enabled = false;
        actorController.isActive = false;
        actorController.OnEnemyDeath?.Invoke();
        InventorySystem.Instance.UpdatePlayerMoney(
            (int)(actorController.moneyDrop * (1 + GlobalData.GetBonusGoldPercent())));
        actorController.animator.CrossFadeInFixedTime(deathAnim, 0.2f);

        Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(_ =>
        {
            SimplePool.Despawn(actorController.gameObject);
        });
    }
}