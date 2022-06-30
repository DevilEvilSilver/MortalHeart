using Sirenix.OdinInspector;

public class BaseEnemyDeathState : BaseEnemyState
{
    [ValueDropdown("AllAnimations")]
    public string deathAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;

        actorController.OnEnemyDeath?.Invoke();
        actorController.animator.Play(deathAnim);
    }
}