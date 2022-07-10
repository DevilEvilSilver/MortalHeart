using Sirenix.OdinInspector;
using UnityEngine;

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
        actorController.animator.Play(deathAnim);
    }
}