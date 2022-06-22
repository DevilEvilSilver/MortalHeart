using Sirenix.OdinInspector;

public class BaseDeathState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string deathAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;

        actorController.animator.Play(deathAnim);
    }
}