using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class LukeController : MainCharacterController
{
    internal bool isUsingSuper;

    protected override void OnActive()
    {
        ChangeMana(0f, false);
        isUsingSuper = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isUsingSuper)
        {
            ChangeMana(-Time.deltaTime * 5f);

            if (playerData.Mana <= 0f)
            {
                GlobalData.BonusAttackPercent -= 0.5f;
                isUsingSuper = false;
            }
        }
    }

    protected override void OnAttackC(InputAction.CallbackContext ctx)
    {
        if (playerData.Mana >= 100f)
            base.OnAttackC(ctx);
    }

    public void ActivateSuper()
    {
        isUsingSuper = true;
        canTakeDamage = false;

        GlobalData.BonusAttackPercent += 0.5f;
    }

    public override void ToggleInvulnerable(bool isInvulnerable)
    {
        if (isUsingSuper) return;

        canTakeDamage = !isInvulnerable;
    }

    public override void OnDealDamage(float damage)
    {
        ChangeMana(damage / 10f);
    }
}
