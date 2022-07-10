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
                isUsingSuper = false;
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

    public void ChangeMana(float change, bool isAnim = true)
    {
        if (fsm.currentState == deathState) return;

        playerData.Mana += change;
        if (playerData.Mana > 100F)
            playerData.Mana = 100F;
        else if (playerData.Mana < 0f)
            playerData.Mana = 0f;
        GameplayScreen.Instance.OnManaChange(playerData.Mana, 100F, isAnim);
    }
}
